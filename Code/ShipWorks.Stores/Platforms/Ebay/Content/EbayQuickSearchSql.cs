using System.Collections.Generic;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;
using Interapptive.Shared.Extensions;
using ShipWorks.Filters.Content.Conditions.QuickSearch;

namespace ShipWorks.Stores.Platforms.Ebay.Content
{
    /// <summary>
    /// Generate SQL lines for a quick search for given store.
    /// </summary>
    public class EbayQuickSearchSql : IQuickSearchStoreSql
    {
        /// <summary>
        /// Generate SQL lines for a quick search for given store.
        /// </summary>
        public IEnumerable<string> GenerateSql(SqlGenerationContext context, string searchText)
        {
            bool isNumeric = searchText.Replace("%", string.Empty).IsNumeric();

            context.ColumnsUsed.Add(EbayOrderFields.EbayBuyerID);
            context.RegisterParameter(searchText);
            string buyerIdParamName = $"@param{context.Parameters.Count}";

            context.ColumnsUsed.Add(EbayOrderFields.SellingManagerRecord);
            context.RegisterParameter(searchText);
            string orderSellingMgrRecordParamName = $"@param{context.Parameters.Count}";

            context.ColumnsUsed.Add(OrderItemFields.Code);
            context.RegisterParameter(searchText);
            string orderItemCodeParamName = $"@param{context.Parameters.Count}";

            List<string> selectStatements = new List<string>()
            {
                $"SELECT OrderID FROM [OrderItem] WHERE Code LIKE {orderItemCodeParamName}"
            };

            // If the search text is numeric, then we can compare int fields.
            // If it has alphas, then int will never match, so don't add them.
            if (isNumeric)
            {
                selectStatements.Add($"SELECT OrderId FROM [EbayOrder] WHERE EbayBuyerID LIKE {buyerIdParamName} OR SellingManagerRecord LIKE {orderSellingMgrRecordParamName}");
                selectStatements.Add($"SELECT OrderId FROM [EbayOrderSearch] WHERE EbayBuyerID LIKE {buyerIdParamName} OR SellingManagerRecord LIKE {orderSellingMgrRecordParamName}");
                selectStatements.Add($"SELECT OrderID FROM [EbayOrderItem] WHERE SellingManagerRecord LIKE {orderSellingMgrRecordParamName}");
            }
            else
            {
                selectStatements.Add($"SELECT OrderId FROM [EbayOrder] WHERE EbayBuyerID LIKE {buyerIdParamName}");
                selectStatements.Add($"SELECT OrderId FROM [EbayOrderSearch] WHERE EbayBuyerID LIKE {buyerIdParamName}");
            }

            return selectStatements;
        }
    }
}