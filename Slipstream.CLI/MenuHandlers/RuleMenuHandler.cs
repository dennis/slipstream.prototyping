using Slipstream.Domain;
using Slipstream.Domain.Rules;

namespace Slipstream.CLI.MenuHandlers;

internal class RuleMenuHandler
{
    private readonly IRegistry _registry;
    private readonly IRuleFactory _ruleFactory;
    private readonly EntityHelper _entityHelper;

    public RuleMenuHandler(
        EntityHelper entityHelper,
        IRegistry registry,
        IRuleFactory ruleFactory
    )
    {
        _entityHelper = entityHelper;
        _registry = registry;
        _ruleFactory = ruleFactory;
    }

    internal void Show(TUIHelper tui)
    {
        do
        {
            tui.PrintHeading("Rules")
                .Print(" l - list rule")
                .Print(" n - new rule")
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
                    Create(tui.NewScope("Create"));
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
        tui.PrintStrong("List rules");

        if (_registry.Rules.Any())
        {
            foreach (var rule in _registry.Rules)
            {
                tui.Print($" - {rule.Name}  ({rule.GetType()})");
            }
        }
        else
        {
            tui.Print("No rules");
        }

        tui.Spacer();
    }

    private void Create(TUIHelper tui)
    {
        tui.PrintStrong("Create Rule");

        _entityHelper.Creator<IRule, IRuleFactory, IRuleConfiguration>(
            tui.NewScope("new rule"),
            (entityTypeName) => _ruleFactory.CreateConfiguration(),
            (entityTypeName, configuration) => _ruleFactory.Validate(configuration),
            (entityTypeName, entityName, configuration) =>
            {
                var rule = _ruleFactory.Create(entityName, configuration);
                _registry.AddRule(rule);
            },
            new List<string> {  "rule" }
        );
    }
}