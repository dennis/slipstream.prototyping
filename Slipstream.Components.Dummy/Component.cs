using Slipstream.Domain;
using Slipstream.Domain.Attributes;
using Slipstream.Domain.Configuration;
using Slipstream.Domain.Entities;
using Slipstream.Domain.ValueObjects;

using System.Collections.Concurrent;

namespace Slipstream.Components.Dummy;

[SlipstreamComponent(typeof(InstanceFactory), typeof(Configuration))] // TODO: Try to avoid this
public class Component : IComponent
{
    private readonly IEventPublisher _eventPublisher;

    public EntityName Name => EntityName.From("Dummy");
    public IEnumerable<EntityName> InstanceNames => TypedInstances.Keys.ToList();

    public ConcurrentDictionary<EntityName, Instance> TypedInstances { get; set; } = new();

    public Component(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }

    public Task MainAsync(CancellationToken cancel)
    {
        return Task.Run(async () =>
        {
            while (!cancel.IsCancellationRequested)
            {
                var c = Console.ReadKey().KeyChar;

                Console.WriteLine("GOT CHAR: " + c);

                await _eventPublisher.Publish(new KeyPressEvent(c)).ConfigureAwait(false);
            }
        }, cancel);
    }

    public ConfigurationValidationResult ValidateConfiguration(IConfiguration config)
    {
        var result = new ConfigurationValidator().Validate((Configuration)config);

        return ConfigurationValidationResult.FromFluentValidationResult(result);
    }

    public void AddInstance(EntityName instanceName, IInstance instance)
    {
        TypedInstances[instanceName] = (Instance)instance;
    }
}
