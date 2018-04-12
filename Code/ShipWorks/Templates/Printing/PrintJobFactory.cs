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
            IEnumerable<PrintableBarcode> shippingProfileBarcodeData = shippingProfileService.GetConfiguredShipmentTypeProfiles()
                .Where(p => !string.IsNullOrWhiteSpace(p.Barcode) || !string.IsNullOrWhiteSpace(p.ShortcutKey))
                .Select(p => new PrintableBarcode(p.ShippingProfileEntity.Name, p.Barcode, p.ShortcutKey));

            BarcodePage profileBarcodePage = new BarcodePage("Shipping Profiles", shippingProfileBarcodeData);
            BarcodePage shortcutsBarcodePage = new BarcodePage("ShipWorks Shortcuts", GetBuiltInShortcutData());
            
            return new BarcodePrintJob(this, new[] { profileBarcodePage, shortcutsBarcodePage }, telemetryEventFunc);
        }
        
        /// <summary>
        /// Get a list of profiles shortcut data
        /// </summary>
        private IEnumerable<PrintableBarcode> GetBuiltInShortcutData()
        {
            List<PrintableBarcode> barcodes = new List<PrintableBarcode>();

            shortcutManager.Shortcuts.Where(s => s.Action == KeyboardShortcutCommand.CreateLabel)
                .ForEach(s => barcodes.Add(new PrintableBarcode("Create Label", s.Barcode, new KeyboardShortcutData(s).ShortcutText)));

            shortcutManager.Shortcuts.Where(s => s.Action == KeyboardShortcutCommand.ApplyWeight)
                .ForEach(s => barcodes.Add(new PrintableBarcode("Apply Weight", s.Barcode, new KeyboardShortcutData(s).ShortcutText)));

            return barcodes;
        }
        
        /// <summary>
        /// Crate a print job with the given template result
        /// </summary>
        public IPrintJob CreatePrintJob(IList<TemplateResult> templateResults) => 
            PrintJob.Create(templateResults);
    }
}
