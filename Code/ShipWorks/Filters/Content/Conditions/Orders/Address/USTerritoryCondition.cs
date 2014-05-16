using ShipWorks.AddressValidation;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Orders.Address
{
    /// <summary>
    /// Condition that compares against the US territory field of an order's shipping address
    /// </summary>
    [ConditionElement("US Territory", "Order.Address.USTerritory")]
    public class USTerritoryCondition : BillShipAddressEnumValueCondition<ValidationDetailStatusType>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public USTerritoryCondition()
        {
            Value = ValidationDetailStatusType.Unknown;
        }

        /// <summary>
        /// Generate the SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(OrderFields.BillUSTerritory), context.GetColumnReference(OrderFields.ShipUSTerritory), context);
        }
    }
}