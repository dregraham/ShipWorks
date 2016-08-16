using ShipWorks.AddressValidation;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.Editors.ValueEditors;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Orders.Address
{
    /// <summary>
    /// Condition that compares against the military address field of an order's shipping address
    /// </summary>
    [ConditionElement("Military Address", "Order.Address.MilitaryAddress")]
    public class MilitaryAddressCondition : EnumCondition<ValidationDetailStatusType>, IBillShipAddressCondition
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MilitaryAddressCondition()
        {
            Value = ValidationDetailStatusType.Unknown;
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
                context.GetColumnReference(OrderFields.BillMilitaryAddress), 
                context.GetColumnReference(OrderFields.ShipMilitaryAddress), 
                GenerateSql);
        }

        /// <summary>
        /// Create the editor
        /// </summary>
        public override ValueEditor CreateEditor()
        {
            return new BillShipAddressEnumValueEditor<ValidationDetailStatusType>(this);
        }
    }
}