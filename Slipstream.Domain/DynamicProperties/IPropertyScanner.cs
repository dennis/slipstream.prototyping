namespace Slipstream.Domain.DynamicProperties;

public interface IPropertyScanner
{
    public PropertyCollection Generate(object o);
}
