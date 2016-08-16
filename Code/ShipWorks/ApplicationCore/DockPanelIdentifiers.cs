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
        /// GUID of the shipping panel
        /// </summary>
        public readonly static Guid ShippingPanelGuid = new Guid("574C96CC-5D02-4689-9463-4FB4DBCE22AD");

        /// <summary>
        /// Is the specified control the rating panel control
        /// </summary>
        public static bool IsRatingPanel(DockControl control)
        {
            return control?.Guid == RatingPanelGuid;
        }
    }
}
