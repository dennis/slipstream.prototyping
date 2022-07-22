using Microsoft.Extensions.DependencyInjection;

using Slipstream.CLI;
using Slipstream.Infrastructure;
using Slipstream.Domain;

using Slipstream.CLI.MenuHandlers;

var services = new ServiceCollection();

services.AddSingleton<Application>();
services.AddSingleton<TUIHelper>();
services.AddSlipstreamInfrastructure();
services.AddSlipstreamDomain();
services.AddTransient<MainMenuHandler>();
services.AddTransient<TriggerMenuHandler>();
services.AddTransient<InstanceMenuHandler>();
services.AddTransient<RuleMenuHandler>();
services.AddTransient<ActionMenuHandler>();
services.AddTransient<EntityHelper>();

var serviceProvider = services.BuildServiceProvider();
var application = serviceProvider.GetRequiredService<Application>();

application.RunAsync().GetAwaiter().GetResult();
