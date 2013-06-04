using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Net;
using ShipWorks.Templates.Processing;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms;
using ShipWorks.Stores.Platforms.Newegg;
using log4net;
using ShipWorks.Shipping;

namespace ShipWorks.Stores.Platforms.Newegg.CoreExtensions.Actions
{
    /// <summary>
    /// A task to upload shipping details to Newegg.
    /// </summary>
    [ActionTask("Upload shipment details", "NeweggShipmentUploadTask")]
    public class NeweggShipmentUploadTask : StoreInstanceTaskBase
    {
        /// <summary>
        /// If the task operates on only one type of input, this specified what 
        /// type that is. This task is for shipments.
        /// </summary>
        public override EntityType? InputEntityType
        {
            get
            {
                return EntityType.ShipmentEntity;
            }
        }

        /// <summary>
        /// The label that goes before what the data source for the task should be.
        /// </summary>
        public override string InputLabel
        {
            get
            {
                return "Upload the tracking number for:";
            }
        }

        /// <summary>
        /// Creates the editor that is used to edit the task.
        /// </summary>
        /// <returns></returns>
        public override ActionTaskEditor CreateEditor()
        {
            return new BasicShipmentUploadTaskEditor();
        }

        /// <summary>
        /// Must be overridden to indicate if the task supports the specified store. This
        /// task only supports Newegg stores.
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        public override bool SupportsStore(StoreEntity store)
        {
            return store is NeweggStoreEntity;
        }

        /// <summary>
        /// Run the task for tasks that require input but don't need the context
        /// This should perform any long-running operations, but should NOT save to the database. This function is NOT within a transaction and should not be, as it's designed
        /// for doing things like printing and connecting to external websites, which take too much time to be within a transaction.
        /// </summary>
        /// <param name="inputKeys"></param>
        protected override void Run(List<long> inputKeys)
        {
            if (inputKeys == null)
            {
                throw new ArgumentNullException("inputKeys", "The inputKeys parameter value was null.");
            }

            if (StoreID <= 0)
            {
                throw new ActionTaskRunException("A store has not been configured for the task.");
            }

            NeweggStoreEntity store = StoreManager.GetStore(StoreID) as NeweggStoreEntity;
            if (store == null)
            {
                throw new ActionTaskRunException("The store configured for the task has been deleted.");
            }

            
            try
            {
                NeweggOnlineUpdater updater = new NeweggOnlineUpdater(store);
                foreach (long shipmentId in inputKeys)
                {
                    // We know we're uploading shipment data, so fetch the shipment from
                    // the ShippingManager and upload the details
                    ShipmentEntity shipment = ShippingManager.GetShipment(shipmentId);
                    updater.UploadShippingDetails(shipment);
                }
            }
            catch (NeweggException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }
    }
}
