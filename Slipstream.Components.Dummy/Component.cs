using Slipstream.Core;
using Slipstream.Core.Attributes;
using Slipstream.Core.Configuration;
using Slipstream.Core.Entities;
using Slipstream.Core.ValueObjects;

using System.Reactive.Linq;

namespace Slipstream.Components.Dummy;

[SlipstreamComponent(typeof(InstanceFactory), typeof(Configuration))] // TODO: Try to avoid this
public class Component : IComponent
{
    public EntityName Name => EntityName.From("Dummy");

    public void Input(IObservable<IEvent> stream, CancellationToken cancel)
    {
        stream.Subscribe(a => Console.WriteLine($"[{Name}] Got event {a.ToString()}"));
    }

    IObservable<IEvent>? IEntity.Output(CancellationToken cancel)
    {
        return Observable.Create<IEvent>(
            obs =>
            {
                return Task.Run(() =>
                {
                    while (!cancel.IsCancellationRequested)
                    {
                        var c = Console.ReadKey().KeyChar;
                        obs.OnNext(new KeyPressEvent(c));
                    }

                    obs.OnCompleted();
                });
            });
    }

    public ConfigurationValidationResult ValidateConfiguration(IConfiguration config)
    {
        var result = new ConfigurationValidator().Validate((Configuration)config);
        return ConfigurationValidationResult.FromFluentValidationResult(result);
    }
}
