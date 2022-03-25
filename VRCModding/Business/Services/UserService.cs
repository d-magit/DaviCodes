using VRCModding.Api;
using VRCModding.Business.Repositories;
using VRCModding.Entities;

namespace VRCModding.Business.Services; 

public class UserService {
	private readonly UserRepository userRepository;
	private readonly HwidRepository hwidRepository;
	private readonly AccountRepository accountRepository;
	private readonly IpRepository ipRepository;
	private readonly AppDbContext dbContext;

	public UserService(UserRepository userRepository, HwidRepository hwidRepository, AccountRepository accountRepository, IpRepository ipRepository, AppDbContext dbContext) {
		this.userRepository = userRepository;
		this.hwidRepository = hwidRepository;
		this.accountRepository = accountRepository;
		this.ipRepository = ipRepository;
		this.dbContext = dbContext;
	}
	
	public async Task<User?> TryGetByIdAsync(Guid userId) {
		return await userRepository.TryGetByGuidAsync(userId);
	}
	
	public async Task<User> CreateAsync((string? id, object? userInfo)[] infoArray, string? userName = null) {
		var userEntity = await userRepository.CreateAsync(userName);

		var id = infoArray[0].id;
		if (!string.IsNullOrEmpty(id)) {
			await hwidRepository.CreateAsync(id, userEntity.Id);
		}
		id = infoArray[1].id;
		if (!string.IsNullOrEmpty(id)) {
			await accountRepository.CreateAsync(id, userEntity.Id);
		}
		id = infoArray[2].id;
		if (!string.IsNullOrEmpty(id)) {
			await ipRepository.CreateAsync(id, userEntity.Id);
		}
		
		await dbContext.SaveChangesAsync();

		return userEntity;
	}

	public async Task<User> UpdateAsync(User userEntity, (string? id, object? userInfo)[]? infoArray = null, string? userName = null, Permissions? permissions = null, bool isLogin = false) {
		var containsNewData = !string.IsNullOrEmpty(userName);
		if (containsNewData) {
			userEntity.Name = userName;
		}

		if (isLogin) {
			containsNewData = true;
			userEntity.LastLogin = DateTime.UtcNow;
		}

		for (var i = 0; i < infoArray?.Length; i++) {
			var (id, userInfo) = infoArray[i];
			if (string.IsNullOrEmpty(id)) continue;
			containsNewData = true;
			switch (i)
			{
				case 0: // New HWID?
					if (userInfo == null)
						await hwidRepository.CreateAsync(id, userEntity.Id);
					else
						((Hwid)userInfo).LastLogin = DateTime.UtcNow;
					break;
				case 1: // New Account?
					if (userInfo == null)
						await accountRepository.CreateAsync(id, userEntity.Id);
					else
						((Account)userInfo).LastLogin = DateTime.UtcNow;
					break;
				case 2: // New IP?
					if (userInfo == null)
						await ipRepository.CreateAsync(id, userEntity.Id);
					else
						((Ip)userInfo).LastLogin = DateTime.UtcNow;
					break;
			}
		}

		if (permissions != null) {
			containsNewData = true;
			userEntity.Permissions = permissions.Value;
		}
		
		if (containsNewData) {
			await dbContext.SaveChangesAsync();
		}
		
		return userEntity;
	}
}
