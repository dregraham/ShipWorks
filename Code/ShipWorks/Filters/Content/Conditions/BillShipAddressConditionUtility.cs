using System;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions
{
    /// <summary>
    /// Utility for generatin SQL statements for BillShipAddressCondition classes
    /// </summary>
    public static class BillShipAddressConditionUtility
    {
        /// <summary>
        /// Generate the sql for the condition based on using the given expressions as the billing and shipping portions to compare.
        /// </summary>
        public static string GenerateSqlInternal(SqlGenerationContext context, BillShipAddressOperator addressOperator, 
            string billExpression, string shipExpression, Func<string, SqlGenerationContext, string> generateSql)
        {
            if (addressOperator == BillShipAddressOperator.Bill)
            {
                return generateSql(billExpression, context);
            }

            if (addressOperator == BillShipAddressOperator.Ship)
            {
                return generateSql(shipExpression, context);
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
                generateSql(billExpression, context),
                join,
                generateSql(shipExpression, context));
        }
    }
}