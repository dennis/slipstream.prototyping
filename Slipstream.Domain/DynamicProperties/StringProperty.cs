using System.Reflection;

namespace Slipstream.Domain.DynamicProperties;

public class StringProperty : IProperty
{
    public string Name { get; private set; }
    public string? Value { get; set; }
    public PropertyInfo Property { get; }
    public string? FormHelp { get; }

    public StringProperty(string name, string? value, PropertyInfo property, string? formHelp)
    {
        Name = name;
        Value = value;
        Property = property;
        FormHelp = formHelp;
    }

    public void Accept(IPropertyVisitor visitor)
    {
        visitor.VisitStringProperty(this);
    }
}
