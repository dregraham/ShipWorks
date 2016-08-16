namespace ShipWorks.Shipping
{
    /// <summary>
    /// An enumeration to be used in conjunction with the ShippingDlg to indicate which tab should
    /// be initially displayed.
    /// </summary>
    public enum InitialShippingTabDisplay
    {
        /// <summary>
        /// The shipping tab of the shipping dialog should be displayed
        /// </summary>
        Shipping = 0,

        /// <summary>
        /// The tracking tab of the shipping dialog should be displayed
        /// </summary>
        Tracking = 1,

        /// <summary>
        /// The insurance tab of the tracking dialog should be displayed
        /// </summary>
        Insurance = 2
    }
}
