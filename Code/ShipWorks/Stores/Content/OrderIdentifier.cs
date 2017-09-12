using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// Base class used for identifying orders in a way that is unique to each store type.  For instance,
    /// osCommerce orders can be identified using OrderNumber, but Amazon orders are identifier using the
    /// AmazonOrderID.
    /// </summary>
    abstract public class OrderIdentifier
    {
        /// <summary>
        /// Apply the order identifier values to the order
        /// </summary>
        public abstract void ApplyTo(OrderEntity order);

        /// <summary>
        /// Apply the order identifier values to the download history entry
        /// </summary>
        public abstract void ApplyTo(DownloadDetailEntity downloadDetail);

        /// <summary>
        /// Create an entity query that can be used to retrieve the search record for a combined order
        /// </summary>
        public abstract QuerySpec CreateCombinedSearchQuery(QueryFactory factory);

        /// <summary>
        /// Value to use when auditing
        /// </summary>
        public abstract string AuditValue { get; }

        /// <summary>
        /// Create the combined search query
        /// </summary>
        protected QuerySpec CreateCombinedSearchQueryInternal<T>(QueryFactory factory,
            EntityQuery<T> entityQuery, EntityField2 originalOrderIDField, IPredicate predicate)
            where T : IEntityCore
        {
            var from = entityQuery
                .InnerJoin(factory.OrderSearch)
                .On(originalOrderIDField == OrderSearchFields.OriginalOrderID);

            return factory.Create()
                .From(from)
                .Select(originalOrderIDField)
                .Where(predicate);
        }
    }
}
