using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.Editors.ValueEditors;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Shipments.ShipTo.Address
{
    /// <summary>
    /// Filter on the address validtion status of the shipment's ShipTo address
    /// </summary>
    [ConditionElement("Validation Status", "Shipment.ShipTo.AddressValidationStatus")]
    public class ShipmentAddressValidationStatusCondition : AddressValidationStatusCondition
    {
        /// <summary>
        /// Generate the SQL required for the filter
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(ShipmentFields.ShipAddressValidationStatus), context);
        }

        /// <summary>
        /// Create the editor for this filter
        /// </summary>
        /// <returns></returns>
        public override ValueEditor CreateEditor()
        {
            return new AddressValidationStatusValueEditor(this);
        }
    }
}
