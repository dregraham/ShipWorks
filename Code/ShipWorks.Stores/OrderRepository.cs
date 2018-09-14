using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Administration.Recovery;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Search;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Orders;

namespace ShipWorks.Stores
{
    /// <summary>
    /// Repository for saving order related content
    /// </summary>
    public class OrderRepository : IOrderRepository
    {
        private readonly ISqlSession sqlSession;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly ISqlAdapterRetryFactory sqlAdapterRetryFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderRepository"/> class.
        /// </summary>
        public OrderRepository(
            ISqlSession sqlSession,
            ISqlAdapterFactory sqlAdapterFactory, 
            ISqlAdapterRetryFactory sqlAdapterRetryFactory)
        {
            this.sqlSession = sqlSession;
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
