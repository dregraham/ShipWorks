using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Users.Security;
using ShipWorks.Users;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// All note creations and deletions should go through the note gateway
    /// </summary>
    public static class NoteManager
    {
        /// <summary>
        /// Save the given note.
        /// </summary>
        public static void SaveNote(NoteEntity note)
        {
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                if (DataProvider.GetEntity(note.ObjectID) == null)
                {
                    throw new SqlForeignKeyException("The related entity has been deleted.");
                }

                // If its new, we have to increment reference counts
                if (note.IsNew)
                {
                    AdjustNoteCount(adapter, note.ObjectID, 1);
                }

                adapter.SaveAndRefetch(note);

                adapter.Commit();
            }
        }

        /// <summary>
        /// Delete the note with the specified ID.
        /// </summary>
        public static void DeleteNote(long noteID)
        {
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                NoteEntity note = (NoteEntity) DataProvider.GetEntity(noteID);
                if (note != null)
                {
                    AdjustNoteCount(adapter, note.ObjectID, -1);

                    // We do it this way since it was from the cache
                    adapter.DeleteEntity(new NoteEntity(note.NoteID));

                    adapter.Commit();
                }
            }
        }

        /// <summary>
        /// Delete all notes used by the given entity
        /// </summary>
        public static void DeleteNotesForDeletedEntity(long entityID, SqlAdapter adapter)
        {
            EntityType relatedType = EntityUtility.GetEntityType(entityID);

            // Note is for a customer. Don't have to adjust counts for customer - b\c the customer and orders will be deleted anyway.
            if (relatedType == EntityType.CustomerEntity)
            {
                // Delete all the notes directly related to this entity
                adapter.DeleteEntitiesDirectly(typeof(NoteEntity), new RelationPredicateBucket(NoteFields.ObjectID == entityID));

                RelationPredicateBucket customersOrders = new RelationPredicateBucket(
                    new FieldCompareSetPredicate(NoteFields.ObjectID, null, OrderFields.OrderID, null, SetOperator.In, OrderFields.CustomerID == entityID, false));

                // For customer's, we also have to delete all the order notes
                adapter.DeleteEntitiesDirectly(typeof(NoteEntity), customersOrders);
            }

            // Note is for order
            if (relatedType == EntityType.OrderEntity)
            {
                // Delete all the notes directly related to this entity
                int deleted = adapter.DeleteEntitiesDirectly(typeof(NoteEntity), new RelationPredicateBucket(NoteFields.ObjectID == entityID));

                CustomerEntity customer = new CustomerEntity();
                customer.Fields[(int) CustomerFieldIndex.RollupNoteCount].ExpressionToApply = CustomerFields.RollupNoteCount - deleted;

                // Adjust the count of the customer
                RelationPredicateBucket customerBucket = new RelationPredicateBucket(OrderFields.OrderID == entityID);
                customerBucket.Relations.Add(OrderEntity.Relations.CustomerEntityUsingCustomerID);
                adapter.UpdateEntitiesDirectly(customer, customerBucket);
            }
        }

        /// <summary>
        /// Delete all notes for every customer that matches the given predicate bucket, for when a store is deleted
        /// </summary>
        public static void DeleteNotesForDeletedStore(RelationPredicateBucket customerBucket, SqlAdapter adapter)
        {
            // Delete all notes for the customers
            adapter.DeleteEntitiesDirectly(typeof(NoteEntity), 
                new RelationPredicateBucket(
                    new FieldCompareSetPredicate(NoteFields.ObjectID, null, CustomerFields.CustomerID, 
                        null, SetOperator.In, customerBucket.PredicateExpression, false)));

            // Delete all notes for the orders
            adapter.DeleteEntitiesDirectly(typeof(NoteEntity),
                new RelationPredicateBucket(
                    new FieldCompareSetPredicate(NoteFields.ObjectID, null, OrderFields.OrderID,
                        null, SetOperator.In, customerBucket.PredicateExpression, new RelationCollection(CustomerEntity.Relations.OrderEntityUsingCustomerID), false)));
        }

        /// <summary>
        /// Adjust the note count for the given object (and related objects) by the given amount
        /// </summary>
        public static void AdjustNoteCount(SqlAdapter adapter, long objectID, int amount)
        {
            if (amount == 0)
            {
                return;
            }

            EntityType relatedType = EntityUtility.GetEntityType(objectID);

            // Note is for a customer
            if (relatedType == EntityType.CustomerEntity)
            {
                CustomerEntity customer = new CustomerEntity();
                customer.Fields[(int) CustomerFieldIndex.RollupNoteCount].ExpressionToApply = CustomerFields.RollupNoteCount + amount;

                adapter.UpdateEntitiesDirectly(customer, new RelationPredicateBucket(CustomerFields.CustomerID == objectID));

                OrderEntity order = new OrderEntity();
                order.Fields[(int) OrderFieldIndex.RollupNoteCount].ExpressionToApply = OrderFields.RollupNoteCount + amount;

                adapter.UpdateEntitiesDirectly(order, new RelationPredicateBucket(OrderFields.CustomerID == objectID));
            }

            // Note is for an order
            else if (relatedType == EntityType.OrderEntity)
            {
                OrderEntity order = new OrderEntity();
                order.Fields[(int) OrderFieldIndex.RollupNoteCount].ExpressionToApply = OrderFields.RollupNoteCount + amount;

                adapter.UpdateEntitiesDirectly(order, new RelationPredicateBucket(OrderFields.OrderID == objectID));

                CustomerEntity customer = new CustomerEntity();
                customer.Fields[(int) CustomerFieldIndex.RollupNoteCount].ExpressionToApply = CustomerFields.RollupNoteCount + amount;

                RelationPredicateBucket customerBucket = new RelationPredicateBucket(OrderFields.OrderID == objectID);
                customerBucket.Relations.Add(OrderEntity.Relations.CustomerEntityUsingCustomerID);
                adapter.UpdateEntitiesDirectly(customer, customerBucket);
            }

            else
            {
                // If you need to add a new target type, make sure to update the above rollup\rolldown queries, as well as
                // deletion code that needs to tell the note gateway that notes need deleted for the new target type.
                throw new InvalidOperationException("Invalid target object type for note.");
            }
        }

        /// <summary>
        /// Get a query that can be used to retrieve all notes for the given entity
        /// </summary>
        public static RelationPredicateBucket GetNotesQuery(long entityID)
        {
            EntityType entityType = EntityUtility.GetEntityType(entityID);

            RelationPredicateBucket bucket = new RelationPredicateBucket();
            bucket.PredicateExpression.Add(NoteFields.ObjectID == entityID);

            // For orders, add in the related customer
            if (entityType == EntityType.OrderEntity)
            {
                FieldCompareSetPredicate predicate = new FieldCompareSetPredicate(
                    NoteFields.ObjectID, null, OrderFields.CustomerID, null, SetOperator.In, OrderFields.OrderID == entityID);

                bucket.PredicateExpression.AddWithOr(predicate);
            }

            // For customers, add in the related orders
            else if (entityType == EntityType.CustomerEntity)
            {
                FieldCompareSetPredicate predicate = new FieldCompareSetPredicate(
                    NoteFields.ObjectID, null, OrderFields.OrderID, null, SetOperator.In, OrderFields.CustomerID == entityID);

                bucket.PredicateExpression.AddWithOr(predicate);
            }
            else
            {
                throw new InvalidOperationException("Unhandled EntityType in NoteControl. " + entityID);
            }

            return bucket;
        }
    }
}
