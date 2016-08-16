using ShipWorks.AddressValidation;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Shipments.ShipTo.Address
{
    /// <summary>
    /// Condition that compares against the military address field of a shipment's ShipTo address
    /// </summary>
    [ConditionElement("Military Address", "Shipment.ShipTo.MilitaryAddress")]
    public class MilitaryAddressCondition : EnumCondition<ValidationDetailStatusType>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MilitaryAddressCondition()
        {
            Value = ValidationDetailStatusType.Unknown;
        }

        /// <summary>
        /// Generate the SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(ShipmentFields.ShipMilitaryAddress), context);
        }
    }
}