using Slipstream.Domain.Instances;
using Slipstream.Domain.Rules;
using Slipstream.Domain.Triggers;

namespace Slipstream.Domain;

public interface IRegistry
{
    public IEnumerable<IInstance> Instances { get; }
    public IEnumerable<ITrigger> Triggers { get; }

    public IDictionary<string, ITriggerFactory> AvailableTriggerTypes { get; }
    public IDictionary<string, IInstanceFactory> AvailableInstanceTypes { get; }
    public List<IRule> Rules { get; }

    public void AddInstance(IInstance instance);
    public void AddTrigger(ITrigger trigger);

    public void Start();
    public void Stop();
    public void AddRule(IRule rule);
}