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
			.Include(i => i.User)
			.FirstOrDefaultAsync(i => i.Id == ip);
    
	public async Task<Ip> CreateAsync(string? ip, Guid guid) {
		var ipEntity = new Ip {
			Id = ip,
			UserFK = guid
		};

		await dbContext.AddAsync(ipEntity);

		return ipEntity;
	}
}
