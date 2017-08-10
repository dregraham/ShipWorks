using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Stores.Platforms.ChannelAdvisor;
using ShipWorks.Data.Model;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Net;
using ShipWorks.Templates.Processing;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks.Common.Editors;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.CoreExtensions.Actions
{

    /// <summary>
    /// Task for uploading shipment detials to ChannelAdvisor
    /// </summary>
    [ActionTask("Upload shipment details", "ChannelAdvisorShipmentUploadTask", ActionTaskCategory.UpdateOnline)]
    public class ChannelAdvisorShipmentUploadTask : StoreInstanceTaskBase
    {
        /// <summary>
        /// This task is for Orders
        /// </summary>
        public override EntityType? InputEntityType
        {
            get
            {
                return EntityType.ShipmentEntity;
            }
        }

        /// <summary>
        /// Descriptive label which appears on the task editor
        /// </summary>
        public override string InputLabel
        {
            get
            {
                return "Upload the tracking number for:";
            }
        }

        /// <summary>
        /// Indicates if the task is supported for the specified store
        /// </summary>
        public override bool SupportsStore(StoreEntity store)
        {
            ChannelAdvisorStoreEntity genericStore = store as ChannelAdvisorStoreEntity;
            if (genericStore == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// This task should be run asynchronously.
        /// </summary>
        public override bool IsAsync => true;

        /// <summary>
        /// Executes the task
        /// </summary>
        protected override async Task RunAsync(List<long> inputKeys)
        {
            if (StoreID <= 0)
            {
                throw new ActionTaskRunException("A store has not been configured for the task.");
            }

            ChannelAdvisorStoreEntity store = StoreManager.GetStore(StoreID) as ChannelAdvisorStoreEntity;
            if (store == null)
            {
                throw new ActionTaskRunException("The store configured for the task has been deleted.");
            }

            try
            {
                ChannelAdvisorOnlineUpdater updater = new ChannelAdvisorOnlineUpdater(store);
                foreach (long entityID in inputKeys)
                {
                    await updater.UploadTrackingNumber(entityID).ConfigureAwait(false);
                }
            }
            catch (ChannelAdvisorException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Instantiates the editor for the action
        /// </summary>
        public override ActionTaskEditor CreateEditor()
        {
            return new BasicShipmentUploadTaskEditor(); 
        }
    }
}
