using Slipstream.Domain.Attributes;
using Slipstream.Domain.Configuration;

namespace Slipstream.Plugins.Dummy;

public class Configuration : IConfiguration
{
    [FormHelp("Type any string you like.")]
    public string String { get; set; } = "";
    public int Integer { get; set; }
}
