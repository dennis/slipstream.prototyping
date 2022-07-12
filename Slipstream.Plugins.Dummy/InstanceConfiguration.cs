using Slipstream.Domain.Attributes;
using Slipstream.Domain.Configuration;

namespace Slipstream.Plugins.Dummy;

public class InstanceConfiguration : IInstanceConfiguration
{
    [FormHelp("Type any string you like.")]
    public string String { get; set; } = "";
    public int Integer { get; set; }
}
