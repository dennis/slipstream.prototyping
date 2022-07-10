using Slipstream.Core;
using Slipstream.Core.Entities;
using Slipstream.Core.ValueObjects;

using System.Reactive.Linq;

namespace Slipstream.Components.Dummy;

public class Instance : IInstance
{
    public EntityName Name { get; private set; }

    public Instance(EntityName name)
        => Name = name; 

    public void Input(IObservable<IEvent> stream, CancellationToken cancel)
    {
        stream.Subscribe(a => Console.WriteLine($"[{Name}] Got event {a}"));
    }

    public IObservable<IEvent>? Output(CancellationToken cancel)
        => Observable
            .Timer(DateTimeOffset.Now.AddSeconds(1.5), TimeSpan.FromSeconds(5))
            .Select(_ => new TimedEvent());
}
