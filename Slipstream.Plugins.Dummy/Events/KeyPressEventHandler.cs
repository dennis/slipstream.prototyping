using Slipstream.Domain;

namespace Slipstream.Plugins.Dummy.Events;

public class KeyPressEventHandler : IEventHandler<KeyPressEvent>
{
    private readonly Plugin _plugin;

    public KeyPressEventHandler(Plugin plugin)
    {
        _plugin = plugin;
    }

    public Task Handle(KeyPressEvent notification, CancellationToken cancellationToken)
    {
        _plugin.ForAllInstances(instance => instance.OnKeyPressEvent(notification));

        return Task.CompletedTask;
    }
}
