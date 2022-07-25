
using Slipstream.Domain.Attributes;
using Slipstream.Domain.Instances;

namespace Slipstream.Plugins.Dummy;

public class InstanceConfiguration : IInstanceConfiguration
{
    [PropertyHelp("Type any string you like.")]
    public string String { get; set; } = "";
    public int Integer { get; set; }
}
