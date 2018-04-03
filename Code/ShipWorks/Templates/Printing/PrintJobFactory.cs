using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Services;
using ShipWorks.Templates.Processing;

namespace ShipWorks.Templates.Printing
{
    /// <summary>
    /// Factory for creating print jobs
    /// </summary>
    [Component]
    public class PrintJobFactory : IPrintJobFactory
    {
        private readonly Func<string, ITrackedEvent> telemetryEventFunc;
        private readonly IShippingProfileRepository shippingProfileRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public PrintJobFactory(Func<string, ITrackedEvent> telemetryEventFunc, IShippingProfileRepository shippingProfileRepository)
        {
            this.telemetryEventFunc = telemetryEventFunc;
            this.shippingProfileRepository = shippingProfileRepository;
        }
        
        /// <summary>
        /// Create a barcode print job for all barcodes
        /// </summary>
        public IPrintJob CreateBarcodePrintJob() =>
            new BarcodePrintJob(this, shippingProfileRepository.GetAll(), telemetryEventFunc);

        /// <summary>
        /// Create a barcode print job
        /// </summary>
        public IPrintJob CreateBarcodePrintJob(IEnumerable<IShippingProfile> shippingProfiles) => 
            new BarcodePrintJob(this, shippingProfiles, telemetryEventFunc);

        /// <summary>
        /// Crate a print job with the given template result
        /// </summary>
        public IPrintJob CreatePrintJob(IList<TemplateResult> templateResults) => 
            PrintJob.Create(templateResults);
    }
}
