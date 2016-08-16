using ShipWorks.AddressValidation;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Shipments.ShipTo.Address
{
    /// <summary>
    /// Condition that compares against the international territory field of an shipment's ShipTo address
    /// </summary>
    [ConditionElement("US Territory", "Shipment.ShipTo.USTerritory")]
    public class USTerritoryCondition : EnumCondition<ValidationDetailStatusType>
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
            return GenerateSql(context.GetColumnReference(ShipmentFields.ShipUSTerritory), context);
        }
    }
}