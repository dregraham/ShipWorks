using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Stores.Platforms.Amazon.CoreExtensions.Filters
{
    /// <summary>
    /// Condition for the amazon fulfillment channel field
    /// </summary>
    [ConditionElement("Fulfilled By", "Amazon.FulfillmentChannel")]
    [ConditionStoreType(StoreTypeCode.Amazon)]
    public class AmazonFulfillmentChannelCondition : EnumCondition<AmazonMwsFulfillmentChannel>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonFulfillmentChannelCondition()
        {
            Value = AmazonMwsFulfillmentChannel.MFN;
        }

        /// <summary>
        /// Generate the SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            // We have to get from Order -> AmazonOrder
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, AmazonOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(base.GenerateSql(context.GetColumnReference(AmazonOrderFields.FulfillmentChannel), context));
            }
        }
    }
}
