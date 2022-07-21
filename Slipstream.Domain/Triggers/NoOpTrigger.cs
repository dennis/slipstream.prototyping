using Slipstream.Domain.Events;
using Slipstream.Domain.ValueObjects;

namespace Slipstream.Domain.Triggers;

public class NoOpTrigger : ITrigger
{
    public ITriggerConfiguration? Configuration => null;

    public EntityName Name => "nooptrigger";

    public EntityTypeName TypeName => "trigger";

    public bool Accepts(IEvent @event)
        => false;

    public Task<bool> EvaluateAsync(IEvent @event)
        => Task.FromResult(false);
}
