using VRCModding.Entities;
using Microsoft.EntityFrameworkCore;

namespace VRCModding.Business.Repositories;

public class DisplayNameRepository {
	private readonly AppDbContext dbContext;

	public DisplayNameRepository(AppDbContext dbContext) {
		this.dbContext = dbContext;
	}
    
	public async Task<DisplayName?> TryGetAsync(string displayName) =>
		await dbContext.DisplayNames
			.Include(d => d.CurrentAccount)
			.FirstOrDefaultAsync(d => d.Name == displayName);
    
	public async Task<DisplayName> CreateAsync(string displayName) {
		var displayNameEntity = new DisplayName {
			Name = displayName
		};

		await dbContext.AddAsync(displayNameEntity);

		return displayNameEntity;
	}
}
