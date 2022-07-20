using MediatR;

using Slipstream.Domain.Events;

namespace Slipstream.Infrastructure;

public class EventPublisher : IEventPublisher
{
    private readonly IMediator _mediator;

    public EventPublisher(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task Publish(IEvent @event)
        => _mediator.Publish(@event);
}
