using ShipWorks.Data.Grid.Columns;

namespace ShipWorks.Stores.Platforms.Ebay.CoreExtensions.Grid
{
    /// <summary>
    /// A column to indicate whether an eBay order is eligible for the Global Shipping Program.
    /// </summary>
    public class GridEbayGlobalShippingProgramEligibilityDisplayType : GridColumnDisplayType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GridEbayGlobalShippingProgramEligibilityDisplayType"/> class.
        /// </summary>
        public GridEbayGlobalShippingProgramEligibilityDisplayType()
            : base()
        { }

        /// <summary>
        /// Get the display text to use for the given value and entity
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override string GetDisplayText(object value)
        {
            string text = string.Empty;

            if (value != null)
            {
                bool isEligible = (bool)value;
                text = isEligible ? "Yes" : "No";
            }

            return text;
        }

    }
}
