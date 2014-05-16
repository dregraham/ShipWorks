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
            if (addressOperator == BillShipAddressOperator.Bill)
            {
                return GenerateSql(billExpression, context);
            }

            if (addressOperator == BillShipAddressOperator.Ship)
            {
                return GenerateSql(shipExpression, context);
            }

            if (addressOperator == BillShipAddressOperator.ShipBillEqual)
            {
                return string.Format("{0} = {1}", billExpression, shipExpression);
            }

            if (addressOperator == BillShipAddressOperator.ShipBillNotEqual)
            {
                return string.Format("{0} != {1}", billExpression, shipExpression);
            }

            string join = (addressOperator == BillShipAddressOperator.ShipAndBill) ? "AND" : "OR";

            return string.Format("({0}) {1} ({2})",
                GenerateSql(billExpression, context),
                join,
                GenerateSql(shipExpression, context));
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
