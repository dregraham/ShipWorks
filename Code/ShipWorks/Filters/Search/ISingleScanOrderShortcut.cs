﻿using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Filters.Search
{
    /// <summary>
    /// Prefix that identifies a scan result as a ShipWorks order
    /// </summary>
    [Service]
    public interface ISingleScanOrderShortcut
    {
        /// <summary>
        /// Get scan result that is displayed to user
        /// </summary>
        string GetDisplayText(string barcodeText);

        /// <summary>
        /// Get the OrderID from the barcodeText
        /// </summary>
        long GetOrderID(string barcodeText);

        /// <summary>
        /// Get the given orders shortcut
        /// </summary>
        string GetShortcutText(IOrderEntity order);

        /// <summary>
        /// Whether or not the scan result starts with the ShipWorks order prefix
        /// and ends with the order entity seed value
        /// </summary>
        bool AppliesTo(string barcodeText);
    }
}
