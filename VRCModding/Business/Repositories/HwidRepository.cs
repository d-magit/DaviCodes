using VRCModding.Entities;
using Microsoft.EntityFrameworkCore;

namespace VRCModding.Business.Repositories;

public class HwidRepository {
    private readonly AppDbContext dbContext;

    public HwidRepository(AppDbContext dbContext) {
        this.dbContext = dbContext;
    }
    
    public async Task<Hwid?> TryGetAsync(string hid) =>
        await dbContext.Hwids.FirstOrDefaultAsync(h => h.Id == hid);
    
    public async Task<Hwid> CreateAsync(string hid) {
        var hwid = new Hwid() {
            Id = hid
        };

        await dbContext.AddAsync(hwid);

        return hwid;
    }
}