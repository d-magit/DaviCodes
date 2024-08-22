using VRCModding.Business.Repositories;
using VRCModding.Entities;

namespace VRCModding.Business.Services;

public class AccountInfoService {
    private readonly AccountInfoRepository accountInfoRepository;
    private readonly AppDbContext dbContext;

    public AccountInfoService(AccountInfoRepository accountInfoRepository, AppDbContext dbContext) {
        this.accountInfoRepository = accountInfoRepository;
        this.dbContext = dbContext;
    }

    public async Task<AccountInfo?> GetAsync(string? uid) {
        if (string.IsNullOrEmpty(uid)) return null;
        
        var accountInfo = await accountInfoRepository.TryGetAsync(uid);

        return accountInfo ?? null; // Todo: If uid is not null, but didn't find in DB, then something went wrong. Create error code.
    }
    
    // Todo: UpdateAsync. Every time player logs in, check if username changed and properly deal. In case it did, update current, getting if not creating UsedDisplayName entity and add history
}
