using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Stores.Platforms.Ebay.CoreExtensions.Filters
{
    [ConditionElement("eBay Guaranteed Delivery", "EbayOrder.GuaranteedDelivery")]
    [ConditionStoreType(StoreTypeCode.Ebay)]
    public class EbayGuaranteedDeliveryCondition : BooleanCondition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EbayGuaranteedDeliveryCondition"/> class.
        /// </summary>
        public EbayGuaranteedDeliveryCondition()
            : base("Yes", "No")
        { }

        /// <summary>
        /// Generate the SQL for the condition clement
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override string GenerateSql(SqlGenerationContext context)
        {
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, EbayOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(base.GenerateSql(context.GetColumnReference(EbayOrderFields.GuaranteedDelivery), context));
            }
        }
    }
}
