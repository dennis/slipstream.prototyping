using Microsoft.Extensions.DependencyInjection;

using Slipstream.CLI;
using Slipstream.Infrastructure;
using Slipstream.Domain;

using System.Reflection;

var services = new ServiceCollection();

services.AddSingleton<Application>();
services.AddSingleton<TUIHelper>();
services.AddSlipstreamInfrastructure();
services.AddSlipstreamDomain(Assembly.GetExecutingAssembly());
services.AddTransient<MainMenuHandler>();
services.AddTransient<TriggerMenuHandler>();
services.AddTransient<EntityHelper>();

var serviceProvider = services.BuildServiceProvider();
var application = serviceProvider.GetRequiredService<Application>();

application.RunAsync().GetAwaiter().GetResult();
