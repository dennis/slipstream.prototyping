using Slipstream.Domain.Instances;
using Slipstream.Domain.ValueObjects;
using Slipstream.Plugins.Dummy.Events;

namespace Slipstream.Plugins.Dummy;

public class Instance : IInstance
{
    private readonly InstanceConfiguration _configuration;
    private readonly InstanceFactory _factory;

    public EntityName Name { get; private set; }
    public EntityTypeName TypeName => EntityTypeName.From("Dummy");

    public IInstanceConfiguration? Configuration => _configuration;

    public Instance(EntityName name, InstanceConfiguration config, InstanceFactory factory)
        => (Name, _configuration, _factory) = (name, config, factory);

    internal void OnKeyPressEvent(KeyPressEvent @event)
    {
        Console.WriteLine($"  [{Name}] Got KeyPress '{@event.Key}'");
    }

    public Task MainAsync(CancellationToken cancel)
        => _factory.StartTaskAsync(cancel);
}
