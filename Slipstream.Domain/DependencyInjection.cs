using Microsoft.Extensions.DependencyInjection;

using Slipstream.Domain.Actions;
using Slipstream.Domain.DynamicProperties;
using Slipstream.Domain.Entities;
using Slipstream.Domain.Instances;
using Slipstream.Domain.Rules;
using Slipstream.Domain.Triggers;

namespace Slipstream.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddSlipstreamDomain(this IServiceCollection services)
    {
        services.AddTransient<IPropertyScanner, PropertyScanner>();
        services.AddTransient<IPropertyScanner, PropertyScanner>();

        services.Scan(selector =>
        {
            selector.FromApplicationDependencies()
                .AddClasses(f => f.AssignableTo<IEntityConfiguration>())
                    .AsSelfWithInterfaces()

                .AddClasses(f => f.AssignableTo<IInstanceConfigurationValidator>())
                    .AsSelf()

                .AddClasses(f => f.AssignableTo<ITriggerFactory>())
                    .AsSelfWithInterfaces()

                .AddClasses(f => f.AssignableTo<IInstanceFactory>())
                    .AsSelfWithInterfaces()
                    .WithSingletonLifetime()

                .AddClasses(f => f.AssignableTo<IRuleFactory>())
                    .AsSelfWithInterfaces()

                .AddClasses(f => f.AssignableTo<IActionFactory>())
                    .AsSelfWithInterfaces()
                ;
        });

        return services;
    }
}
