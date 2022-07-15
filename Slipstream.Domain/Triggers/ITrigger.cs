using Slipstream.Domain.Entities;
using Slipstream.Domain.Events;

namespace Slipstream.Domain.Triggers;

public interface ITrigger : IEntity
{
    ITriggerConfiguration? Configuration { get; }

    public Task<bool> EvaluateAsync(IEvent @event);
}
