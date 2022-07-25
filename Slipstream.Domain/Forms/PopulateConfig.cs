namespace Slipstream.Domain.Forms;

public class PopulateConfig : IFormCollectionVisitor
{
    private readonly object _obj;

    public PopulateConfig(object obj)
    {
        _obj = obj;
    }

    public void VisitStringFormElement(StringFormElement element)
    {
        element.Property.SetValue(_obj, element.Value);
    }

    public void VisitUnsupportedFormElement(UnsupportedFormElement element)
    {
    }
}