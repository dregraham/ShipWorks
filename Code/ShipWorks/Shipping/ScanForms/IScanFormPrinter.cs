using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.Shipping.ScanForms
{
    public interface IScanFormPrinter
    {
        /// <summary>
        /// Prints the SCAN form.
        /// </summary>
        /// <param name="owner">The IWin32Window object that a print dialog will below to if needed.</param>
        /// <param name="scanForm">The SCAN form.</param>
        /// <returns>Returns [true] if the SCAN form was printed successfully; otherwise [false].</returns>
        bool Print(IWin32Window owner, ScanForm scanForm);
    }
}
