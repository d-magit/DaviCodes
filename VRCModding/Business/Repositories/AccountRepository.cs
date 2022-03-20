using VRCModding.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace VRCModding.Business.Repositories;

public class AccountRepository {
    private readonly AppDbContext dbContext;

    public AccountRepository(AppDbContext dbContext) {
        this.dbContext = dbContext;
    }

    public async Task<Account?> TryGetAsync(string uid, bool includeDisplayName = false) {
	    var initialQuery = dbContext.Accounts;
		    //.Include(a => a.User);
	    IIncludableQueryable<Account, DisplayName?>? displayNameQuery = null; 
	    if (includeDisplayName) {
		    displayNameQuery = initialQuery
			    .Include(a => a.CurrentDisplayName);
	    }
	    return await (includeDisplayName ? displayNameQuery!.AsQueryable() : initialQuery.AsQueryable())
		    .FirstOrDefaultAsync(a => a.Id == uid);
    }
    
    public async Task<Account> CreateAsync(string uid, Guid userGuid, string? displayName = null) {
        var accountInfo = new Account {
            Id = uid,
            UserFK = userGuid,
            DisplayNameFK = displayName
        };
        
        if (!string.IsNullOrEmpty(displayName)) { // Todo
	        // Try get DisplayNameEntity
	        // if null create one and cache it
	        // if not null cache it, and get it's previous account and update it's current to null (unknown)
	        // Create UsedDisplayNameEntity
        } else {
	        // If this is is null, then the person might be trying a funny! Save, but issue warning...
        }

        await dbContext.AddAsync(accountInfo);

        return accountInfo;
    }
    
    // Todo: UpdateAsync. This should update the DateTime from UsedDisplayName. Every time player logs in, check if username changed and properly deal. In case it did, update current, getting, if not creating, the UsedDisplayName entity and add it to history
}
