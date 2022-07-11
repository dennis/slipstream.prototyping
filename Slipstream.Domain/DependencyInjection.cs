using MediatR;

using Microsoft.Extensions.DependencyInjection;

using Slipstream.Domain.Configuration;
using Slipstream.Domain.Entities;
using Slipstream.Domain.Forms;

using System.Reflection;

namespace Slipstream.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddSlipstreamDomain(this IServiceCollection services, Assembly assembly)
    {
        services.AddTransient<IFormGenerator, FormGenerator>();
        services.AddMediatR(assembly);

        services.Scan(selector =>
        {
            selector.FromApplicationDependencies()
                .AddClasses(f => f.AssignableTo<IPlugin>())
                    .AsSelfWithInterfaces()
                    .WithSingletonLifetime()

                .AddClasses(f => f.AssignableTo<IInstance>())
                    .AsSelf()

                .AddClasses(f => f.AssignableTo<IConfiguration>())
                    .AsSelf()

                .AddClasses(f => f.AssignableTo<IConfigurationValidator>())
                    .AsSelf()

                // Add the mediatr INotificationHandlers from the other assemblies
                .AddClasses(f => f.AssignableTo(typeof(INotificationHandler<>)))
                    .AsImplementedInterfaces()
                ;
        });

        return services;
    }
}
