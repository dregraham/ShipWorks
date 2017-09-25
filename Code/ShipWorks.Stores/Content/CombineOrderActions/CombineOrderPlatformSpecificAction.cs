using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Content.CombineOrderActions
{
    /// <summary>
    /// Perform store specific order combination
    /// </summary>
    public class PlatformSpecificAction : ICombineOrderAction
    {
        private readonly IStoreManager storeManager;
        private readonly IIndex<StoreTypeCode, IStoreSpecificCombineOrderAction> storeSpecificOrderCombiner;

        /// <summary>
        /// Constructor
        /// </summary>
        public PlatformSpecificAction(IStoreManager storeManager, IIndex<StoreTypeCode, IStoreSpecificCombineOrderAction> storeSpecificOrderCombiner)
        {
            this.storeSpecificOrderCombiner = storeSpecificOrderCombiner;
            this.storeManager = storeManager;
        }

        /// <summary>
        /// Perform the Action
        /// </summary>
        public Task Perform(OrderEntity combinedOrder, long survivingOrderID, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            IStoreEntity store = storeManager.GetStore(combinedOrder.StoreID);

            IStoreSpecificCombineOrderAction platformCombiner;

            return storeSpecificOrderCombiner.TryGetValue((StoreTypeCode) store.TypeCode, out platformCombiner) ?
                platformCombiner.Perform(combinedOrder, orders, sqlAdapter) :
                Task.FromResult(Unit.Default);
        }
    }
}
