using ShipWorks.AddressValidation;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Orders.Address
{
    /// <summary>
    /// Condition that compares against the PO Box field of an order's shipping address
    /// </summary>
    [ConditionElement("PO Box", "Order.Address.POBox")]
    public class POBoxCondition : EnumCondition<POBoxType>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public POBoxCondition()
        {
            Value = POBoxType.Unknown;
        }

        /// <summary>
        /// Generate the SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(OrderFields.ShipPOBox), context);
        }
    }
}