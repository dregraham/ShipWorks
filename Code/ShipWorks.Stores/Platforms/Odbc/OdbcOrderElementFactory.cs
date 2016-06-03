using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Import;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content;
using System;
using System.Linq;

namespace ShipWorks.Stores.Platforms.Odbc
{
    public class OdbcOrderElementFactory : IOrderElementFactory
    {

        /// <summary>
        /// Create a new item attached to the order
        /// </summary>
        public OrderItemEntity CreateItem(OrderEntity order)
        {
            // If we generalize this for other stores, we will need to get
            // store specific OrderItemEntities... Downloader is calling StoreType.CreateOrderItemInstance
            OrderItemEntity item = new OrderItemEntity
            {
                Order = order,
                
                // Downloaded items are assumed not manual
                IsManual = false
            };

            // Initialize the rest of the fields
            item.InitializeNullsToDefault();

            return item;
        }

        /// <summary>
        /// Create a new item attribute attached to the order
        /// </summary>
        public OrderItemAttributeEntity CreateItemAttribute(OrderItemEntity item)
        {
            // If we generalize this for other stores, we will need to get
            // store specific OrderItemEntities... Downloader is calling StoreType.CreateItemAttributeInstance 
            return new OrderItemAttributeEntity
            {
                OrderItem = item,
                
                // downloaded attributes are not manual
                IsManual = false
            };
        }

        /// <summary>
        /// Create a new charge attached to the order
        /// </summary>
        public OrderChargeEntity CreateCharge(OrderEntity order) => new OrderChargeEntity {Order = order};

        /// <summary>
        /// Create a new note and attach it to the order.
        /// </summary>
        public NoteEntity CreateNote(OrderEntity order, string noteText, DateTime noteDate, NoteVisibility noteVisibility)
        {
            if (string.IsNullOrWhiteSpace(noteText))
            {
                return null;
            }

            noteText = noteText.Trim();

            
            // First see if any of the current (newly downloaded) notes match this note
            if (order.Notes.Any(n =>
                string.Compare(n.Text, noteText, StringComparison.OrdinalIgnoreCase) == 0
                && n.Source == (int)NoteSource.Downloaded))
            {
                return null;
            }

            if (HasMatchingNoteInDatabase(order, noteText))
            {
                return null;
            }

            // Instantiate the note
            NoteEntity note = new NoteEntity
            {
                Order = order,
                UserID = null,
                Edited = noteDate,
                Source = (int) NoteSource.Downloaded,
                Visibility = (int) noteVisibility,
                Text = noteText
            };

            return note;
        }

        /// <summary>
        /// Determines whether the specified order has matching note in database.
        /// </summary>
        private static bool HasMatchingNoteInDatabase(OrderEntity order, string noteText)
        {
            // If the order isn't new, check the ones in the database too
            if (!order.IsNew)
            {
                IRelationPredicateBucket relationPredicateBucket = order.GetRelationInfoNotes();
                relationPredicateBucket.PredicateExpression.AddWithAnd(new FieldCompareValuePredicate(NoteFields.Text,
                    null, ComparisonOperator.Equal, noteText));
                relationPredicateBucket.PredicateExpression.AddWithAnd(new FieldCompareValuePredicate(
                    NoteFields.Source, null, ComparisonOperator.Equal, (int)NoteSource.Downloaded));

                using (EntityCollection<NoteEntity> notes = new EntityCollection<NoteEntity>())
                {
                    int matchingNotes = SqlAdapter.Default.GetDbCount(notes, relationPredicateBucket);

                    if (matchingNotes > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Create a new payment detail attached to the order
        /// </summary>
        public OrderPaymentDetailEntity CreatePaymentDetail(OrderEntity order)
            => new OrderPaymentDetailEntity {Order = order};
    }
}
