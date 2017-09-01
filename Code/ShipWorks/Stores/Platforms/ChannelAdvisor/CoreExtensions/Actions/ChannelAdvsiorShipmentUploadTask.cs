using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.ChannelAdvisor.OnlineUpdating;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.CoreExtensions.Actions
{
    /// <summary>
    /// Task for uploading shipment details to ChannelAdvisor
    /// </summary>
    [ActionTask("Upload shipment details", "ChannelAdvisorShipmentUploadTask", ActionTaskCategory.UpdateOnline)]
    public class ChannelAdvisorShipmentUploadTask : StoreInstanceTaskBase
    {
        private readonly IChannelAdvisorOnlineUpdater onlineUpdater;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelAdvisorShipmentUploadTask(IChannelAdvisorOnlineUpdater onlineUpdater)
        {
            this.onlineUpdater = onlineUpdater;
        }

        /// <summary>
        /// This task is for Orders
        /// </summary>
        public override EntityType? InputEntityType => EntityType.ShipmentEntity;

        /// <summary>
        /// Descriptive label which appears on the task editor
        /// </summary>
        public override string InputLabel => "Upload the tracking number for:";

        /// <summary>
        /// Indicates if the task is supported for the specified store
        /// </summary>
        public override bool SupportsStore(StoreEntity store) => store is ChannelAdvisorStoreEntity;

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
                foreach (long entityID in inputKeys)
                {
                    await onlineUpdater.UploadTrackingNumber(store, entityID).ConfigureAwait(false);
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
