using VRCModding.Entities;
using Microsoft.EntityFrameworkCore;

namespace VRCModding.Business.Repositories;

public class AccountInfoRepository {
    private readonly AppDbContext dbContext;

    public AccountInfoRepository(AppDbContext dbContext) {
        this.dbContext = dbContext;
    }
    
    public async Task<AccountInfo?> TryGetAsync(string uid) =>
        await dbContext.AccountInfos.FirstOrDefaultAsync(a => a.Id == uid);
    
    public async Task<AccountInfo> CreateAsync(string uid, string displayName) {
        var accountInfo = new AccountInfo() {
            Id = uid,
            DisplayNameFK = displayName
        };

        await dbContext.AddAsync(accountInfo);

        return accountInfo;
    }
    
    // Todo: UpdateAsync. REMEMBER: This should also add the Ips or Hwids that got added to DB, to the correct User if successfully authed. 
}
