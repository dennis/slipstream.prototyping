namespace Slipstream.Domain.Forms;

public interface IFormCollectionVisitor
{
    void VisitStringFormElement(StringFormElement element);
    void VisitUnsupportedFormElement(UnsupportedFormElement element);
}