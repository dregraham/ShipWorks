using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using SD.LLBLGen.Pro.QuerySpec.Adapter;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Stores.Content.OrderCombinerActions
{
    public class MoveNotesAction : IOrderCombinerAction
    {
        public async Task Perform(OrderEntity combinedOrder, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            IRelationPredicateBucket notesBucket = new RelationPredicateBucket(NoteFields.EntityID.In(orders.Select(x => x.OrderID)));
            await sqlAdapter.UpdateEntitiesDirectlyAsync(new NoteEntity { EntityID = combinedOrder.OrderID }, notesBucket);
            DynamicQuery noteCountQuery = new QueryFactory().Note
                .Select(NoteFields.NoteID.Count())
                .Where(NoteFields.EntityID == combinedOrder.OrderID);

            combinedOrder.RollupNoteCount = await sqlAdapter.FetchScalarAsync<int>(noteCountQuery);
        }
    }
}
