using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Stores.Platforms.ProStores.CoreExtensions.Filters
{
    [ConditionElement("ProStores Confirmation #", "ProStoresOrder.Confirmation")]
    [ConditionStoreType(StoreTypeCode.ProStores)]
    public class ProStoresConfirmationCondition : StringCondition
    {
        /// <summary>
        /// Generate the filter SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            // First we have to get from Order -> ProStoresOrder            
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, ProStoresOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(GenerateSql(context.GetColumnReference(ProStoresOrderFields.ConfirmationNumber), context));
            }
        }
    }
}
