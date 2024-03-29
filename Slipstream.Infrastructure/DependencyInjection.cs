﻿using Microsoft.Extensions.DependencyInjection;

using Slipstream.Domain;
using Slipstream.Domain.Events;

namespace Slipstream.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddSlipstreamInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IRegistry, Registry>();
        services.AddSingleton<IEventPublisher, EventPublisher>();
        services.AddSingleton<IApplicationSettings, ApplicationSettings>();
        services.AddSingleton<IInstanceContainer, InstanceContainer>();
        services.AddSingleton<ITriggerContainer, TriggerContainer>();
        services.AddSingleton<IActionContainer, ActionContainer>();
        services.AddSingleton<IRuleContainer, RuleContainer>();

        return services;
    }
}
