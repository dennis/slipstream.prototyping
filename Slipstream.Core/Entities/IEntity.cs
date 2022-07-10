using Slipstream.Core.ValueObjects;

namespace Slipstream.Core.Entities;

public interface IEntity
{
    EntityName Name { get; }

    void Input(IObservable<IEvent> stream, CancellationToken cancel);
    IObservable<IEvent>? Output(CancellationToken cancel);
}
