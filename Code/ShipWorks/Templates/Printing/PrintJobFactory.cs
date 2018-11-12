using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
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
            this.shippingProfileService = shippingProfileService;
            this.shortcutManager = shortcutManager;
        }

        /// <summary>
        /// Create a barcode print job for all barcodes
        /// </summary>
        public IPrintJob CreateBarcodePrintJob()
        {
            IEnumerable<PrintableBarcode> shippingProfileBarcodeData = shippingProfileService
                .GetConfiguredShipmentTypeProfiles()
                .Where(p => p.HasShortcutOrBarcode)
                .Select(p => p.ToPrintableBarcode());

            BarcodePage profileBarcodePage = new BarcodePage("Shipping Profiles", shippingProfileBarcodeData);
            BarcodePage shortcutsBarcodePage = new BarcodePage("ShipWorks Shortcuts", GetBuiltInShortcutData());

            return new BarcodePrintJob(this, new[] { profileBarcodePage, shortcutsBarcodePage });
        }

        /// <summary>
        /// Get a list of profiles shortcut data
        /// </summary>
        private IEnumerable<PrintableBarcode> GetBuiltInShortcutData() =>
            shortcutManager.Shortcuts
                .Where(s => s.Action != KeyboardShortcutCommand.ApplyProfile)
                .Select(s => new PrintableBarcode(EnumHelper.GetDescription(s.Action), s.Barcode, new KeyboardShortcutData(s).ShortcutText));

        /// <summary>
        /// Crate a print job with the given template result
        /// </summary>
        public IPrintJob CreatePrintJob(IList<TemplateResult> templateResults) =>
            PrintJob.Create(templateResults);
    }
}
