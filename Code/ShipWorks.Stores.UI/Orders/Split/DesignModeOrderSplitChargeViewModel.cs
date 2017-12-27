namespace ShipWorks.Stores.UI.Orders.Split
{
    /// <summary>
    /// SplitChargeViewModel for design mode
    /// </summary>
    public class DesignModeOrderSplitChargeViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DesignModeOrderSplitChargeViewModel()
        {
            Type = "Sales Tax";
            OriginalAmount = 3;
            SplitAmount = 1;
        }

        /// <summary>
        /// Type of the charge
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Amount of the charge on the original order
        /// </summary>
        public double OriginalAmount { get; set; }

        /// <summary>
        /// Amount of the charge on the split order
        /// </summary>
        public double SplitAmount { get; set; }
    }
}
