using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Templates.Printing
{
    /// <summary>
    /// Interface a class can implement to indicate that it allows the user to configure templates to print with.  This enables
    /// the TemplatePrinterSelectionDlg a way to easily determine every template that the user still needs to configure a printer for.
    /// </summary>
    public interface IPrintWithTemplates
    {
        IEnumerable<long> TemplatesToPrintWith { get; }
    }
}
