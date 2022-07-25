namespace Slipstream.Domain.Forms;

public interface IFormElement
{
    string Name { get; }

    void Accept(IFormCollectionVisitor visitor);
}
