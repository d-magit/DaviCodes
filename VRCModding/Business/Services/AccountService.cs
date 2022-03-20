using VRCModding.Api;
using VRCModding.Business.Repositories;
using VRCModding.Entities;

namespace VRCModding.Business.Services;

public class AccountService {
    private readonly AccountRepository accountRepository;
    private readonly AppDbContext dbContext;
    private readonly ExceptionBuilder exceptionBuilder;

    public AccountService(AccountRepository accountRepository, AppDbContext dbContext, ExceptionBuilder exceptionBuilder) {
        this.accountRepository = accountRepository;
        this.dbContext = dbContext;
        this.exceptionBuilder = exceptionBuilder;
    }

    public async Task<Account?> TryGetAsync(string? uid) {
        if (string.IsNullOrEmpty(uid)) return null;
        var accountEntity = await accountRepository.TryGetAsync(uid);
        if (accountEntity == null) {} // Todo: If uid is not null, but didn't find in DB, then something went wrong (maybe someone tryna be funny..?), add discord notification.
        return accountEntity; 
    }
    
    public async Task<Account?> CreateAsync(string? uid, Guid userGuid, string? displayName = null) {
	    if (string.IsNullOrEmpty(uid))
		    throw exceptionBuilder.Api(ErrorCodes.UserIdIsRequired);

	    var accountEntity = await accountRepository.CreateAsync(uid, userGuid, displayName);
	    await dbContext.SaveChangesAsync();

	    return accountEntity;
    }
}
