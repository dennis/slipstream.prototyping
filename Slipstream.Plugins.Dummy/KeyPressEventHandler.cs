using Slipstream.Domain;

namespace Slipstream.Plugins.Dummy;

public class KeyPressEventHandler : IEventHandler<KeyPressEvent>
{
    private readonly Plugin _plugin;

    public KeyPressEventHandler(Plugin plugin)
    {
        _plugin = plugin;
    }

    public Task Handle(KeyPressEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[{_plugin.Name}]: got KeyPressEvent {notification.Key}");

        foreach (var instance in _plugin.TypedInstances.Values)
        {
            instance.OnKeyPressEvent(notification);
        }
        return Task.CompletedTask;
    }
}
