using VRCModding.Business.Repositories;
using VRCModding.Entities;

namespace VRCModding.Business.Services;

public class AccountService {
	private readonly DisplayNameService displayNameService;
	private readonly AccountRepository accountRepository;
	private readonly UsedDisplayNameRepository usedDisplayNameRepository;
	private readonly AppDbContext dbContext;
    private readonly ExceptionBuilder exceptionBuilder;

    public AccountService(DisplayNameService displayNameService, AccountRepository accountRepository, UsedDisplayNameRepository usedDisplayNameRepository, AppDbContext dbContext, ExceptionBuilder exceptionBuilder) {
	    this.displayNameService = displayNameService;
	    this.accountRepository = accountRepository;
	    this.usedDisplayNameRepository = usedDisplayNameRepository;
	    this.dbContext = dbContext;
        this.exceptionBuilder = exceptionBuilder;
    }

    public async Task<Account?> TryGetAsync(string? uid, bool includeDisplayName = false) {
        if (string.IsNullOrEmpty(uid)) return null;
        var accountEntity = await accountRepository.TryGetAsync(uid, includeDisplayName);
        if (accountEntity == null) {} // Todo: If uid is not null, but didn't find in DB, then something went wrong (maybe someone tryna be funny..?), add discord notification.
        return accountEntity; 
    }
    
    public async Task CreateAsync(string accountId, Guid userId, string displayName) { // If got here, nothing is not null or empty. This is temporary. If uid can ever be null, remove check from Controller's CreateOrUpdateAsync
	    await accountRepository.CreateAsync(accountId, userId, displayName);
	    await dbContext.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(Account accountEntity, string displayName) { // If got here, displayName is not null or empty (for now).
	    var displayNameEntity = accountEntity.CurrentDisplayName;
	    if (accountEntity.DisplayNameFK != displayName) {
		    displayNameEntity = await displayNameService.ReserveAsync(displayName);
		    accountEntity.DisplayNameFK = displayName;
	    }
	    
	    var usedDisplayNameEntity = await usedDisplayNameRepository.TryGetAsync(accountEntity.Id, displayNameEntity!.Name);
	    if (usedDisplayNameEntity == null)
		    await usedDisplayNameRepository.CreateAsync(accountEntity.Id, displayNameEntity.Name);
	    else
		    usedDisplayNameEntity.LastUsage = DateTime.UtcNow;
	    
	    await dbContext.SaveChangesAsync();
    }
}
