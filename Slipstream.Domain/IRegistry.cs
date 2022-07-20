using Slipstream.Domain.Instances;
using Slipstream.Domain.Rules;
using Slipstream.Domain.Triggers;

namespace Slipstream.Domain;

public interface IRegistry
{

    
    public ITriggerContainer TriggerContainer { get; }
    public IInstanceContainer InstanceContainer { get; }
    
    public List<IRule> Rules { get; }

    public void AddInstance(IInstance instance);
    public void AddTrigger(ITrigger trigger);

    public void Start();
    public void Stop();
    public void AddRule(IRule rule);
}