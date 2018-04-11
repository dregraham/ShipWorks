using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.IO.KeyboardShortcuts;
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
        private readonly IShortcutManager shortcutManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public PrintJobFactory(
            Func<string, ITrackedEvent> telemetryEventFunc, 
            IShippingProfileService shippingProfileService, 
            IShortcutManager shortcutManager)
        {
            this.telemetryEventFunc = telemetryEventFunc;
            this.shippingProfileService = shippingProfileService;
            this.shortcutManager = shortcutManager;
        }
        
        /// <summary>
        /// Create a barcode print job for all barcodes
        /// </summary>
        public IPrintJob CreateBarcodePrintJob()
        {
            var barcodeData = new Dictionary<string, IEnumerable<(string Name, string Barcode, string KeyboardShortcut)>>();
            
            IEnumerable<(string Name, string Barcode, string KeyboardShortcut)> shippingProfileBarcodeData = shippingProfileService.GetConfiguredShipmentTypeProfiles()
                .Select(profile => (profile.ShippingProfileEntity.Name, profile.Barcode, profile.ShortcutKey));

            barcodeData.Add("Shipping Profiles", shippingProfileBarcodeData);
            barcodeData.Add("ShipWorks Shortcuts", GetBuiltInShortcutData());

            return new BarcodePrintJob(this, barcodeData, telemetryEventFunc);
        }
        
        /// <summary>
        /// Get a list of profiles shortcut data
        /// </summary>
        private IEnumerable<(string Name, string Barcode, string KeyboardShortcut)> GetBuiltInShortcutData()
        {
            var barcodeData = new List<(string Name, string Barcode, string KeyboardShortcut)>();

            shortcutManager.Shortcuts.Where(s => s.Action == KeyboardShortcutCommand.CreateLabel)
                .ForEach(s => barcodeData.Add(("Create Label", s.Barcode, new KeyboardShortcutData(s).ShortcutText)));

            shortcutManager.Shortcuts.Where(s => s.Action == KeyboardShortcutCommand.ApplyWeight)
                .ForEach(s => barcodeData.Add(("Apply Weight", s.Barcode, new KeyboardShortcutData(s).ShortcutText)));

            return barcodeData;
        }
        
        /// <summary>
        /// Crate a print job with the given template result
        /// </summary>
        public IPrintJob CreatePrintJob(IList<TemplateResult> templateResults) => 
            PrintJob.Create(templateResults);
    }
}
