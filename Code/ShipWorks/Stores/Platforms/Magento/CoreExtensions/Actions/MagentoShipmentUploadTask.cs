using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Magento;
using ShipWorks.Data.Model;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Net;
using ShipWorks.Templates.Processing;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Actions;

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

            return (genericStore.TypeCode == (int)StoreTypeCode.Magento);
        }

        /// <summary>
        /// Executes the task
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

            try
            {
                IMagentoOnlineUpdater updater = (IMagentoOnlineUpdater) new MagentoStoreType(store).CreateOnlineUpdater();

                foreach (long entityID in inputKeys)
                {
                    updater.UploadShipmentDetails(entityID, "complete", comment, magentoSendEmail, context.CommitWork);
                }
            }
            catch (MagentoException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
            catch (GenericStoreException ex)
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
