using ShipWorks.Filters.Content.Editors.ValueEditors;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions
{
    /// <summary>
    /// Base for all conditions that are address and have a billing and shipping component
    /// </summary>
    public abstract class BillShipAddressStringCondition : StringCondition
    {
        BillShipAddressOperator addressOperator = BillShipAddressOperator.ShipOrBill;

        /// <summary>
        /// How to apply the condition to the Billing\Shipping portions of the address
        /// </summary>
        public BillShipAddressOperator AddressOperator
        {
            get
            {
                return addressOperator;
            }
            set
            {
                addressOperator = value;
            }
        }

        /// <summary>
        /// Generate the sql for the condition based on using the given expressions as the billing and shipping portions to compare.
        /// </summary>
        protected string GenerateSql(string billExpression, string shipExpression, SqlGenerationContext context)
        {
            return BillShipAddressConditionUtility.GenerateSqlInternal(context, addressOperator, billExpression, shipExpression, GenerateSql);
        }

        /// <summary>
        /// Create the editor
        /// </summary>
        public override ValueEditor CreateEditor()
        {
            return new BillShipAddressStringValueEditor(this);
        }
    }
}
