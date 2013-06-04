using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Stores.Platforms.Ebay.CoreExtensions.Filters
{
    [ConditionElement("eBay Buyer", "EbayOrder.EbayBuyerID")]
    [ConditionStoreType(StoreTypeCode.Ebay)]
    public class EbayBuyerIDCondition : StringCondition
    {
        /// <summary>
        /// Generate the filter SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            // First we have to get from Order -> EbayOrder            
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, EbayOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(GenerateSql(context.GetColumnReference(EbayOrderFields.EbayBuyerID), context));
            }
        }
    }
}
