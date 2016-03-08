using ShipWorks.Core.UI.SandRibbon;

namespace ShipWorks.Shipping.UI.ShippingRibbon
{
    /// <summary>
    /// Actions that can be triggered from the shipping ribbon
    /// </summary>
    public interface IShippingRibbonActions
    {
        /// <summary>
        /// Create label action
        /// </summary>
        IRibbonButton CreateLabel { get; }

        /// <summary>
        /// Void label action
        /// </summary>
        IRibbonButton Void { get; }

        /// <summary>
        /// Create return shipment action
        /// </summary>
        IRibbonButton Return { get; }

        /// <summary>
        /// Reprint shipment action
        /// </summary>
        IRibbonButton Reprint { get; }

        /// <summary>
        /// Ship again action
        /// </summary>
        IRibbonButton ShipAgain { get; }
    }
}