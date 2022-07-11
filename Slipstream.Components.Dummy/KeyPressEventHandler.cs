using Slipstream.Domain;

namespace Slipstream.Components.Dummy;

public class KeyPressEventHandler : IEventHandler<KeyPressEvent>
{
    private readonly Component _component;

    public KeyPressEventHandler(Component component)
    {
        _component = component;
    }

    public Task Handle(KeyPressEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[{_component.Name}]: got keypressevent {notification.Key}");

        foreach (var instance in _component.TypedInstances.Values)
        {
            instance.OnKeyPressEvent(notification);
        }
        return Task.CompletedTask;
    }
}
