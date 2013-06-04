using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;

namespace ShipWorks.Stores.Platforms.Newegg.CoreExtensions.Filters
{
    [ConditionElement("Newegg Invoice Number", "NeweggOrder.InvoiceNumber")]
    [ConditionStoreType(StoreTypeCode.NeweggMarketplace)]
    public class NeweggOrderInvoiceNumberCondition : StringCondition
    {
        /// <summary>
        /// Generate the filter SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            // First we have to get from Order -> NeweggOrder            
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, NeweggOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(GenerateSql(context.GetColumnReference(NeweggOrderFields.InvoiceNumber), context));
            }
        }
    }
}
