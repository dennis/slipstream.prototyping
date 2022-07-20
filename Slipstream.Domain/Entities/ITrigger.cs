using Slipstream.Domain.Events;

namespace Slipstream.Domain.Entities;

public interface ITrigger : IEntity
{
    ITriggerConfiguration? Configuration { get; }

    public Task<bool> EvaluateAsync(IEvent @event);
}
