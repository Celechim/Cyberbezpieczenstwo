using Cyberbezpieczenstwo.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cyberbezpieczenstwo.Data;


public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public ApplicationDbContext()
    { 
    }

    public DbSet<User> CustomUsers => Set<User>();
    public DbSet<PasswordLimitation> PasswordLimitations => Set<PasswordLimitation>();
    public DbSet<LogHistory> LogHistory => Set<LogHistory>();
    public DbSet<SecuritySettings> SecuritySettings => Set<SecuritySettings>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();

        if (optionsBuilder.IsConfigured) return;

        optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=cybersecurity;User Id=postgres;Password=postgrespw;Pooling=False",
                    b => b.MigrationsAssembly("Cyberbezpieczenstwo"));

        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<PasswordLimitation>().HasData(
            new { Id = 1, LimitationName = "Co najmniej 8 znaków", IsActive = false },
            new { Id = 2, LimitationName = "Co najmniej 1 wielka litera", IsActive = false },
            new { Id = 3, LimitationName = "Co najmniej 1 znak specjalny", IsActive = false }
            );
        modelBuilder.Entity<User>().HasData(
            new User() { Id = 1, Login = "ADMIN", Password = "AKCR1THZKROiOzsL/LhA4tIrT/+uRESeOtapnWAtMedABbbYt/WGZDbFtUXX7IHfxg==", Role = 0, ExpirationTime = null, IsBlocked = false });
    }
}