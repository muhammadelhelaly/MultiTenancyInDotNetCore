namespace MultiTenancy.Services;

public interface ITenantService
{
    public string? GetDatabaseProvider();
    public string? GetConnectionString();
    public Tenant? GetCurrentTenant();
}