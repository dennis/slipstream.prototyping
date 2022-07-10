namespace Slipstream.Core.Forms;

public class FormGenerator : IFormGenerator
{
    public FormCollection Generate(object o)
    {
        var formElements = new List<IFormElement>();

        var properties = o.GetType().GetProperties();

        foreach (var property in properties)
        {
            if (property.PropertyType == typeof(string))
            {
                formElements.Add(new StringFormElement(property.Name, property.GetValue(o) as string, property));
            }
            else
            {
                formElements.Add(new UnsupportedFormElement(property));
            }
        }

        return new FormCollection(formElements);
    }
}
