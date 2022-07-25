using Slipstream.Domain.Attributes;
using Slipstream.Domain.Entities;

namespace Slipstream.Domain.DynamicProperties;

public class PropertyScanner : IPropertyScanner
{
    public PropertyScanner(IEnumerable<IEntityConfiguration> configurations)
    {
        // Todo, cache these properties

    }

    public PropertyCollection Generate(object o)
    {
        var propertiesFound = new List<IProperty>();

        var properties = o.GetType().GetProperties();

        foreach (var property in properties)
        {
            var a = property.GetCustomAttributes(false).FirstOrDefault(a => a is PropertyHelp);

            if (property.PropertyType == typeof(string))
            {
                propertiesFound.Add(new StringProperty(property.Name, property.GetValue(o) as string, property, (a as PropertyHelp)?.Description));
            }
            else
            {
                propertiesFound.Add(new UnsupportedProperty(property));
            }
        }

        return new PropertyCollection(propertiesFound);
    }
}
