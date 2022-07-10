using Microsoft.Extensions.DependencyInjection;

using Slipstream.Core.Configuration;
using Slipstream.Core.Entities;
using Slipstream.Core.Forms;

namespace Slipstream.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddSlipstreamCore(this IServiceCollection services)
    {
        services.AddSingleton<IRegistry, Registry>();
        services.AddTransient<IFormGenerator, FormGenerator>();
        services.Scan(selector =>
        {
            selector.FromApplicationDependencies()
                .AddClasses(f => f.AssignableTo<IComponent>())
                    .AsImplementedInterfaces()
                    .WithSingletonLifetime()

                .AddClasses(f => f.AssignableTo<IInstance>())
                    .AsSelf()

                .AddClasses(f => f.AssignableTo<IConfiguration>())
                    .AsSelf()

                .AddClasses(f => f.AssignableTo<IInstanceFactory>())
                    .AsSelf()

                .AddClasses(f => f.AssignableTo<IConfigurationValidator>())
                    .AsSelf()
                ;
        });

        return services;
    }
}
