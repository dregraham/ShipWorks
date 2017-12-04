﻿using System.Collections.Generic;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.Conditions.QuickSearch;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Stores.Platforms.Amazon.Content
{
    /// <summary>
    /// Generate SQL lines for a quick search for given store.
    /// </summary>
    public class AmazonQuickSearchSql : IQuickSearchStoreSql
    {
        /// <summary>
        /// Generate SQL lines for a quick search for given store.
        /// The result of each line must ONLY be OrderId
        /// </summary>
        public IEnumerable<string> GenerateSql(ISqlGenerationContext context, string searchText)
        {
            context.AddColumnUsed(AmazonOrderFields.AmazonOrderID);
            string paramName = context.RegisterParameter(searchText);

            return new[]
            {
                $"SELECT OrderId FROM [AmazonOrder] WHERE AmazonOrderID LIKE {paramName}",
                $"SELECT OrderId FROM [AmazonOrderSearch] WHERE AmazonOrderID LIKE {paramName}"
            };
        }
    }
}