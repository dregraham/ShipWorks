using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Filters.Content.Conditions.Orders.Address
{
    /// <summary>
    /// Filter on the validation status of the order's shipping address
    /// </summary>
    [ConditionElement("Ship Address Validation Status", "Order.AddressValidationStatus")]
    public class OrderAddressValidationStatusCondition : AddressValidationStatusCondition
    {
        /// <summary>
        /// Get the validation field that should be filtered on
        /// </summary>
        protected override EntityField2 ValidationField
        {
            get
            {
                return OrderFields.ShipAddressValidationStatus;
            }
        }
    }
}
