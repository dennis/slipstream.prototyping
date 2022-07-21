using Slipstream.Domain.Actions;

namespace Slipstream.Plugins.Dummy;

internal class PrintActionConfiguration : IActionConfiguration
{
    public string Message { get; set; } = "";
}