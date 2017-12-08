using System.Collections.Generic;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.Conditions.QuickSearch;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Stores.Platforms.CommerceInterface.Content
{
    /// <summary>
    /// Generate SQL lines for a quick search for given store.
    /// </summary>
    public class CommerceInterfaceQuickSearchSql : IQuickSearchStoreSql
    {
        /// <summary>
        /// Store type supported by this IQuickSearchStoreSql
        /// </summary>
        public StoreTypeCode StoreType => StoreTypeCode.CommerceInterface;

        /// <summary>
        /// Generate SQL lines for a quick search for given store.
        /// The result of each line must ONLY be OrderId
        /// </summary>
        public IEnumerable<string> GenerateSql(ISqlGenerationBuilder context, string searchText)
        {
            var customOrderIdentifierParam = context.RegisterParameter(CommerceInterfaceOrderFields.CommerceInterfaceOrderNumber, searchText);

            return new[]
            {
                $"SELECT OrderId FROM [CommerceInterfaceOrder] WHERE {CommerceInterfaceOrderFields.CommerceInterfaceOrderNumber.Name} LIKE {customOrderIdentifierParam}",
                $"SELECT OrderId FROM [CommerceInterfaceOrderSearch] WHERE {CommerceInterfaceOrderSearchFields.CommerceInterfaceOrderNumber.Name} LIKE {customOrderIdentifierParam}",
            };
        }
    }
}
