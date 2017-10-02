using System.Collections.Generic;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.MarketplaceAdvisor.AppDomainHelpers;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor.CoreExtensions.Actions
{
    /// <summary>
    /// Task for promoting an online MarketplaceAdvisor parcel
    /// </summary>
    /// <remarks>
    /// THIS STORE IS DEAD
    /// This store is scheduled for removal as it no longer exists. Do not update this store when making
    /// all-platform changes.
    /// </remarks>
    [ActionTask("Promote parcel", "MarketplaceAdvisorPromoteParcel", ActionTaskCategory.UpdateOnline)]
    public class MarketplaceAdvisorPromoteParcelTask : StoreInstanceTaskBase
    {
        /// <summary>
        /// Indiciates if the given store is supported for the task
        /// </summary>
        public override bool SupportsStore(StoreEntity store)
        {
            MarketplaceAdvisorStoreEntity mwStore = store as MarketplaceAdvisorStoreEntity;
            if (mwStore == null)
            {
                return false;
            }

            return mwStore.AccountType == (int) MarketplaceAdvisorAccountType.OMS;
        }

        /// <summary>
        /// Descriptive label which appears on the task editor
        /// </summary>
        public override string InputLabel
        {
            get
            {
                return "Promote:";
            }
        }

        /// <summary>
        /// This task only operates on orders
        /// </summary>
        public override EntityType? InputEntityType
        {
            get
            {
                return EntityType.OrderEntity;
            }
        }

        /// <summary>
        /// Create the UI editor for the task
        /// </summary>
        public override ActionTaskEditor CreateEditor()
        {
            return new MarketplaceAdvisorPromoteTaskEditor("parcel");
        }

        /// <summary>
        /// Run the task on the given set of keys
        /// </summary>
        protected override void Run(List<long> inputKeys)
        {
            MarketplaceAdvisorStoreEntity storeEntity = StoreManager.GetStore(StoreID) as MarketplaceAdvisorStoreEntity;
            if (storeEntity == null)
            {
                // The store went away
                return;
            }

            foreach (long entityID in inputKeys)
            {
                MarketplaceAdvisorOrderEntity order = DataProvider.GetEntity(entityID) as MarketplaceAdvisorOrderEntity;
                if (order == null)
                {
                    // the order was deleted or manual
                    continue;
                }

                try
                {
                    MarketplaceAdvisorOmsClient.Create(storeEntity).PromoteParcel(new MarketplaceAdvisorOrderDto(order));
                }
                catch (MarketplaceAdvisorException ex)
                {
                    throw new ActionTaskRunException(ex.Message, ex);
                }
            }
        }
    }
}
