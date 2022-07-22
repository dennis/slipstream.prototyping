using Slipstream.Domain.Actions;
using Slipstream.Domain.Entities;
using Slipstream.Domain.Triggers;

namespace Slipstream.Domain.Rules;

public interface IRule : IEntity
{
    public ITrigger Trigger { get; }
    public IAction Action { get; }
    public IRuleConfiguration Configuration { get; }
}
