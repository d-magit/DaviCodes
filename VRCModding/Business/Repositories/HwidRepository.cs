using VRCModding.Entities;
using Microsoft.EntityFrameworkCore;

namespace VRCModding.Business.Repositories;

public class HwidRepository {
    private readonly AppDbContext dbContext;

    public HwidRepository(AppDbContext dbContext) {
        this.dbContext = dbContext;
    }
    
    public async Task<Hwid?> TryGetAsync(string hwid) =>
        await dbContext.Hwids
	        //.Include(h => h.User)
	        .FirstOrDefaultAsync(h => h.Id == hwid);
    
    public async Task CreateAsync(string hwid, Guid userId) {
        var hwidEntity = new Hwid {
            Id = hwid,
            UserFK = userId
        };

        await dbContext.AddAsync(hwidEntity);
    }
}
