using System;
using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
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
        private readonly Func<string, ITrackedEvent> telemetryEventFunc;
        private readonly IShippingProfileService shippingProfileService;

        /// <summary>
        /// Constructor
        /// </summary>
        public PrintJobFactory(Func<string, ITrackedEvent> telemetryEventFunc, IShippingProfileService shippingProfileService)
        {
            this.telemetryEventFunc = telemetryEventFunc;
            this.shippingProfileService = shippingProfileService;
        }
        
        /// <summary>
        /// Create a barcode print job for all barcodes
        /// </summary>
        public IPrintJob CreateBarcodePrintJob()
        {
            return CreateBarcodePrintJob(shippingProfileService.GetConfiguredShipmentTypeProfiles());
        }

        /// <summary>
        /// Create a barcode print job
        /// </summary>
        public IPrintJob CreateBarcodePrintJob(IEnumerable<IShippingProfile> shippingProfiles)
        {
            List<(string Name, string Barcode, string KeyboardShortcut)> profileShortcutData = new List<(string Name, string Barcode, string KeyboardShortcut)>();
            
            foreach (IShippingProfile profile in shippingProfiles)
            {
                profileShortcutData.Add((profile.ShippingProfileEntity.Name, profile.Barcode, profile.ShortcutKey));
            }

            Dictionary<string, IEnumerable<(string Name, string Barcode, string KeyboardShortcut)>> shortcutDat = 
                new Dictionary<string, IEnumerable<(string Name, string Barcode, string KeyboardShortcut)>>();

            shortcutDat.Add("Shipping Profiles", profileShortcutData);

            return new BarcodePrintJob(this, shortcutDat, telemetryEventFunc);
        }
            

        /// <summary>
        /// Crate a print job with the given template result
        /// </summary>
        public IPrintJob CreatePrintJob(IList<TemplateResult> templateResults) => 
            PrintJob.Create(templateResults);
    }
}
