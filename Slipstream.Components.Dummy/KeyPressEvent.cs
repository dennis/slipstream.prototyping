using Slipstream.Core;

namespace Slipstream.Components.Dummy;

public class KeyPressEvent : IEvent
{
    public char Key { get; }

    public KeyPressEvent(char key)
        => Key = key;
}
