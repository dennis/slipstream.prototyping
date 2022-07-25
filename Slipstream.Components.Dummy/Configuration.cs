using Slipstream.Domain.Configuration;

namespace Slipstream.Components.Dummy;

public class Configuration : IConfiguration
{
    public string Name { get; set; } = "";
    public int Value { get; set; }
}
