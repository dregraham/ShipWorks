using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Orders
{
    /// <summary>
    /// Condition base on the Order# of an Order
    /// </summary>
    [ConditionElement("Order Number", "Order.Number")]
    public class OrderNumberCondition : NumericStringCondition<long>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderNumberCondition()
        {
            IsNumeric = false;
        }

        /// <summary>
        /// Generate the SQL that evaluates the condition
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            string orderNumberSql = string.Empty;
            string orderSearchSql = string.Empty;
            string storeCombinedOrderSearchSql = string.Empty;

            if (IsNumeric)
            {
                orderNumberSql = $"{GenerateSql(context.GetColumnReference(OrderFields.OrderNumber), context)} AND OrderNumber != {long.MinValue}";
            }
            else
            {
                orderNumberSql = StringCondition.GenerateSql(StringValue, StringOperator, context.GetColumnReference(OrderFields.OrderNumberComplete), context);
            }

            orderSearchSql = GenerateCombinedOrderSearchSql(context);

            return $"{orderNumberSql} OR {orderSearchSql}";
        }

        /// <summary>
        /// Get the SQL for searching order specific combined order numbers
        /// </summary>
        private string GenerateCombinedOrderSearchSql(SqlGenerationContext context)
        {
            EntityField2 searchField = IsNumeric ? OrderSearchFields.OrderNumber : OrderSearchFields.OrderNumberComplete;

            CombinedOrderNumberCondition combinedOrderNumberCondition = new CombinedOrderNumberCondition(searchField);
            combinedOrderNumberCondition.IsNumeric = IsNumeric;

            if (IsNumeric)
            {
                combinedOrderNumberCondition.Operator = Operator;
                combinedOrderNumberCondition.Value1 = Value1;
                combinedOrderNumberCondition.Value2 = Value2;
            }
            else
            {
                combinedOrderNumberCondition.StringOperator = StringOperator;
                combinedOrderNumberCondition.StringValue = StringValue;
            }

            return combinedOrderNumberCondition.GenerateSql(context);
        }
    }
}
