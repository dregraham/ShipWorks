using System.Collections.Generic;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Templates.Processing;

namespace ShipWorks.Templates.Printing
{
    /// <summary>
    /// Factory for creating print jobs
    /// </summary>
    [Component]
    public class PrintJobFactory : IPrintJobFactory
    {
        /// <summary>
        /// Create a barcode print job
        /// </summary>
        public IPrintJob CreateBarcodePrintJob(IEnumerable<IShippingProfile> shippingProfiles) => 
            new BarcodePrintJob(this, shippingProfiles);

        /// <summary>
        /// Crate a print job with the given template result
        /// </summary>
        public IPrintJob CreatePrintJob(IList<TemplateResult> templateResults) => 
            PrintJob.Create(templateResults);
    }
}
