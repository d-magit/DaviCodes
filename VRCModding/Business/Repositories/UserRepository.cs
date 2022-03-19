using VRCModding.Entities;
using Microsoft.EntityFrameworkCore;

namespace VRCModding.Business.Repositories; 

public class UserRepository {
	private readonly AppDbContext dbContext;

	public UserRepository(AppDbContext dbContext) {
		this.dbContext = dbContext;
	}
	
	public async Task<User?> TryGetByGuidAsync(Guid userGuid) =>
		await dbContext.Users
			.FirstOrDefaultAsync(u => u.Id == userGuid);
	
	public async Task<User?> TryGetByUsernameAsync(string username) =>
		await dbContext.Users
			.FirstOrDefaultAsync(u => u.Name == username);
	
	public async Task<User> CreateAsync(string? userName = null) {
		var userEntity = new User {
			Id = Guid.NewGuid(),
			Name = userName
		};
		
		await dbContext.AddAsync(userEntity);

		return userEntity;
	}
}
