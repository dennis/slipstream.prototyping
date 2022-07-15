using Slipstream.Domain;
using Slipstream.Domain.Instances;
using Slipstream.Domain.Triggers;
using Slipstream.Domain.ValueObjects;

namespace Slipstream.Infrastructure;

public class Registry : IRegistry
{
    private readonly Dictionary<EntityName, Task> _tasks = new();  // TODO - we need to handle it properly
    private readonly List<ITrigger> _triggers = new();
    private readonly List<IInstance> _instances= new();

    private CancellationTokenSource _cancelTokenSource;

    private bool _started;

    public IEnumerable<ITrigger> Triggers => _triggers;
    public IEnumerable<IInstance> Instances => _instances;
    public IDictionary<string, ITriggerFactory> AvailableTriggerTypes { get; } = new Dictionary<string, ITriggerFactory>();
    public IDictionary<string, IInstanceFactory> AvailableInstanceTypes { get; } = new Dictionary<string, IInstanceFactory>();

    public Registry(IEnumerable<ITriggerFactory> triggerFactories, IEnumerable<IInstanceFactory> instanceFactories)
    {
        _cancelTokenSource = new CancellationTokenSource();

        foreach (var triggerFactory in triggerFactories)
        {
            AddTriggerType(triggerFactory);
        }

        foreach (var factory in instanceFactories)
        {
            AddInstanceType(factory);
        }
    }

    private void AddInstanceType(IInstanceFactory factory)
    {
        AvailableInstanceTypes.Add(factory.TypeName, factory);
    }

    private void AddTriggerType(ITriggerFactory factory)
    {
        AvailableTriggerTypes.Add(factory.TypeName, factory);
    }

    public void Start()
    {
        if (_started)
            throw new InvalidOperationException("Already started");

        _cancelTokenSource = new CancellationTokenSource();

        foreach (var instance in _instances)
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
        if (_triggers.Select(a => a.Name).Contains(name))
        {
            throw new ArgumentException($"{name} already exists (used by a trigger)");
        }

        if (_instances.Select(a => a.Name).Contains(name))
        {
            throw new ArgumentException($"{name} already exists (used by an instance)");
        }
    }

    public void AddInstance(IInstance instance)
    {
        EnsureValidEntityName(instance.Name);

        _instances.Add(instance);
    }

    public void AddTrigger(ITrigger trigger)
    {
        EnsureValidEntityName(trigger.Name);

        _triggers.Add(trigger);
    }
}
