using System.Reflection;

namespace Slipstream.Domain.Forms;

public class StringFormElement : IFormElement
{
    public string Name { get; private set; }
    public string? Value { get; set; }
    public PropertyInfo Property { get; }
    public string? FormHelp { get; }

    public StringFormElement(string name, string? value, PropertyInfo property, string? formHelp)
    {
        Name = name;
        Value = value;
        Property = property;
        FormHelp = formHelp;
    }

    public void Accept(IFormCollectionVisitor visitor)
    {
        visitor.VisitStringFormElement(this);
    }
}
