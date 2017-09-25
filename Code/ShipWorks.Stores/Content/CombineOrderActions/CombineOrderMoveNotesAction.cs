using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Stores.Content.CombineOrderActions
{
    /// <summary>
    /// Moves notes from orders to the surviving order
    /// </summary>
    public class CombineOrderMoveNotesAction : ICombineOrderAction
    {
        /// <summary>
        /// Perform moving the notes
        /// </summary>
        public async Task Perform(OrderEntity combinedOrder, long survivingOrderID, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            IRelationPredicateBucket notesBucket = new RelationPredicateBucket(NoteFields.EntityID.In(orders.Select(x => x.OrderID)));
            notesBucket.Relations.Add(NoteEntity.Relations.OrderEntityUsingEntityID);

            NoteEntity note = new NoteEntity
            {
                EntityID = combinedOrder.OrderID,
                Source = (int) NoteSource.CombinedOrder
            };

            // ExpressionToApply does NOT work with string interpolation, so do NOT change to use it.
            note.Fields[(int) NoteFieldIndex.Text].ExpressionToApply = "From Order " + OrderFields.OrderNumberComplete + ": " + NoteFields.Text;

            await sqlAdapter.UpdateEntitiesDirectlyAsync(note, notesBucket).ConfigureAwait(false);

            DynamicQuery noteCountQuery = new QueryFactory().Note
                .Select(NoteFields.NoteID.Count())
                .Where(NoteFields.EntityID == combinedOrder.OrderID);

            combinedOrder.RollupNoteCount = await sqlAdapter.FetchScalarAsync<int>(noteCountQuery).ConfigureAwait(false);
        }
    }
}