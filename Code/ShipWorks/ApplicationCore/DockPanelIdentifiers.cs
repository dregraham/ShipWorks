using System;
using TD.SandDock;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Central location that can be used to locate UI panels
    /// </summary>
    public static class DockPanelIdentifiers
    {
        /// <summary>
        /// GUID of the rating panel
        /// </summary>
        public readonly static Guid RatingPanelGuid = new Guid("B82A3A5F-931A-40E7-AB35-9189D564C187");

        /// <summary>
        /// GUID of the shipments panel
        /// </summary>
        public readonly static Guid ShipmentsPanelGuid = new Guid("B65CC6D7-1B93-43AD-B0E6-23BC4B1EC699");

        /// <summary>
        /// GUID of the shipping panel
        /// </summary>
        public readonly static Guid ShippingPanelGuid = new Guid("574C96CC-5D02-4689-9463-4FB4DBCE22AD");

        /// <summary>
        /// Is the specified control the rating panel control
        /// </summary>
        public static bool IsRatingPanel(DockControl control) => control?.Guid == RatingPanelGuid;

        /// <summary>
        /// Is the specified control the shipments panel
        /// </summary>
        public static bool IsShipmentsPanel(DockControl control) => control?.Guid == ShipmentsPanelGuid;

        /// <summary>
        /// Is the specified control the shipping panel
        /// </summary>
        public static bool IsShippingPanel(DockControl control) => control?.Guid == ShippingPanelGuid;
    }
}
