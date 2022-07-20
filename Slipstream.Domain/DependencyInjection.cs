﻿using MediatR;

using Microsoft.Extensions.DependencyInjection;

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
                .AddClasses(f => f.AssignableTo<IInstance>())
                    .AsSelf()

                .AddClasses(f => f.AssignableTo<IInstanceConfiguration>())
                    .AsSelf()

                .AddClasses(f => f.AssignableTo<IInstanceConfigurationValidator>())
                    .AsSelf()

                .AddClasses(f => f.AssignableTo<ITriggerFactory>())
                    .AsSelfWithInterfaces()

                .AddClasses(f => f.AssignableTo<IInstanceFactory>())
                    .AsSelfWithInterfaces()
                    .WithSingletonLifetime()

                // Add the mediatr INotificationHandlers from the other assemblies
                .AddClasses(f => f.AssignableTo(typeof(INotificationHandler<>)))
                    .AsImplementedInterfaces()
                ;
        });

        return services;
    }
}
