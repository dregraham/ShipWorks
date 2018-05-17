using System.Collections.Generic;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Templates.Processing;

namespace ShipWorks.Templates.Printing
{
    /// <summary>
    /// Represents a factory for creating print jobs
    /// </summary>
    public interface IPrintJobFactory
    {
        /// <summary>
        /// Create a print job from the given template result
        /// </summary>
        IPrintJob CreatePrintJob(IList<TemplateResult> templateResults);
        
        /// <summary>
        /// Create a barcode print job for all barcodes
        /// </summary>
        IPrintJob CreateBarcodePrintJob();
    }
}
