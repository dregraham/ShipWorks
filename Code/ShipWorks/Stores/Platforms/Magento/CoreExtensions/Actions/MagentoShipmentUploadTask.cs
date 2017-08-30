using System;
using System.Collections.Generic;
using Autofac;
using ShipWorks.Actions;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Stores.Platforms.Magento.Enums;
using System.Threading.Tasks;

namespace ShipWorks.Stores.Platforms.Magento.CoreExtensions.Actions
{
    /// <summary>
    /// Task for uploading shipment detials to Magento
    /// </summary>
    [ActionTask("Upload shipment details", "MagentoShipmentUploadTask", ActionTaskCategory.UpdateOnline)]
    public class MagentoShipmentUploadTask : StoreInstanceTaskBase
    {
        // comments to be uploaded.
        string comment = "";

        // flag for magento to send completed emails
        bool magentoSendEmail = false;

        /// <summary>
        /// Comments to be uploaded with the tracking information
        /// </summary>
        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        /// <summary>
        /// Specifies if Magento should send customer emails when an order is Completed
        /// </summary>
        public bool MagentoSendEmail
        {
            get { return magentoSendEmail; }
            set { magentoSendEmail = value; }
        }

        /// <summary>
        /// This task is for Orders
        /// </summary>
        public override EntityType? InputEntityType
        {
            get
            {
                return EntityType.OrderEntity;
            }
        }

        /// <summary>
        /// Descriptive label which appears on the task editor
        /// </summary>
        public override string InputLabel
        {
            get
            {
                return "Complete order and upload tracking number for:";
            }
        }

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

            return (genericStore.TypeCode == (int) StoreTypeCode.Magento);
        }

        /// <summary>
        /// This task should be run asynchronously
        /// </summary>
        public override bool IsAsync => true;

        /// <summary>
        /// Executes the task
        /// </summary>
        public override async Task RunAsync(List<long> inputKeys, ActionStepContext context)
        {
            if (StoreID <= 0)
            {
                throw new ActionTaskRunException("A store has not been configured for the task.");
            }

            StoreEntity store = StoreManager.GetStore(StoreID);
            if (store == null)
            {
                throw new ActionTaskRunException("The store configured for the task has been deleted.");
            }

            try
            {
                using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                {
                    MagentoStoreType storeType = lifetimeScope.Resolve<MagentoStoreType>(TypedParameter.From(store));
                    IMagentoOnlineUpdater updater = (IMagentoOnlineUpdater) storeType.CreateOnlineUpdater();

                    foreach (long entityID in inputKeys)
                    {
                        await updater.UploadShipmentDetails(entityID, MagentoUploadCommand.Complete, comment, magentoSendEmail, context.CommitWork).ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex) when (ex is MagentoException || ex is GenericStoreException)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Instantiates the editor for the action
        /// </summary>
        public override ActionTaskEditor CreateEditor()
        {
            return new MagentoShipmentUploadTaskEditor(this);
        }
    }
}
