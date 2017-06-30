using System;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Stores.Platforms.Amazon.CoreExtensions.Filters
{
    /// <summary>
    /// Condition for the Amazon order numbers that were combined into a single order
    /// </summary>
    [ConditionElement("Amazon Order #", "Amazon.CombinedOrderNumbers")]
    [ConditionStoreType(StoreTypeCode.Amazon)]
    public class AmazonCombinedOrderNumberCondition : NumericStringCondition<long>
    {
        private readonly EntityField2 searchField;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="searchField">This is the field to search, either OrderNumber or OrderNumberComplete</param>
        public AmazonCombinedOrderNumberCondition(EntityField2 searchField)
        {
            this.searchField = searchField;
        }

        /// <summary>
        /// Generate the filter SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            string amazonOrderSearchSql = String.Empty;

            // Add any combined order AmazonOrderID entries.
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, AmazonOrderSearchFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                amazonOrderSearchSql = scope.Adorn(GenerateSql(context.GetColumnReference(searchField), context));
            }

            return amazonOrderSearchSql;
        }
    }
}
