using Slipstream.Domain;
using Slipstream.Domain.Instances;
using Slipstream.Domain.Rules;
using Slipstream.Domain.Triggers;
using Slipstream.Domain.ValueObjects;

namespace Slipstream.Infrastructure;

public class Registry : IRegistry
{
    private readonly Dictionary<EntityName, Task> _tasks = new();  // TODO - we need to handle it properly

    private CancellationTokenSource _cancelTokenSource;

    private bool _started;

    public IInstanceContainer InstanceContainer { get; }
    public ITriggerContainer TriggerContainer { get; }
    public List<IRule> Rules { get; } = new();

    public Registry(IInstanceContainer availableInstanceTypes, ITriggerContainer triggerContainer)
    {
        InstanceContainer = availableInstanceTypes;
        TriggerContainer = triggerContainer;
        _cancelTokenSource = new CancellationTokenSource();
    }

    public void Start()
    {
        if (_started)
            throw new InvalidOperationException("Already started");

        _cancelTokenSource = new CancellationTokenSource();

        foreach (var instance in InstanceContainer.Instances)
        {
            _tasks.Add(instance.Name, instance.MainAsync(_cancelTokenSource.Token));
        }

        _started = true;
    }

    public void Stop()
    {
        _cancelTokenSource.Cancel();
        _started = false;
    }

    private void EnsureValidEntityName(EntityName name)
    {
        if (TriggerContainer.Triggers.Select(a => a.Name).Contains(name))
        {
            throw new ArgumentException($"{name} already exists (used by a trigger)");
        }

        if (InstanceContainer.Instances.Select(a => a.Name).Contains(name))
        {
            throw new ArgumentException($"{name} already exists (used by an instance)");
        }
    }

    public void AddInstance(IInstance instance)
    {
        EnsureValidEntityName(instance.Name);

        InstanceContainer.Instances.Add(instance);
    }

    public void AddTrigger(ITrigger trigger)
    {
        EnsureValidEntityName(trigger.Name);

        TriggerContainer.Triggers.Add(trigger);
    }

    public void AddRule(IRule rule)
    {
        EnsureValidEntityName(rule.Name);

        Rules.Add(rule);
    }
}
