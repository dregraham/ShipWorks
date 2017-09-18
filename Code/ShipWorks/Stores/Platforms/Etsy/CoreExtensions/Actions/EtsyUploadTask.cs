using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Actions;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Etsy.OnlineUpdating;

namespace ShipWorks.Stores.Platforms.Etsy.CoreExtensions.Actions
{
    /// <summary>
    /// Task for uploading shipment details to Etsy
    /// </summary>
    [ActionTask("Update store status", "EtsyUploadTask", ActionTaskCategory.UpdateOnline)]
    public class EtsyUploadTask : StoreInstanceTaskBase
    {
        private readonly Func<EtsyStoreEntity, IEtsyOnlineUpdater> createUpdater;

        /// <summary>
        /// Constructor
        /// </summary>
        public EtsyUploadTask(Func<EtsyStoreEntity, IEtsyOnlineUpdater> createUpdater)
        {
            this.createUpdater = createUpdater;
        }

        /// <summary>
        /// This task is for Shipments
        /// </summary>
        public override EntityType? InputEntityType => EntityType.ShipmentEntity;

        /// <summary>
        /// Descriptive label which appears on the task editor
        /// </summary>
        public override string InputLabel => "Set status of:";

        /// <summary>
        /// The comment to upload with the status
        /// </summary>
        public string Comment { get; set; } = "{//ServiceUsed} - {//TrackingNumber}";

        /// <summary>
        /// If true, send was_shipped=true. Never send was_shipped=false.
        /// </summary>
        public bool MarkAsShipped { get; set; }

        /// <summary>
        /// If true, send was_paid=true. Never send was_paid=false.
        /// </summary>
        public bool MarkAsPaid { get; set; }

        /// <summary>
        /// Should Set Comment
        /// </summary>
        public bool WithComment { get; set; }

        /// <summary>
        /// Indicates if the task is supported for the specified store
        /// </summary>
        public override bool SupportsStore(StoreEntity store) => store is EtsyStoreEntity;

        /// <summary>
        /// Instantiates the editor for the action
        /// </summary>
        public override ActionTaskEditor CreateEditor() => new EtsyUploadTaskEditor(this);

        /// <summary>
        /// Should the ActionTask be run async
        /// </summary>
        public override bool IsAsync => true;

        /// <summary>
        /// Executes the task
        /// </summary>
        public override async Task RunAsync(List<long> inputKeys, IActionStepContext context)
        {
            MethodConditions.EnsureArgumentIsNotNull(context, nameof(context));
            MethodConditions.EnsureArgumentIsNotNull(inputKeys, nameof(inputKeys));

            if (StoreID <= 0)
            {
                throw new ActionTaskRunException("A store has not been configured for the task.");
            }

            EtsyStoreEntity store = StoreManager.GetStore(StoreID) as EtsyStoreEntity;

            if (store == null)
            {
                throw new ActionTaskRunException("The store configured for the task has been deleted.");
            }

            var updater = createUpdater(store);

            foreach (var shipmentID in inputKeys)
            {
                try
                {
                    await updater.UpdateOnlineStatus(shipmentID,
                        MarkAsPaid ? true : (bool?) null,
                        MarkAsShipped ? true : (bool?) null,
                        WithComment ? Comment : string.Empty,
                        context.CommitWork).ConfigureAwait(false);
                }
                catch (EtsyException ex)
                {
                    throw new ActionTaskRunException(ex.Message, ex);
                }
            }
        }
    }
}