using VRCModding.Business.Repositories;
using VRCModding.Entities;

namespace VRCModding.Business.Services;

public class AccountService {
    private readonly AccountRepository accountRepository;
    private readonly AppDbContext dbContext;

    public AccountService(AccountRepository accountRepository, AppDbContext dbContext) {
        this.accountRepository = accountRepository;
        this.dbContext = dbContext;
    }

    public async Task<Account?> GetAsync(string? uid) {
        if (string.IsNullOrEmpty(uid)) return null;
        return await accountRepository.TryGetAsync(uid); // Todo: If uid is not null, but didn't find in DB, then something went wrong (maybe someone tryna be funny..?), create error code.
    }
    
    // Todo: UpdateAsync. Every time player logs in, check if username changed and properly deal. In case it did, update current, getting if not creating UsedDisplayName entity and add history
    public async Task<Account?> CreateAsync(string accountInfoId, Guid userEntityId) {
	    throw new NotImplementedException(); // Todo: Implement Account creation
    }
}
