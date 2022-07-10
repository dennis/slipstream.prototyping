using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Slipstream.CLI;
using Slipstream.Core;

//var builder = new ConfigurationBuilder().AddEnvironmentVariables();
//var configuration = builder.Build();

var services = new ServiceCollection();

services.AddSingleton<Application>();
services.AddSlipstreamCore();

//PrintServiceCollection(services);

var serviceProvider = services.BuildServiceProvider();
var application = serviceProvider.GetRequiredService<Application>();

application.RunAsync();

static void PrintServiceCollection(IServiceCollection serviceCollection)
{
    Console.WriteLine("Service Registered: ");
    foreach (var service in serviceCollection)
    {
        Console.WriteLine($" - {service.ServiceType.FullName} -> {service.ImplementationType?.FullName} as {service.Lifetime}");
    }
}
