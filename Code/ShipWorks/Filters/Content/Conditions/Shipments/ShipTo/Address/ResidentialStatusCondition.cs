using ShipWorks.AddressValidation;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Shipments.ShipTo.Address
{
    /// <summary>
    /// Condition that compares against the residential status of a shipment's ShipTo address
    /// </summary>
    [ConditionElement("Residential Status", "Shipment.ShipTo.ResidentialStatus")]
    public class ResidentialStatusCondition : EnumCondition<ResidentialStatusType>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ResidentialStatusCondition()
        {
            Value = ResidentialStatusType.Unknown;
        }

        /// <summary>
        /// Generate the SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(ShipmentFields.ShipResidentialStatus), context);
        }
    }
}
