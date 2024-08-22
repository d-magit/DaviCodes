using VRCModding.Business.Repositories;
using VRCModding.Entities;

namespace VRCModding.Business.Services;

public class HwidService {
    private readonly HwidRepository hwidRepository;
    private readonly AppDbContext dbContext;

    public HwidService(HwidRepository hwidRepository, AppDbContext dbContext) {
        this.hwidRepository = hwidRepository;
        this.dbContext = dbContext;
    }

    public async Task<Hwid?> GetOrCreateAsync(string? hid) {
        if (string.IsNullOrEmpty(hid)) return null;
        
        var hwid = await hwidRepository.TryGetAsync(hid);

        if (hwid != null) return hwid;
        
        hwid = await hwidRepository.CreateAsync(hid);
        await dbContext.SaveChangesAsync();

        return hwid;
    }
}
