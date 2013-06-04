using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.Shipping.ScanForms
{
    public interface IScanFormBatchPrinter
    {
        /// <summary>
        /// Prints a batch of SCAN forms.
        /// </summary>
        /// <param name="owner">The IWin32Window object that a print dialog will below to if needed.</param>
        /// <param name="scanFormBatch">The scan form batch.</param>
        /// <returns>Returns [true] if the SCAN forms were printed successfully; otherwise [false].</returns>
        bool Print(IWin32Window owner, ScanFormBatch scanFormBatch);
    }
}
