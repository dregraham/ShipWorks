using System.Collections.Generic;
using ShipWorks.Actions;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Ebay.CoreExtensions.Actions
{
    /// <summary>
    /// Task for marking eBay orders as shipped or processed
    /// </summary>
    [ActionTask("Update store status", "EbayOnlineUpdate", ActionTaskCategory.UpdateOnline)]
    public class EbayOnlineUpdateTask : StoreTypeTaskBase
    {
        private readonly IEbayOnlineUpdater updater;

        /// <summary>
        /// Constructor
        /// </summary>
        public EbayOnlineUpdateTask(IEbayOnlineUpdater updater)
        {
            this.updater = updater;
        }

        /// <summary>
        /// Mark the eBay transaction as Shipped
        /// </summary>
        public bool MarkShipped { get; set; }

        /// <summary>
        /// Mark the eBay transaction as Paid
        /// </summary>
        public bool MarkPaid { get; set; }

        /// <summary>
        /// Contextual hint label
        /// </summary>
        public override string InputLabel => "Update the details of: ";

        /// <summary>
        /// Indicates if a task supports a given store type
        /// </summary>
        public override bool SupportsType(StoreType storeType) => storeType is EbayStoreType;

        /// <summary>
        /// This task operates on orders
        /// </summary>
        public override EntityType? InputEntityType => EntityType.OrderEntity;

        /// <summary>
        /// Create the UI control for editing the task
        /// </summary>
        public override ActionTaskEditor CreateEditor() =>
            new EbayOnlineUpdateTaskEditor(this);

        /// <summary>
        /// Execute the details upload
        /// </summary>
        public override void Run(List<long> inputKeys, ActionStepContext context)
        {
            foreach (long orderId in inputKeys)
            {
                EbayStoreEntity ebaystore = StoreManager.GetRelatedStore(orderId) as EbayStoreEntity;
                if (ebaystore == null)
                {
                    // store disappeared
                    continue;
                }

                var markPaid = MarkPaid ? true : (bool?) null;
                var markShipped = MarkShipped ? true : (bool?) null;

                try
                {
                    updater.UpdateOnlineStatus(ebaystore, orderId, markPaid, markShipped, context.CommitWork);
                }
                catch (EbayException ex)
                {
                    throw new ActionTaskRunException(ex.Message, ex);
                }
            }
        }
    }
}
