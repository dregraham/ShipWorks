using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Amazon.CoreExtensions.Filters;

namespace ShipWorks.Filters.Content.Conditions.Orders
{
    /// <summary>
    /// Condition base on the Order# of an Order
    /// </summary>
    [ConditionElement("Order Number", "Order.Number")]
    public class OrderNumberCondition : NumericStringCondition<long>
    {
        private EntityField2 orderNumberField = null;
        private EntityField2 orderNumberCompleteField = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderNumberCondition()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderNumberCondition(EntityField2 orderNumberField, EntityField2 orderNumberCompleteField)
        {
            this.orderNumberField = orderNumberField;
            this.orderNumberCompleteField = orderNumberCompleteField;
        }

        /// <summary>
        /// Generate the SQL that evaluates the condition
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            string orderNumberSql = string.Empty;
            string orderSearchSql = string.Empty;

            if (IsNumeric)
            {
                orderNumberField = orderNumberField ?? OrderFields.OrderNumber;
                orderNumberSql = GenerateSql(context.GetColumnReference(orderNumberField), context);

                CombinedOrderNumberCondition combinedOrderNumberCondition = new CombinedOrderNumberCondition(OrderSearchFields.OrderNumber);
                combinedOrderNumberCondition.Operator = Operator;
                combinedOrderNumberCondition.Value1 = Value1;
                combinedOrderNumberCondition.Value2 = Value2;
                orderSearchSql = $" OR {combinedOrderNumberCondition.GenerateSql(context)}";

                AmazonCombinedOrderNumberCondition amazonCombinedOrderNumberCondition = new AmazonCombinedOrderNumberCondition(AmazonOrderSearchFields.OrderNumber);
                amazonCombinedOrderNumberCondition.Operator = Operator;
                amazonCombinedOrderNumberCondition.Value1 = Value1;
                amazonCombinedOrderNumberCondition.Value2 = Value2;
                orderSearchSql = $" {orderSearchSql} OR {amazonCombinedOrderNumberCondition.GenerateSql(context)}";
            }
            else
            {
                orderNumberCompleteField = orderNumberCompleteField ?? OrderFields.OrderNumberComplete;
                orderNumberSql = StringCondition.GenerateSql(StringValue, StringOperator, context.GetColumnReference(orderNumberCompleteField), context);
                
                CombinedOrderNumberCondition combinedOrderNumberCondition = new CombinedOrderNumberCondition(OrderSearchFields.OrderNumberComplete);
                combinedOrderNumberCondition.IsNumeric = IsNumeric;
                combinedOrderNumberCondition.StringOperator = StringOperator;
                combinedOrderNumberCondition.StringValue = StringValue;
                orderSearchSql = $" OR {combinedOrderNumberCondition.GenerateSql(context)}";

                AmazonCombinedOrderNumberCondition amazonCombinedOrderNumberCondition = new AmazonCombinedOrderNumberCondition(AmazonOrderSearchFields.OrderNumberComplete);
                amazonCombinedOrderNumberCondition.IsNumeric = IsNumeric;
                amazonCombinedOrderNumberCondition.StringOperator = StringOperator;
                amazonCombinedOrderNumberCondition.StringValue = StringValue;

                orderSearchSql = $" {orderSearchSql} OR {amazonCombinedOrderNumberCondition.GenerateSql(context)}";
            }

            return $"{orderNumberSql} {orderSearchSql}";
        }
    }
}
