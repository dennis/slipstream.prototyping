using Slipstream.Domain.Triggers;

namespace Slipstream.Plugins.Dummy;

public class KeyPressTriggerConfiguration : ITriggerConfiguration
{
    public string Key { get; set; } = "";
}
