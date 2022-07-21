using Slipstream.Domain.Entities;

namespace Slipstream.Domain.Actions;

public interface IAction : IEntity
{
    IActionConfiguration? Configuration { get; }
}