using System.Collections.Generic;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor.CoreExtensions.Actions
{
    /// <summary>
    /// Task to update the online shipment record of a MarketplaceAdvisor order
    /// </summary>
    /// <remarks>
    /// THIS STORE IS DEAD
    /// This store is scheduled for removal as it no longer exists. Do not update this store when making
    /// all-platform changes.
    /// </remarks>
    [ActionTask("Upload shipment details", "MarketplaceAdvisorShipmentUpload", ActionTaskCategory.UpdateOnline)]
    public class MarketplaceAdvisorShipmentUploadTask : StoreInstanceTaskBase
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
                return "Update shipment details of:";
            }
        }

        /// <summary>
        /// This task only operates on shipments
        /// </summary>
        public override EntityType? InputEntityType
        {
            get
            {
                return EntityType.ShipmentEntity;
            }
        }

        /// <summary>
        /// Create the UI editor for the task
        /// </summary>
        public override ActionTaskEditor CreateEditor()
        {
            return new BasicShipmentUploadTaskEditor();
        }

        /// <summary>
        /// Run the task on the given keys
        /// </summary>
        protected override void Run(List<long> inputKeys)
        {
            MarketplaceAdvisorStoreEntity storeEntity = StoreManager.GetStore(StoreID) as MarketplaceAdvisorStoreEntity;
            if (storeEntity == null)
            {
                return;
            }

            try
            {
                foreach (long entityID in inputKeys)
                {
                    ShipmentEntity shipment = ShippingManager.GetShipment(entityID);
                    if (shipment == null)
                    {
                        // the shipment was deleted
                        continue;
                    }

                    MarketplaceAdvisorOnlineUpdater updater = new MarketplaceAdvisorOnlineUpdater(storeEntity);
                    updater.UpdateShipmentStatus(shipment);
                }
            }
            catch (MarketplaceAdvisorException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }
    }
}
