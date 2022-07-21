using Slipstream.Domain.Entities;
using Slipstream.Domain.Extensions;
using Slipstream.Domain.Forms;
using Slipstream.Domain.ValueObjects;

namespace Slipstream.CLI;

internal class EntityHelper
{
    private readonly IFormGenerator _formGenerator;

    public EntityHelper(IFormGenerator formGenerator)
    {
        _formGenerator = formGenerator;
    }

    public void Creator<TInstance, TFactory, TConfiguration>(
        TUIHelper tui,
        Func<EntityTypeName, TConfiguration?> configurationCreator,
        Func<EntityTypeName, TConfiguration?, ConfigurationValidation> validateConfiguration,
        Action<EntityTypeName, EntityName, TConfiguration?> adder,
        IList<string> entityTypes
    )
        where TInstance : IEntity
        where TFactory : class
    {
        string entityTypeName = "";

        tui.PrintStrong("Select type");

        foreach (var (item, index) in entityTypes.WithIndex())
        {
            tui.Print($"{index + 1}: {item}");
        }

        if(int.TryParse(tui.Prompt("Please select"), out int idx) && idx > 0 && idx <= entityTypes.Count)
        {
            entityTypeName = entityTypes[idx-1];
        }
        else
        {
            tui.Error("Invalid input!");
            return;
        }

        var entityName = tui.Prompt("name");
        var configTui = tui.NewScope("configuration");
        configTui.PrintStrong("configuration");

        var config = configurationCreator(entityTypeName);
        if (config is not null)
        {
            var form = _formGenerator.Generate(config);
            form.Visit(new ConsoleFormVisitor(configTui));
            form.Populate(config);

            var result = validateConfiguration(entityTypeName, config);

            if (result.Errors.Any())
            {
                foreach (var err in result.Errors)
                {
                    configTui.Error($"{err.Key}: {err.Value}");
                }
                return;
            }
        }

        adder(entityTypeName, entityName, config);

        tui.Spacer();
    }
}