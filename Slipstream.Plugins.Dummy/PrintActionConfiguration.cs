using Slipstream.Domain.Actions;

namespace Slipstream.Plugins.Dummy;

public class PrintActionConfiguration : IActionConfiguration
{
    public string Message { get; set; } = "";
}