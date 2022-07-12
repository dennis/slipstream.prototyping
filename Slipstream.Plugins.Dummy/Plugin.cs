using Slipstream.Domain;
using Slipstream.Domain.Configuration;
using Slipstream.Domain.Entities;
using Slipstream.Domain.ValueObjects;
using Slipstream.Plugins.Dummy.Events;
using Slipstream.Plugins.Dummy.Internal;

using System.Collections.Concurrent;
using System.Text.Json;

namespace Slipstream.Plugins.Dummy;

public class Plugin : IPlugin
{
    private readonly IEventPublisher _eventPublisher;

    public EntityName Name => EntityName.From("Dummy");
    public IEnumerable<EntityName> InstanceNames => _typedInstances.Keys.ToList();

    private readonly ConcurrentDictionary<EntityName, Instance> _typedInstances = new();

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

    internal void ForAllInstances(Action<Instance> func)
    {
        foreach (var instance in _typedInstances.Values)
        {
            lock (instance)
            {
                func(instance);
            }
        }
    }

    public InstanceConfigurationValidationResult ValidateInstanceConfiguration(IInstanceConfiguration config)
    {
        var result = new ConfigurationValidator().Validate((InstanceConfiguration)config);

        return InstanceConfigurationValidationResult.FromFluentValidationResult(result);
    }

    public IInstanceConfiguration CreateInstanceConfiguration()
    {
        return new InstanceConfiguration();
    }

    public void CreateInstance(EntityName instanceName, IInstanceConfiguration config)
    {
        _typedInstances[instanceName] = new Instance(instanceName, (InstanceConfiguration)config);
    }

    public void Save(IApplicationSettings applicationSettings)
    {
        ForAllInstances(instance =>
        {
            instance.Save(Name, applicationSettings);
        });
    }

    public void LoadInstance(EntityName instanceName, IApplicationSettings applicationSettings)
    {
        var json = applicationSettings.LoadInstance(Name, instanceName);
        var config = JsonSerializer.Deserialize<InstanceConfiguration>(json);
        if (config is not null)
        {
            CreateInstance(instanceName, config);
        }
    }
}
