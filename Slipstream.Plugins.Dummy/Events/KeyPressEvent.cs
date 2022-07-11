using Slipstream.Domain;

namespace Slipstream.Plugins.Dummy.Events;

public class KeyPressEvent : IEvent
{
    public char Key { get; }

    public KeyPressEvent(char key)
        => Key = key;
}
