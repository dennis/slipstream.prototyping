using Slipstream.Domain.Entities;

namespace Slipstream.Domain.Actions;

public interface IActionFactory : IEntityFactory<IAction, IActionConfiguration>
{
}
