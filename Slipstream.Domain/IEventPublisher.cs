namespace Slipstream.Domain;

public interface IEventPublisher
{
    Task Publish(IEvent @event);
}
