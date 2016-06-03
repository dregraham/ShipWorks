using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores
{
    public class OrderRepository : IOrderRepository
    {
        /// <summary>
        /// Determines whether the specified order has matching note in database.
        /// </summary>
        public bool ContainsNote(OrderEntity order, string noteText, NoteSource source)
        {
            // If the order isn't new, check the ones in the database too
            if (!order.IsNew)
            {
                IRelationPredicateBucket relationPredicateBucket = order.GetRelationInfoNotes();
                relationPredicateBucket.PredicateExpression.AddWithAnd(new FieldCompareValuePredicate(NoteFields.Text,
                    null, ComparisonOperator.Equal, noteText));
                relationPredicateBucket.PredicateExpression.AddWithAnd(new FieldCompareValuePredicate(
                    NoteFields.Source, null, ComparisonOperator.Equal, (int)source));

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
    }
}
