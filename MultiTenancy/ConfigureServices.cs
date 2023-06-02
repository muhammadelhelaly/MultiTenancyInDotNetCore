using Microsoft.EntityFrameworkCore;

namespace MultiTenancy;

public static class ConfigureServices
{
    public static IServiceCollection AddTenancy(this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddScoped<ITenantService, TenantService>();

        services.Configure<TenantSettings>(configuration.GetSection(nameof(TenantSettings)));

        TenantSettings options = new();
        configuration.GetSection(nameof(TenantSettings)).Bind(options);

        var defaultDbProvider = options.Defaults.DBProvider;

        if (defaultDbProvider.ToLower() == "mssql")
        {
            services.AddDbContext<ApplicationDbContext>(m => m.UseSqlServer());
        }

        foreach (var tenant in options.Tenants)
        {
            var connectionString = tenant.ConnectionString ?? options.Defaults.ConnectionString;

            using var scope = services.BuildServiceProvider().CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            dbContext.Database.SetConnectionString(connectionString);

            if (dbContext.Database.GetPendingMigrations().Any())
            {
                dbContext.Database.Migrate();
            }
        }

        return services;
    }
}