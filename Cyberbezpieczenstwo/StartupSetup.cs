using System;
using Cyberbezpieczenstwo.Data;
using Microsoft.EntityFrameworkCore;

namespace Cyberbezpieczenstwo;

public static class StartupSetup
{
    public static void AddDbContext(this IServiceCollection services) =>
        services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql("Server=localhost;Port=5432;Database=cybersecurity;User Id=postgres;Password=postgrespw;Pooling=False",
                    b => b.MigrationsAssembly("Cyberbezpieczenstwo")));
}

