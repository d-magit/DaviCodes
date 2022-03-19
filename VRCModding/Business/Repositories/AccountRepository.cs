using VRCModding.Entities;
using Microsoft.EntityFrameworkCore;

namespace VRCModding.Business.Repositories;

public class AccountRepository {
    private readonly AppDbContext dbContext;

    public AccountRepository(AppDbContext dbContext) {
        this.dbContext = dbContext;
    }

    public async Task<Account?> TryGetAsync(string uid) =>
	    await dbContext.Accounts
		    .Include(a => a.User)
		    .FirstOrDefaultAsync(a => a.Id == uid);
    
    public async Task<Account> CreateAsync(string? uid, Guid guid, string? displayName = null) {
        var accountInfo = new Account {
            Id = uid,
            UserFK = guid,
            DisplayNameFK = displayName // Todo: if this is is null, then the person might be trying a funny!
        };

        await dbContext.AddAsync(accountInfo);

        return accountInfo;
    }
    
    // Todo: UpdateAsync. REMEMBER: This should also add the Ips or Hwids that got added to DB, to the correct User if successfully authed. 
}
