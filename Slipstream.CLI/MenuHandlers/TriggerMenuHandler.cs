﻿using Slipstream.Domain;
using Slipstream.Domain.Triggers;

namespace Slipstream.CLI.MenuHandlers;

internal class TriggerMenuHandler
{
    private readonly IRegistry _registry;
    private readonly EntityHelper _entityHelper;

    public TriggerMenuHandler(
        EntityHelper entityHelper,
        IRegistry registry
    )
    {
        _entityHelper = entityHelper;
        _registry = registry;
    }

    internal void Show(TUIHelper tui)
    {
        do
        {
            tui.PrintHeading("Triggers")
                .Print(" l - list triggers")
                .Print(" n - new trigger")
                .Print(" b - back")
                .Spacer();

            var input = tui.ReadKey();

            switch (input)
            {
                case 'l':
                case 'L':
                    ListTriggers(tui);
                    break;

                case 'n':
                case 'N':
                    CreateTrigger(tui.NewScope("Create"));
                    break;

                case 'b':
                case 'B':
                    return;

                default:
                    tui.Error("Unknown option");
                    break;
            }

        } while (true);

    }

    private void ListTriggers(TUIHelper tui)
    {
        tui.PrintStrong("list triggers");

        if (_registry.TriggerContainer.Triggers.Any())
        {
            foreach (var trigger in _registry.TriggerContainer.Triggers)
            {
                tui.Print($" - {trigger.Name}  ({trigger.GetType()})");
            }
        }
        else
        {
            tui.Print("No triggers");
        }

        tui.Spacer();
    }

    private void CreateTrigger(TUIHelper tui)
    {
        tui.PrintStrong("Create trigger");

        if (!_registry.TriggerContainer.Types.Keys.Any())
        {
            tui.Error("No trigger types available").Spacer();
            return;
        }

        foreach (var t in _registry.TriggerContainer.Types.Keys)
        {
            tui.Print($" - {t}");
        }

        _entityHelper.Creator<ITrigger, ITriggerFactory, ITriggerConfiguration>(
            tui.NewScope("new trigger"),
            (entityTypeName) => _registry.TriggerContainer.Types[entityTypeName].CreateConfiguration(),
            (entityTypeName, configuration) => _registry.TriggerContainer.Types[entityTypeName].Validate(configuration),
            (entityTypeName, entityName, configuration) =>
            {
                var trigger = _registry.TriggerContainer.Types[entityTypeName].Create(entityName, configuration);
                _registry.AddTrigger(trigger);
            },
            _registry.TriggerContainer.Types.Keys.ToList()
        );
    }
}