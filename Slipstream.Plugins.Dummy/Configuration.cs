using Slipstream.Domain.Configuration;

namespace Slipstream.Plugins.Dummy;

public class Configuration : IConfiguration
{
    public string Name { get; set; } = "";
    public int Value { get; set; }
}
