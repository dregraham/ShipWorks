using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Actions;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.CoreExtensions.Actions
{
    /// <summary>
    /// Action task for uploading shipment details to Yahoo
    /// </summary>
    [ActionTask("Upload shipment details", "YahooApiShipmentUploadTask", ActionTaskCategory.UpdateOnline)]
    public class YahooApiShipmentUploadTask : StoreInstanceTaskBase
    {
        /// <summary>
        /// The maximum batch size
        /// </summary>
        private const long maxBatchSize = 1000;

        /// <summary>
        /// This task is for Shipments
        /// </summary>
        public override EntityType? InputEntityType => EntityType.ShipmentEntity;

        /// <summary>
        /// Instantiates the editor for the action
        /// </summary>
        /// <returns></returns>
        public override ActionTaskEditor CreateEditor() => new BasicShipmentUploadTaskEditor();

        /// <summary>
        ///     Descriptive label which appears on the task editor
        /// </summary>
        public override string InputLabel => "Upload the tracking number for:";

        /// <summary>
        /// Indicates if the task is supported for the specified store
        /// </summary>
        /// <param name="store">The store.</param>
        /// <returns></returns>
        public override bool SupportsStore(StoreEntity store) => store is YahooStoreEntity;

        /// <summary>
        /// Run the task
        /// </summary>
        /// <param name="inputKeys">The input keys.</param>
        /// <param name="context">The context.</param>
        public override void Run(List<long> inputKeys, ActionStepContext context)
        {
            if (inputKeys == null)
            {
                throw new ArgumentNullException("inputKeys", "The inputKeys parameter value was null.");
            }

            if (StoreID <= 0)
            {
                throw new ActionTaskRunException("A store has not been configured for the task.");
            }

            YahooStoreEntity store = StoreManager.GetStore(StoreID) as YahooStoreEntity;

            if (store == null)
            {
                throw new ActionTaskRunException("The store configured for the task has been deleted.");
            }

            // Get any postponed data we've previously stored away
            List<long> postponedKeys = context.GetPostponedData().SelectMany(d => (List<long>)d).ToList();

            // To avoid postponing forever on big selections, we only postpone up to maxBatchSize
            if (context.CanPostpone && postponedKeys.Count < maxBatchSize)
            {
                context.Postpone(inputKeys);
            }
            else
            {
                context.ConsumingPostponed();

                // Upload the details, first starting with all the postponed input, plus the current input
                UploadShipmentDetails(store, postponedKeys.Concat(inputKeys));
            }
        }

        /// <summary>
        /// Run the batched up (already combined from postponed tasks, if any) input keys through the task
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="shipmentKeys">The shipment keys.</param>
        private void UploadShipmentDetails(YahooStoreEntity store, IEnumerable<long> shipmentKeys)
        {
            try
            {
                YahooApiOnlineUpdater updater = new YahooApiOnlineUpdater(store);

                foreach (long shipmentKey in shipmentKeys)
                {
                    ShipmentEntity shipment = ShippingManager.GetShipment(shipmentKey);

                    OrderEntity order = shipment.Order;

                    if (order == null)
                    {
                        throw new ActionTaskRunException($"Error getting the order for shipment ID: {shipmentKey}");
                    }

                    if (!order.IsManual)
                    {
                        updater.UpdateShipmentDetails(shipment);
                    }
                }
            }
            catch (YahooException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }
    }
}
