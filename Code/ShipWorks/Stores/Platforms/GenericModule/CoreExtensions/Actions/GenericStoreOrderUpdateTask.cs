using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms.XCart;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Stores.Platforms;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks;
using Interapptive.Shared.Net;
using ShipWorks.Actions;

namespace ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Actions
{
    /// <summary>
    /// Task editor for updating a Generic Store's online order with a status code
    /// </summary>
    [ActionTask("Update store status", "GenericStoreOrderUpdate", ActionTaskCategory.UpdateOnline)]
    public class GenericStoreOrderUpdateTask : StoreInstanceTaskBase
    {
        string comment = "{//ServiceUsed} - {//TrackingNumber}";
        string statusCode = null;

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
        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        /// <summary>
        /// The status code the task will be run with
        /// </summary>
        public string StatusCode
        {
            get { return statusCode; }
            set { statusCode = value; }
        }   

        /// <summary>
        /// How to label input selection for the task
        /// </summary>
        public override string InputLabel
        {
            get
            {
                return "Set status of:";
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
        /// Insantiates the editor for this action
        /// </summary>
        public override ActionTaskEditor CreateEditor()
        {
            return new GenericStoreOrderUpdateTaskEditor(this);
        }

        /// <summary>
        /// Execute the status updates
        /// </summary>
        public override void Run(List<long> inputKeys, ActionStepContext context)
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
                    updater.UpdateOrderStatus(entityID, statusCodeProvider.ConvertCodeValue(StatusCode), Comment, context.CommitWork);
                }
            }
            catch (GenericStoreException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }
    }
}
