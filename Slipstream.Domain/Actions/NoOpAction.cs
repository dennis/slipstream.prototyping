using Slipstream.Domain.ValueObjects;

namespace Slipstream.Domain.Actions;

public class NoOpAction : IAction
{
    public EntityName Name => "noopaction";

    public EntityTypeName TypeName => "action";

    public IActionConfiguration? Configuration => null;
}
