using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.Editors.ValueEditors;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Orders.Address
{
    /// <summary>
    /// Filter on the validation status of the order's shipping address
    /// </summary>
    [ConditionElement("Address Validation Status", "Order.Address.ValidationStatus")]
    public class OrderAddressValidationStatusCondition : AddressValidationStatusCondition
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderAddressValidationStatusCondition()
        {
            AddressOperator = BillShipAddressOperator.ShipOrBill;
        }

        /// <summary>
        /// How to apply the condition to the Billing\Shipping portions of the address
        /// </summary>
        public BillShipAddressOperator AddressOperator { get; set; }

        /// <summary>
        /// Generate the SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return BillShipAddressConditionUtility.GenerateSqlInternal(context, AddressOperator, 
                context.GetColumnReference(OrderFields.BillAddressValidationStatus), 
                context.GetColumnReference(OrderFields.ShipAddressValidationStatus), 
                GenerateSql);
        }

        public override ValueEditor CreateEditor()
        {
            return new BillShipAddressValidationStatusEditor(this);
        }
    }
}
