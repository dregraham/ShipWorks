using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Actions;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Actions
{
    /// <summary>
    /// Task editor for updating a Generic Store's online order with a status code
    /// </summary>
    [ActionTask("Update store status", "GenericStoreOrderUpdate", ActionTaskCategory.UpdateOnline)]
    public class GenericStoreOrderUpdateTask : StoreInstanceTaskBase
    {
        /// <summary>
        /// Indicates if the task is supported for the specified store
        /// </summary>
        public override bool SupportsStore(StoreEntity store)
        {
            GenericModuleStoreEntity genericStore = store as GenericModuleStoreEntity;
            if (genericStore == null)
            {
                return false;
            }

            return genericStore.ModuleOnlineStatusSupport != (int) GenericOnlineStatusSupport.None &&
                    genericStore.ModuleOnlineStatusSupport != (int) GenericOnlineStatusSupport.DownloadOnly;
        }

        /// <summary>
        /// The comment to upload with the status
        /// </summary>
        public string Comment { get; set; } = "{//ServiceUsed} - {//TrackingNumber}";

        /// <summary>
        /// The status code the task will be run with
        /// </summary>
        public string StatusCode { get; set; }

        /// <summary>
        /// How to label input selection for the task
        /// </summary>
        public override string InputLabel => "Set status of:";

        /// <summary>
        /// This task only operates on orders
        /// </summary>
        public override EntityType? InputEntityType => EntityType.OrderEntity;

        /// <summary>
        /// Should the action be run async
        /// </summary>
        public override bool IsAsync => true;

        /// <summary>
        /// Instantiates the editor for this action
        /// </summary>
        public override ActionTaskEditor CreateEditor()
        {
            return new GenericStoreOrderUpdateTaskEditor(this);
        }

        /// <summary>
        /// Execute the status updates
        /// </summary>
        public override async Task RunAsync(List<long> inputKeys, IActionStepContext context)
        {
            if (StoreID <= 0)
            {
                throw new ActionTaskRunException("A store has not been configured for the task.");
            }

            GenericModuleStoreEntity store = StoreManager.GetStore(StoreID) as GenericModuleStoreEntity;
            if (store == null)
            {
                throw new ActionTaskRunException("The store configured for the task has been deleted.");
            }

            if (store.ModuleOnlineStatusSupport == (int) GenericOnlineStatusSupport.None ||
                store.ModuleOnlineStatusSupport == (int) GenericOnlineStatusSupport.DownloadOnly)
            {
                throw new ActionTaskRunException("The store no longer supports online status updates.");
            }

            if (StatusCode == null)
            {
                throw new ActionTaskRunException("A status code has not been selected.");
            }

            GenericModuleStoreType storeType = (GenericModuleStoreType) StoreTypeManager.GetType(store);
            GenericStoreStatusCodeProvider statusCodeProvider = storeType.CreateStatusCodeProvider();
            GenericStoreOnlineUpdater updater = storeType.CreateOnlineUpdater();

            try
            {
                foreach (long entityID in inputKeys)
                {
                    await updater.UpdateOrderStatus(entityID, statusCodeProvider.ConvertCodeValue(StatusCode), Comment, context.CommitWork)
                        .ConfigureAwait(false);
                }
            }
            catch (GenericStoreException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }
    }
}
