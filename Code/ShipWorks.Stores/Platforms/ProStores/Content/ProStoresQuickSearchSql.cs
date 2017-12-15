using System.Collections.Generic;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.Conditions.QuickSearch;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Stores.Platforms.ProStores.Content
{
    /// <summary>
    /// Generate SQL lines for a quick search for ProStores
    /// </summary>
    public class ProStoresQuickSearchSql : IQuickSearchStoreSql
    {
        /// <summary>
        /// Store type supported by this IQuickSearchStoreSql
        /// </summary>
        public StoreTypeCode StoreType => StoreTypeCode.ProStores;

        /// <summary>
        /// Generate SQL lines for a quick search for ProStores
        /// </summary>
        public IEnumerable<string> GenerateSql(ISqlGenerationBuilder context, string searchText)
        {
            var confirmationNumberParam = context.RegisterParameter(ProStoresOrderFields.ConfirmationNumber, searchText);

            return new[]
            {
                $"SELECT OrderId FROM [ProStoresOrder] WHERE {ProStoresOrderFields.ConfirmationNumber.Name} LIKE {confirmationNumberParam}",
                $"SELECT OrderId FROM [ProStoresOrderSearch] WHERE {ProStoresOrderFields.ConfirmationNumber.Name} LIKE {confirmationNumberParam}"
            };
        }
    }
}
