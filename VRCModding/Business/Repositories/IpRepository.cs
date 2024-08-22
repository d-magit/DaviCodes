using VRCModding.Entities;
using Microsoft.EntityFrameworkCore;

namespace VRCModding.Business.Repositories;

public class IpRepository {
	private readonly AppDbContext dbContext;

	public IpRepository(AppDbContext dbContext) {
		this.dbContext = dbContext;
	}
    
	public async Task<Ip?> TryGetAsync(string ip) =>
		await dbContext.Ips.FirstOrDefaultAsync(i => i.Id == ip);
    
	public async Task<Ip> CreateAsync(string ip) {
		var ipCreated = new Ip() {
			Id = ip
		};

		await dbContext.AddAsync(ipCreated);

		return ipCreated;
	}
}
