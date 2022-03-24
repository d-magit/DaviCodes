using VRCModding.Entities;
using Microsoft.EntityFrameworkCore;

namespace VRCModding.Business.Repositories;

public class DisplayNameRepository {
	private readonly AppDbContext dbContext;

	public DisplayNameRepository(AppDbContext dbContext) {
		this.dbContext = dbContext;
	}

	public async Task<DisplayName?> TryGetAsync(string displayName, bool includeCurrAcc = false) {
		var query = dbContext.DisplayNames.AsQueryable();

		if (includeCurrAcc) {
			query = query.Include(d => d.CurrentAccount);
		}

		return await query.FirstOrDefaultAsync(d => d.Name == displayName);
	}
    
	public async Task<DisplayName> CreateAsync(string displayName) {
		var displayNameEntity = new DisplayName {
			Name = displayName
		};

		await dbContext.AddAsync(displayNameEntity);

		return displayNameEntity;
	}
}
