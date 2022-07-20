using Slipstream.Domain;
using Slipstream.Domain.Instances;

namespace Slipstream.CLI.MenuHandlers;

internal class InstanceMenuHandler
{
    private readonly IRegistry _registry;
    private readonly EntityHelper _entityHelper;

    public InstanceMenuHandler(
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
            tui.PrintHeading("Instances")
                .Print(" l - list")
                .Print(" n - new")
                .Print(" b - back")
                .Spacer();

            var input = tui.ReadKey();

            switch (input)
            {
                case 'l':
                case 'L':
                    ShowInstances(tui);
                    break;

                case 'n':
                case 'N':
                    CreateInstance(tui);
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

    private void ShowInstanceTypes(TUIHelper tui)
    {
        tui.PrintStrong("Instance types available:");

        foreach (var instanceType in _registry.InstanceContainer.Keys)
        {
            tui.Print($" - {instanceType}");
        }

        tui.Spacer();
    }

    private void CreateInstance(TUIHelper tui)
    {
        ShowInstanceTypes(tui);

        _entityHelper.Creator<IInstance, IInstanceFactory, IInstanceConfiguration>(
            tui.NewScope("new instance"),
            (entityTypeName) => _registry.InstanceContainer[entityTypeName].CreateConfiguration(),
            (entityTypeName, configuration) => _registry.InstanceContainer[entityTypeName].Validate(configuration),
            (entityTypeName, entityName, configuration) =>
            {
                var instance = _registry.InstanceContainer[entityTypeName].Create(entityName, configuration);
                _registry.AddInstance(instance);
            }, 
            _registry.InstanceContainer.Keys.ToList()
        );
    }

    private void ShowInstances(TUIHelper tui)
    {
        tui.PrintStrong("Instances available:");

        foreach (var instance in _registry.InstanceContainer.Instances)
        {
            tui.Print($" - {instance.Name}");
        }

        tui.Spacer();
    }

}