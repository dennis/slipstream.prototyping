namespace Slipstream.Domain.Events;

public interface IEventPublisher
{
    Task Publish(IEvent @event);
}
