using System.Data.SqlTypes;

namespace ShipWorks.Filters.Content.Conditions
{
    /// <summary>
    /// Abstract condition to be used for money data types.
    /// </summary>
    public abstract class MoneyCondition : NumericCondition<decimal>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MoneyCondition"/> class.
        /// </summary>
        protected MoneyCondition()
        {
            minimumValue = SqlMoney.MinValue.ToDecimal();
            maximumValue = SqlMoney.MaxValue.ToDecimal();

            // Format as currency
            format = "C";
        }
    }
}
