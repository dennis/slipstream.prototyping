namespace Slipstream.Domain.Events;

public interface IEventPublisher
{
    Task PublishAsync(IEvent @event);
}
