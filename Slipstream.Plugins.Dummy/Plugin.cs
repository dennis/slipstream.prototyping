using Slipstream.Domain;
using Slipstream.Domain.Configuration;
using Slipstream.Domain.Entities;
using Slipstream.Domain.ValueObjects;
using Slipstream.Plugins.Dummy.Events;
using Slipstream.Plugins.Dummy.Internal;

using System.Collections.Concurrent;

namespace Slipstream.Plugins.Dummy;

public class Plugin : IPlugin
{
    private readonly IEventPublisher _eventPublisher;

    public EntityName Name => EntityName.From("Dummy");
    public IEnumerable<EntityName> InstanceNames => TypedInstances.Keys.ToList();

    public ConcurrentDictionary<EntityName, Instance> TypedInstances { get; set; } = new();

    public Plugin(IEventPublisher eventPublisher)
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

    public IConfiguration CreateConfiguration()
    {
        return new Configuration();
    }

    public void CreateInstance(EntityName instanceName, IConfiguration config)
    {
        TypedInstances[instanceName] = new Instance(instanceName);
    }
}
