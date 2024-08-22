using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VRCModding.Entities;

public class AppDbContext : DbContext 
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
	    modelBuilder.Entity<UsedDisplayName>()
		    .HasKey(u => new { u.AccountInfoFK, u.DisplayNameFK });
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Hwid> Hwids { get; set; }
    public DbSet<Ip> Ips { get; set; }
    public DbSet<AccountInfo> AccountInfos { get; set; }
    public DbSet<UsedDisplayName> UsedDisplayNames { get; set; }
    public DbSet<DisplayName> DisplayNames { get; set; }
}
