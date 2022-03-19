using Microsoft.EntityFrameworkCore;

namespace VRCModding.Entities;

public class AppDbContext : DbContext 
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
	    modelBuilder.Entity<UsedDisplayName>()
		    .HasKey(u => new { u.AccountFK, u.DisplayNameFK });
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Hwid> Hwids { get; set; }
    public DbSet<Ip> Ips { get; set; }
    public DbSet<UsedDisplayName> UsedDisplayNames { get; set; }
    public DbSet<DisplayName> DisplayNames { get; set; }
}
