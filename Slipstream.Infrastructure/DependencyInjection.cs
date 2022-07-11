using Microsoft.Extensions.DependencyInjection;

using Slipstream.Domain;

namespace Slipstream.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddSlipstreamInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IRegistry, Registry>();
        services.AddSingleton<IEventPublisher, EventPublisher>();

        return services;
    }
}
