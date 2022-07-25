namespace Slipstream.Domain.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class PropertyHelp : Attribute
{
    public string Description { get; }

    public PropertyHelp(string text)
        => Description = text; 
}
