using Chat.BL.Abstractions.Data;
using Chat.DAL;
using Microsoft.EntityFrameworkCore;

namespace Chat.App.Startup;

public static class EfCoreSetup
{
    public static IServiceCollection AddEfCore(this IServiceCollection services, IConfiguration configuration)
    {
        var defaultConnection = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(defaultConnection));

        services.AddScoped<IDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IUnityOfWork>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());

        return services;
    }
}