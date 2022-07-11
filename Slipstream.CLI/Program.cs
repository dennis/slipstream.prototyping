using Microsoft.Extensions.DependencyInjection;

using Slipstream.CLI;
using Slipstream.Infrastructure;
using Slipstream.Domain;

using System.Reflection;

//var builder = new ConfigurationBuilder().AddEnvironmentVariables();
//var configuration = builder.Build();

var services = new ServiceCollection();

services.AddSingleton<Application>();
services.AddSlipstreamInfrastructure();
services.AddSlipstreamDomain(Assembly.GetExecutingAssembly());
services.AddTransient<TUIHelper>();
services.AddTransient<ConsoleFormVisitor>();

var serviceProvider = services.BuildServiceProvider();
var application = serviceProvider.GetRequiredService<Application>();

application.RunAsync().GetAwaiter().GetResult();
