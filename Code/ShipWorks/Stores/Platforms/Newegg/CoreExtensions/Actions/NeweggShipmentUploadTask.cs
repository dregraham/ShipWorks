using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Stores.Platforms.Newegg.OnlineUpdating;

namespace ShipWorks.Stores.Platforms.Newegg.CoreExtensions.Actions
{
    /// <summary>
    /// A task to upload shipping details to Newegg.
    /// </summary>
    [ActionTask("Upload shipment details", "NeweggShipmentUploadTask", ActionTaskCategory.UpdateOnline)]
    public class NeweggShipmentUploadTask : StoreInstanceTaskBase
    {
        private readonly IShipmentDetailsUpdater updater;
        private readonly IStoreManager storeManager;
        private readonly IShippingManager shippingManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public NeweggShipmentUploadTask(IShipmentDetailsUpdater updater, IStoreManager storeManager, IShippingManager shippingManager)
        {
            this.storeManager = storeManager;
            this.shippingManager = shippingManager;
            this.updater = updater;
        }

        /// <summary>
        /// Should the ActionTask be run async
        /// </summary>
        public override bool IsAsync => true;

        /// <summary>
        /// If the task operates on only one type of input, this specified what
        /// type that is. This task is for shipments.
        /// </summary>
        public override EntityType? InputEntityType => EntityType.ShipmentEntity;

        /// <summary>
        /// The label that goes before what the data source for the task should be.
        /// </summary>
        public override string InputLabel => "Upload the tracking number for:";

        /// <summary>
        /// Creates the editor that is used to edit the task.
        /// </summary>
        /// <returns></returns>
        public override ActionTaskEditor CreateEditor() =>
            new BasicShipmentUploadTaskEditor();

        /// <summary>
        /// Must be overridden to indicate if the task supports the specified store. This
        /// task only supports Newegg stores.
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        public override bool SupportsStore(StoreEntity store) =>
            store is NeweggStoreEntity;

        /// <summary>
        /// Run the task for tasks that require input but don't need the context
        /// This should perform any long-running operations, but should NOT save to the database. This function is NOT within a transaction and should not be, as it's designed
        /// for doing things like printing and connecting to external websites, which take too much time to be within a transaction.
        /// </summary>
        /// <param name="inputKeys"></param>
        protected override async Task RunAsync(List<long> inputKeys)
        {
            if (inputKeys == null)
            {
                throw new ArgumentNullException("inputKeys", "The inputKeys parameter value was null.");
            }

            if (StoreID <= 0)
            {
                throw new ActionTaskRunException("A store has not been configured for the task.");
            }

            INeweggStoreEntity store = storeManager.GetStore(StoreID) as INeweggStoreEntity;
            if (store == null)
            {
                throw new ActionTaskRunException("The store configured for the task has been deleted.");
            }

            try
            {
                foreach (long shipmentId in inputKeys)
                {
                    // We know we're uploading shipment data, so fetch the shipment from
                    // the ShippingManager and upload the details
                    var shipmentAdapter = await shippingManager.GetShipmentAsync(shipmentId).ConfigureAwait(false);
                    await updater.UploadShippingDetails(store, shipmentAdapter.Shipment).ConfigureAwait(false);
                }
            }
            catch (NeweggException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }
    }
}
