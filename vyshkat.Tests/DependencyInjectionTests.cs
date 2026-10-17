using Microsoft.Extensions.DependencyInjection;

public class DependencyInjectionTests
{
    [Fact]
    public void Container_CanBuildMainApplicationServices()
    {
        var services = new ServiceCollection();
        services.AddApplicationServices();

        using var provider = services.BuildServiceProvider();

        Assert.NotNull(provider.GetRequiredService<EntityFactory>());
        Assert.NotNull(provider.GetRequiredService<IServiceCenter>());
        Assert.NotNull(provider.GetRequiredService<ConsoleApp>());

        Assert.Equal(
            3,
            provider.GetServices<IEntityProcessor>().Count());
    }
}
