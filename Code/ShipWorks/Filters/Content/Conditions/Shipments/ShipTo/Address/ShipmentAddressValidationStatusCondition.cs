using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Filters.Content.Conditions.Shipments.ShipTo.Address
{
    /// <summary>
    /// Filter on the address validtion status of the shipment's ShipTo address
    /// </summary>
    [ConditionElement("Address Validation Status", "Shipment.ShipTo.AddressValidationStatus")]
    public class ShipmentAddressValidationStatusCondition : AddressValidationStatusCondition
    {
        /// <summary>
        /// Get the validation field that should be filtered on
        /// </summary>
        protected override EntityField2 ValidationField
        {
            get
            {
                return ShipmentFields.ShipAddressValidationStatus;
            }
        }
    }
}
