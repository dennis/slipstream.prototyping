﻿namespace Slipstream.Core.Forms;

public interface IFormCollectionVisitor
{
    void VisitStringFormElement(StringFormElement element);
    void VisitUnsupportedFormElement(UnsupportedFormElement element);
}