using VRCModding.Api;
using VRCModding.Business.Repositories;
using VRCModding.Entities;

namespace VRCModding.Business.Services;

public class HwidService {
    private readonly HwidRepository hwidRepository;
    private readonly AppDbContext dbContext;
    private readonly ExceptionBuilder exceptionBuilder;

    public HwidService(HwidRepository hwidRepository, AppDbContext dbContext, ExceptionBuilder exceptionBuilder) {
        this.hwidRepository = hwidRepository;
        this.dbContext = dbContext;
        this.exceptionBuilder = exceptionBuilder;
    }

    public async Task<Hwid?> GetAsync(string? hwid) {
	    if (string.IsNullOrEmpty(hwid)) return null;
	    return await hwidRepository.TryGetAsync(hwid);
    }

    public async Task<Hwid?> CreateAsync(string? hwid, Guid userGuid) {
	    if (string.IsNullOrEmpty(hwid))
		    throw exceptionBuilder.Api(ErrorCodes.HwidIsRequired);

	    var hwidEntity = await hwidRepository.CreateAsync(hwid, userGuid);
	    await dbContext.SaveChangesAsync();

	    return hwidEntity;
    }
}
