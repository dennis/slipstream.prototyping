using MediatR;

namespace Slipstream.Domain.Events;

public interface IEventHandler<T> : INotificationHandler<T> where T : INotification
{
}
