using System;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Amazon.CoreExtensions.Filters;

namespace ShipWorks.Filters.Content.Conditions.Orders
{
    /// <summary>
    /// Condition base on the combined Order# of an Order
    /// </summary>
    [ConditionElement("Combined Order Number", "CombinedOrder.Number")]
    public class CombinedOrderNumberCondition : NumericStringCondition<long>
    {
        private readonly EntityField2 searchField;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="searchField">This is the field to search, either OrderNumber or OrderNumberComplete</param>
        public CombinedOrderNumberCondition(EntityField2 searchField)
        {
            this.searchField = searchField;
        }

        /// <summary>
        /// Generate the SQL that evaluates the condition
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            string orderSearchSql = String.Empty;

            // Add any combined order number entries.
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, OrderSearchFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                orderSearchSql = scope.Adorn(GenerateSql(context.GetColumnReference(searchField), context));
            }

            return orderSearchSql;
        }
    }
}
