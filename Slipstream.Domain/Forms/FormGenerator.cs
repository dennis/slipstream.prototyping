using Slipstream.Domain.Attributes;

namespace Slipstream.Domain.Forms;

public class FormGenerator : IFormGenerator
{
    public FormCollection Generate(object o)
    {
        var formElements = new List<IFormElement>();

        var properties = o.GetType().GetProperties();

        foreach (var property in properties)
        {
            var a = property.GetCustomAttributes(false).FirstOrDefault(a => a is FormHelp);

            if (property.PropertyType == typeof(string))
            {
                formElements.Add(new StringFormElement(property.Name, property.GetValue(o) as string, property, (a as FormHelp)?.Description));
            }
            else
            {
                formElements.Add(new UnsupportedFormElement(property));
            }
        }

        return new FormCollection(formElements);
    }
}
