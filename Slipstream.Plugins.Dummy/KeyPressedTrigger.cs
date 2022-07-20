using Slipstream.Domain.Events;
using Slipstream.Domain.Triggers;
using Slipstream.Domain.ValueObjects;
using Slipstream.Plugins.Dummy.Events;

namespace Slipstream.Plugins.Dummy;

public class KeyPressedTrigger : ITrigger
{
    private readonly KeyPressTriggerConfiguration _configuration;

    public EntityName Name { get; init; }
    public EntityTypeName TypeName => EntityTypeName.From("KeyPress");
    public ITriggerConfiguration? Configuration => _configuration;

    public KeyPressedTrigger(EntityName name, KeyPressTriggerConfiguration configuration)
        => (Name, _configuration) = (name, configuration);

    // Accepts() evaluates everything, so we just return true
    public Task<bool> EvaluateAsync(IEvent @event)
        => Task.FromResult(true);

    public bool Accepts(IEvent @event)
        => @event is KeyPressEvent event1 && _configuration.Key.Contains(event1.Key);
}
