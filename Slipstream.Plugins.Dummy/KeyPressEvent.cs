using Slipstream.Domain;

namespace Slipstream.Plugins.Dummy;

public class KeyPressEvent : IEvent
{
    public char Key { get; }

    public KeyPressEvent(char key)
        => Key = key;
}
