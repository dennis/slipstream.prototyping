using Slipstream.Domain;
using Slipstream.Domain.Actions;
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
    public IActionContainer ActionContainer { get; }
    public IRuleContainer RuleContainer { get; }

    public Registry(IInstanceContainer availableInstanceTypes, ITriggerContainer triggerContainer, IActionContainer actionContainer, IRuleContainer ruleContainer)
    {
        InstanceContainer = availableInstanceTypes;
        TriggerContainer = triggerContainer;
        ActionContainer = actionContainer;
        _cancelTokenSource = new CancellationTokenSource();
        RuleContainer = ruleContainer;
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

        RuleContainer.Rules.Add(rule);
    }

    public void AddAction(IAction action)
    {
        EnsureValidEntityName(action.Name);

        ActionContainer.Actions.Add(action);
    }
}
