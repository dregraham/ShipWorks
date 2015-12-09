using System;
using System.Collections.Generic;
using System.Linq;
using Quartz.Util;
using ShipWorks.Actions.Tasks;
using ShipWorks.Data.Model;
using ShipWorks.Actions;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration;
using ShipWorks.Stores.Platforms.Yahoo.EmailIntegration;

namespace ShipWorks.Stores.Platforms.Yahoo.CoreExtensions.Actions
{
    /// <summary>
    /// Task for uploading shipment details to a yahoo Store
    /// </summary>
    [ActionTask("Upload shipment details", "YahooShipmentUpload", ActionTaskCategory.UpdateOnline)]
    public class YahooShipmentUploadTask : StoreInstanceTaskBase
    {
        private const long maxBatchSize = 1000;

        /// <summary>
        /// Indicates if the task is supported for the specified store
        /// </summary>
        /// <param name="store">The store.</param>
        /// <returns></returns>
        public override bool SupportsStore(StoreEntity store) => store is YahooStoreEntity;

        /// <summary>
        /// Descriptive label which appears on the task editor
        /// </summary>
        public override string InputLabel => "Upload tracking number of:";

        /// <summary>
        /// This task only operates on orders
        /// </summary>
        public override EntityType? InputEntityType => EntityType.ShipmentEntity;

        /// <summary>
        /// Instantiates the editor for this action
        /// </summary>
        public override ActionTaskEditor CreateEditor() => new BasicShipmentUploadTaskEditor();

        /// <summary>
        /// Execute the details upload
        /// </summary>
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

            if (store.YahooStoreID.IsNullOrWhiteSpace())
            {
                EmailShipmentUploadTask(inputKeys, context);
            }
            else
            {
                ApiShipmentUploadTask(inputKeys, context, store);
            }
        }

        private void ApiShipmentUploadTask(List<long> inputKeys, ActionStepContext context, YahooStoreEntity store)
        {
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

        private void EmailShipmentUploadTask(List<long> inputKeys, ActionStepContext context)
        {
            foreach (long entityID in inputKeys)
            {
                try
                {
                    YahooEmailOnlineUpdater updater = new YahooEmailOnlineUpdater();
                    EmailOutboundEntity email = updater.GenerateShipmentUpdateEmail(entityID);

                    if (email != null)
                    {
                        context.AddGeneratedEmail(email);
                    }
                }
                catch (YahooException ex)
                {
                    throw new ActionTaskRunException(ex.Message, ex);
                }
            }
        }
    }
}
