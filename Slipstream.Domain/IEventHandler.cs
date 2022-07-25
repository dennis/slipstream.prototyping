using MediatR;

namespace Slipstream.Domain;

public interface IEventHandler<T> : INotificationHandler<T> where T : INotification
{
}
