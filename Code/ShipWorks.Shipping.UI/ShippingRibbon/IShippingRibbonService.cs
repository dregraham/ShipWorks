namespace ShipWorks.Shipping.UI.ShippingRibbon
{
    /// <summary>
    /// Service that handles interaction with the shipping ribbon
    /// </summary>
    public interface IShippingRibbonService
    {
        /// <summary>
        /// Register a set of actions with the ribbon service
        /// </summary>
        void Register(IShippingRibbonActions shippingRibbonActions);
    }
}