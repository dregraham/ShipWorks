using System.Data.SqlClient;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores
{
    /// <summary>
    /// Repository for saving order related content
    /// </summary>
    public class OrderRepository : IOrderRepository
    {
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly ISqlAdapterRetryFactory sqlAdapterRetryFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderRepository"/> class.
        /// </summary>
        public OrderRepository(ISqlAdapterFactory sqlAdapterFactory, ISqlAdapterRetryFactory sqlAdapterRetryFactory)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.sqlAdapterRetryFactory = sqlAdapterRetryFactory;
        }

        /// <summary>
        /// Determines whether the specified order has matching note in database.
        /// </summary>
        public bool ContainsNote(OrderEntity order, string noteText, NoteSource source)
        {
            IRelationPredicateBucket relationPredicateBucket = order.GetRelationInfoNotes();
            relationPredicateBucket.PredicateExpression.AddWithAnd(new FieldCompareValuePredicate(NoteFields.Text,
                null, ComparisonOperator.Equal, noteText));
            relationPredicateBucket.PredicateExpression.AddWithAnd(new FieldCompareValuePredicate(
                NoteFields.Source, null, ComparisonOperator.Equal, (int) source));

            using (EntityCollection<NoteEntity> notes = new EntityCollection<NoteEntity>())
            {
                int matchingNotes = SqlAdapter.Default.GetDbCount(notes, relationPredicateBucket);

                if (matchingNotes > 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Populates the order, order items, order charges, and order item attribute for the given order.
        /// </summary>
        /// <param name="order">The order.</param>
        public void PopulateOrderDetails(OrderEntity order) => OrderUtility.PopulateOrderDetails(order);

        /// <summary>
        /// Saves an order to the database
        /// </summary>
        public void Save(OrderEntity order)
        {
            ISqlAdapterRetry sqlAdapterRetry = sqlAdapterRetryFactory.Create<SqlException>(5, 0,
                "OrderRepository.PopulateOrderDetails");
            sqlAdapterRetry.ExecuteWithRetry(() => InternalSave(order));
        }

        /// <summary>
        /// Actual save command
        /// </summary>
        private void InternalSave(OrderEntity order)
        {
            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                sqlAdapter.SaveAndRefetch(order);
                sqlAdapter.Commit();
            }
        }
    }
}
