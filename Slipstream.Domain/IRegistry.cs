using Slipstream.Domain.Actions;
using Slipstream.Domain.Instances;
using Slipstream.Domain.Rules;
using Slipstream.Domain.Triggers;

namespace Slipstream.Domain;

public interface IRegistry
{
    public ITriggerContainer TriggerContainer { get; }
    public IInstanceContainer InstanceContainer { get; }
    public IActionContainer ActionContainer { get; }
    public IRuleContainer RuleContainer { get; }
    
    public void AddInstance(IInstance instance);
    public void AddTrigger(ITrigger trigger);
    public void AddAction(IAction action);
    public void AddRule(IRule rule);

    public void Start();
    public void Stop();
}