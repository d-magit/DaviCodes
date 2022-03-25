using VRCModding.Entities;
using Microsoft.EntityFrameworkCore;

namespace VRCModding.Business.Repositories;

public class IpRepository {
	private readonly AppDbContext dbContext;

	public IpRepository(AppDbContext dbContext) {
		this.dbContext = dbContext;
	}
    
	public async Task<Ip?> TryGetAsync(string ip) =>
		await dbContext.Ips
			//.Include(i => i.User)
			.FirstOrDefaultAsync(i => i.Id == ip);
    
	public async Task CreateAsync(string ip, Guid userId) {
		var ipEntity = new Ip {
			Id = ip,
			UserFK = userId
		};

		await dbContext.AddAsync(ipEntity);
	}
}
