using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        services.AddSingleton<Park>();
        services.AddSingleton<Inventory>();
        services.AddSingleton<BeginnerList>();

        services.AddSingleton<BeginnerService>();
        services.AddSingleton<EnergyConsumptionService>();

        services.AddSingleton<
            IInspectionRule,
            QualityInspectionRule>();

        services.AddSingleton<
            IServiceCenter,
            ServiceCenter>();

        services.AddSingleton<
            IEntityProcessor,
            TransportProcessor>();

        services.AddSingleton<
            IEntityProcessor,
            InventoryProcessor>();

        services.AddSingleton<
            IEntityProcessor,
            BeginnerProcessor>();

        services.AddSingleton<EntityFactory>();

        services.AddSingleton<ConsoleInput>();
        services.AddSingleton<ConsoleApp>();

        return services;
    }
}