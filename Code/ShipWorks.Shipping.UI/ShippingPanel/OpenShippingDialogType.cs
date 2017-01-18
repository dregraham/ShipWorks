namespace ShipWorks.Shipping.UI.ShippingPanel
{
    /// <summary>
    /// How should the dialog be opened
    /// </summary>
    public enum OpenShippingDialogType
    {
        /// <summary>
        /// Type isn't specified
        /// </summary>
        None,

        /// <summary>
        /// Open with all shipments
        /// </summary>
        AllShipments,

        /// <summary>
        /// Open with the selected shipment
        /// </summary>
        SelectedShipment,

        /// <summary>
        /// Open with selected orders
        /// </summary>
        SelectedOrders,
    }
}
