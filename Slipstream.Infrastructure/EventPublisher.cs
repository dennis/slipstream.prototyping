using Slipstream.Domain;
using Slipstream.Domain.Events;
using Slipstream.Domain.Triggers;

namespace Slipstream.Infrastructure;

public class EventPublisher : IEventPublisher
{
    private class TriggerWrap
    {
        private readonly ITrigger _trigger;

        public bool Triggered { get; private set; }
        public string Name => _trigger.Name;

        public TriggerWrap(ITrigger trigger)
        {
            _trigger = trigger;
        }

        public async Task<TriggerWrap> EvaluateAsync(IEvent @event)
        {
            Triggered = await _trigger.EvaluateAsync(@event);

            return this;
        }
    }

    private readonly ITriggerContainer _triggerContainer;
    private readonly IRuleContainer _ruleContainer;

    public EventPublisher(ITriggerContainer triggerContainer, IRuleContainer ruleContainer)
    {
        _triggerContainer = triggerContainer;
        _ruleContainer = ruleContainer;
    }

    public async Task PublishAsync(IEvent @event)
    {
        Console.WriteLine($"Publishing event {@event}");

        var result = await Task.WhenAll(
            _triggerContainer
            .Triggers
            .Where(t => t.Accepts(@event))
            .Select(t => new TriggerWrap(t))
            .Select(t => t.EvaluateAsync(@event))
        );

        var triggerFired = result
            .Where(t => t.Triggered)
            .Select(t => t.Name);
            
        foreach (var item in triggerFired)
        {
            Console.WriteLine($" - triggered: {item}");

            foreach (var triggeredRule in _ruleContainer.Rules.Where(a => a.Trigger.Name == item))
            {
                Console.WriteLine($"   - used by rule: {triggeredRule.Name}. Invoking action {triggeredRule.Action.Name}");
                triggeredRule.Action.Invoke(@event);
            }
        }
    }
}
