﻿using System.Collections.Generic;
using Interapptive.Shared.Extensions;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.Conditions.QuickSearch;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Stores.Platforms.Ebay.Content
{
    /// <summary>
    /// Generate SQL lines for a quick search for given store.
    /// </summary>
    public class EbayQuickSearchSql : IQuickSearchStoreSql
    {
        /// <summary>
        /// Store type supported by this IQuickSearchStoreSql
        /// </summary>
        public StoreTypeCode StoreType => StoreTypeCode.Ebay;

        /// <summary>
        /// Generate SQL lines for a quick search for given store.
        /// </summary>
        public IEnumerable<string> GenerateSql(ISqlGenerationBuilder context, string searchText)
        {
            bool isNumeric = searchText.Replace("%", string.Empty).IsNumeric();

            string buyerIdParamName = context.RegisterParameter(EbayOrderFields.EbayBuyerID, searchText);
            string orderIdParamName = context.RegisterParameter(EbayOrderFields.EbayOrderID, searchText);
            string orderSellingMgrRecordParamName = context.RegisterParameter(EbayOrderFields.SellingManagerRecord, searchText);
            string orderItemCodeParamName = context.RegisterParameter(OrderItemFields.Code, searchText);
            string extendedOrderIdParam = context.RegisterParameter(EbayOrderFields.ExtendedOrderID, searchText);

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
                selectStatements.Add($"SELECT OrderId FROM [EbayOrder] WHERE EbayOrderID LIKE {orderIdParamName} OR SellingManagerRecord LIKE {orderSellingMgrRecordParamName}");
                selectStatements.Add($"SELECT OrderId FROM [EbayOrderSearch] WHERE EbayOrderID LIKE {orderIdParamName} OR SellingManagerRecord LIKE {orderSellingMgrRecordParamName}");
                selectStatements.Add($"SELECT OrderID FROM [EbayOrderItem] WHERE SellingManagerRecord LIKE {orderSellingMgrRecordParamName}");
            }
            else
            {
                selectStatements.Add($"SELECT OrderId FROM [EbayOrder] WHERE EbayBuyerID LIKE {buyerIdParamName}");
                selectStatements.Add($"SELECT OrderId FROM [EbayOrderSearch] WHERE EbayBuyerID LIKE {buyerIdParamName}");
                selectStatements.Add($"SELECT OrderId FROM [EbayOrder] WHERE EbayOrderID LIKE {orderIdParamName}");
                selectStatements.Add($"SELECT OrderId FROM [EbayOrderSearch] WHERE EbayOrderID LIKE {orderIdParamName}");
                selectStatements.Add($"SELECT OrderId FROM [EbayOrder] WHERE ExtendedOrderID LIKE {extendedOrderIdParam}");
                selectStatements.Add($"SELECT OrderId FROM [EbayOrderSearch] WHERE ExtendedOrderID LIKE {extendedOrderIdParam}");
            }

            return selectStatements;
        }
    }
}