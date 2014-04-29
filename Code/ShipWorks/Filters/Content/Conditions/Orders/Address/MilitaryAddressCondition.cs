using ShipWorks.AddressValidation;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Orders.Address
{
    /// <summary>
    /// Condition that compares against the military address field of an order's shipping address
    /// </summary>
    [ConditionElement("Military Address", "Order.Address.MilitaryAddress")]
    public class MilitaryAddressCondition : EnumCondition<MilitaryAddressType>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MilitaryAddressCondition()
        {
            Value = MilitaryAddressType.Unknown;
        }

        /// <summary>
        /// Generate the SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(OrderFields.ShipMilitaryAddress), context);
        }
    }
}