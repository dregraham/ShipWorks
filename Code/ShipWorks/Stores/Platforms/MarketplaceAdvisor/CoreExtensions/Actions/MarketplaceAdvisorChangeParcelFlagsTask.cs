using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms;
using ShipWorks.Stores;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.MarketplaceAdvisor;
using ShipWorks.Shipping;
using ShipWorks.Data;
using ShipWorks.Actions.Tasks;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor.CoreExtensions.Actions
{
    /// <summary>
    /// Task to change the flags of MarketplaceAdvisor parcel
    /// </summary>
    [ActionTask("Change parcel flags", "MarketplaceAdvisorChangeParcelFlags")]
    public class MarketplaceAdvisorChangeParcelFlagsTask : MarketplaceAdvisorChangeFlagsTaskBase
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
                return "Change flags of:";
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
            return new MarketplaceAdvisorChangeFlagsEditor(this);
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
                    MarketplaceAdvisorOmsClient.ChangeParcelFlags(
                        storeEntity, 
                        order,
                        FlagsOn,
                        FlagsOff);
                }
                catch (MarketplaceAdvisorException ex)
                {
                    throw new ActionTaskRunException(ex.Message, ex);
                }
            }
        }
    }
}
