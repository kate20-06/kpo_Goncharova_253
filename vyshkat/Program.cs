using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddApplicationServices();

using var provider = services.BuildServiceProvider();

var app = provider.GetRequiredService<ConsoleApp>();
app.Run();