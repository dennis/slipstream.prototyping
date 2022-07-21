using Slipstream.Domain;
using Slipstream.Domain.Actions;
using Slipstream.Domain.Triggers;

namespace Slipstream.CLI.MenuHandlers;

internal class ActionMenuHandler
{
    private readonly IActionContainer _actions;
    private readonly EntityHelper _entityHelper;
    private readonly IRegistry _registry;

    public ActionMenuHandler(IActionContainer actions, EntityHelper entityHelper, IRegistry registry)
    {
        _actions = actions;
        _entityHelper = entityHelper;
        _registry = registry;
    }

    internal void Show(TUIHelper tui)
    {
        do
        {
            tui.PrintHeading("Actions")
                .Print(" l - list actions")
                .Print(" n - new actions")
                .Print(" b - back")
                .Spacer();

            var input = tui.ReadKey();

            switch (input)
            {
                case 'l':
                case 'L':
                    List(tui);
                    break;

                case 'n':
                case 'N':
                    Create(tui);
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

    private void List(TUIHelper tui)
    {
        tui.PrintStrong("List actions");

        if (_actions.Actions.Any())
        {
            foreach (var rule in _actions.Actions)
            {
                tui.Print($" - {rule.Name}  ({rule.GetType()})");
            }
        }
        else
        {
            tui.Print("No actions");
        }

        tui.Spacer();
    }

    private void Create(TUIHelper tui)
    {
        tui.PrintStrong("Create actions");


        _entityHelper.Creator<IAction, IActionFactory, IActionConfiguration>(
            tui.NewScope("new actions"),
            (entityTypeName) => _registry.ActionContainer.Types[entityTypeName].CreateConfiguration(),
            (entityTypeName, configuration) => _registry.ActionContainer.Types[entityTypeName].Validate(configuration),
            (entityTypeName, entityName, configuration) =>
            {
                var action = _registry.ActionContainer.Types[entityTypeName].Create(entityName, configuration);
                _registry.AddAction(action);
            },
            _registry.ActionContainer.Types.Keys.ToList()
        );
    }
}