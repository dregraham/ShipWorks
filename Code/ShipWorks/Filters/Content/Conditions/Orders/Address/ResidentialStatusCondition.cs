using ShipWorks.AddressValidation;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Orders.Address
{
    /// <summary>
    /// Condition that compares against the residential status of an order's shipping address
    /// </summary>
    [ConditionElement("Residential Status", "Order.Address.ResidentialStatus")]
    public class ResidentialStatusCondition : EnumCondition<ValidationDetailStatusType>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ResidentialStatusCondition()
        {
            Value = ValidationDetailStatusType.Unknown;
        }

        /// <summary>
        /// Generate the SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(OrderFields.ShipResidentialStatus), context);
        }
    }
}
