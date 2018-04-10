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
            Dictionary<string, IEnumerable<(string Name, string Barcode, string KeyboardShortcut)>> shortcutDat =
                new Dictionary<string, IEnumerable<(string Name, string Barcode, string KeyboardShortcut)>>();

            shortcutDat.Add("ShipWorks Shortcuts", GetBuiltInShortcutData());
            shortcutDat.Add("Shipping Profiles", GetProfileShortcutData(shippingProfileService.GetConfiguredShipmentTypeProfiles()));
            
            return CreateBarcodePrintJob(shortcutDat);
        }
        
        /// <summary>
        /// Create a barcode print job
        /// </summary>
        public IPrintJob CreateBarcodePrintJob(IEnumerable<IShippingProfile> shippingProfiles)
        {
            Dictionary<string, IEnumerable<(string Name, string Barcode, string KeyboardShortcut)>> shortcutDat = 
                new Dictionary<string, IEnumerable<(string Name, string Barcode, string KeyboardShortcut)>>();

            shortcutDat.Add("ShipWorks Shortcuts", GetBuiltInShortcutData());
            shortcutDat.Add("Shipping Profiles", GetProfileShortcutData(shippingProfiles));
            
            return CreateBarcodePrintJob(shortcutDat);
        }

        /// <summary>
        /// Get a list of profiles shortcut data
        /// </summary>
        private IEnumerable<(string Name, string Barcode, string KeyboardShortcut)> GetBuiltInShortcutData()
        {
            List<(string Name, string Barcode, string KeyboardShortcut)> profileShortcutData = new List<(string Name, string Barcode, string KeyboardShortcut)>();

            shortcutManager.Shortcuts.Where(s => s.Action == KeyboardShortcutCommand.CreateLabel)
                .ForEach(s => profileShortcutData.Add(("Create Label", s.Barcode, new KeyboardShortcutData(s).ShortcutText)));

            shortcutManager.Shortcuts.Where(s => s.Action == KeyboardShortcutCommand.ApplyWeight)
                .ForEach(s => profileShortcutData.Add(("Apply Weight", s.Barcode, new KeyboardShortcutData(s).ShortcutText)));

            return profileShortcutData;
        }
        
        /// <summary>
        /// Get a list of profiles shortcut data
        /// </summary>
        private IEnumerable<(string Name, string Barcode, string KeyboardShortcut)> GetProfileShortcutData(IEnumerable<IShippingProfile> shippingProfiles)
        {
            List<(string Name, string Barcode, string KeyboardShortcut)> profileShortcutData = new List<(string Name, string Barcode, string KeyboardShortcut)>();

            foreach (IShippingProfile profile in shippingProfiles)
            {
                profileShortcutData.Add((profile.ShippingProfileEntity.Name, profile.Barcode, profile.ShortcutKey));
            }

            return profileShortcutData;
        }

        /// <summary>
        /// create a barcode print job using the dictionary 
        /// </summary>
        private IPrintJob CreateBarcodePrintJob(IDictionary<string, IEnumerable<(string Name, string Barcode, string KeyboardShortcut)>> shortcutData) =>
            new BarcodePrintJob(this, shortcutData, telemetryEventFunc);

        /// <summary>
        /// Crate a print job with the given template result
        /// </summary>
        public IPrintJob CreatePrintJob(IList<TemplateResult> templateResults) => 
            PrintJob.Create(templateResults);
    }
}
