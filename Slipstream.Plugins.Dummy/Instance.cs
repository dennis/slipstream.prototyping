using Slipstream.Domain;
using Slipstream.Domain.Entities;
using Slipstream.Domain.ValueObjects;
using Slipstream.Plugins.Dummy.Events;

using System.Text.Json;

namespace Slipstream.Plugins.Dummy;

public class Instance : IInstance
{
    public EntityName Name { get; private set; }
    public InstanceConfiguration Configuration { get; private set; }

    public Instance(EntityName name, InstanceConfiguration config)
        => (Name, Configuration) = (name, config);

    public Task MainAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;

    internal void OnKeyPressEvent(KeyPressEvent @event)
    {
        Console.WriteLine($"  [{Name}] Got KeyPress '{@event.Key}'");
    }

    internal void Save(EntityName pluginName, IApplicationSettings applicationSettings)
    {
        var content = JsonSerializer.Serialize(Configuration);

        applicationSettings.SaveInstance(pluginName, Name, content);
    }
}
