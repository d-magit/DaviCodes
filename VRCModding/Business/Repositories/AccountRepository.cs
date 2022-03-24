using VRCModding.Business.Services;
using VRCModding.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace VRCModding.Business.Repositories;

public class AccountRepository {
	private readonly DisplayNameService displayNameService;
	private readonly UsedDisplayNameRepository usedDisplayNameRepository;
	private readonly AppDbContext dbContext;

    public AccountRepository(DisplayNameService displayNameService, UsedDisplayNameRepository usedDisplayNameRepository, AppDbContext dbContext) {
	    this.displayNameService = displayNameService;
	    this.usedDisplayNameRepository = usedDisplayNameRepository;
	    this.dbContext = dbContext;
    }

    public async Task<Account?> TryGetAsync(string uid, bool includeDisplayName = false) {
	    var initialQuery = dbContext.Accounts.AsQueryable();
	    
	    if (includeDisplayName) {
		    initialQuery = initialQuery.Include(a => a.CurrentDisplayName);
	    }
	    
	    return await initialQuery.AsQueryable().FirstOrDefaultAsync(a => a.Id == uid);
    }
    
    public async Task CreateAsync(string uid, Guid userGuid, string? displayName = null) {
	    var accountEntity = new Account {
		    Id = uid,
		    UserFK = userGuid
	    };
        
	    if (!string.IsNullOrEmpty(displayName)) {
		    accountEntity.DisplayNameFK = displayName;
		    var displayNameEntity = await displayNameService.ReserveAsync(displayName);
		    await usedDisplayNameRepository.CreateAsync(accountEntity.Id, displayNameEntity.Name);
	    } else {
		    // Todo: If this is is null, then the person might be trying a funny! Save, but issue warning...
	    }

	    await dbContext.AddAsync(accountEntity);
    }
}
