using VRCModding.Entities;
using Microsoft.EntityFrameworkCore;

namespace VRCModding.Business.Repositories;

public class UsedDisplayNameRepository {
	private readonly AppDbContext dbContext;

	public UsedDisplayNameRepository(AppDbContext dbContext) {
		this.dbContext = dbContext;
	}
    
	public async Task<UsedDisplayName?> TryGetAsync(string accountId, string displayName) => // None of these will be null (temporary).
		await dbContext.UsedDisplayNames
			.FirstOrDefaultAsync(u => u.AccountFK == accountId && u.DisplayNameFK == displayName);
    
	public async Task CreateAsync(string accountId, string displayName) { // None of these will be null (temporary).
		var usedDisplayNameEntity = new UsedDisplayName {
			AccountFK =	accountId,
			DisplayNameFK = displayName
		};

		await dbContext.AddAsync(usedDisplayNameEntity);
	}
}
