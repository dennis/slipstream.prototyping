namespace Slipstream.Domain.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class FormHelp : Attribute
{
    public string Description { get; }

    public FormHelp(string text)
        => Description = text; 
}
