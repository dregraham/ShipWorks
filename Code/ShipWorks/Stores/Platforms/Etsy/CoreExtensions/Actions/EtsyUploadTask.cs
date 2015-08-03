using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Actions.Tasks.Common.Editors;
using System.ComponentModel;
using ShipWorks.Actions;

namespace ShipWorks.Stores.Platforms.Etsy.CoreExtensions.Actions
{
    /// <summary>
    /// Task for uploading shipment detials to Etsy
    /// </summary>
    [ActionTask("Update store status", "EtsyUploadTask", ActionTaskCategory.UpdateOnline)]
    public class EtsyUploadTask : StoreInstanceTaskBase
    {
        string comment = "{//ServiceUsed} - {//TrackingNumber}";

        /// <summary>
        /// This task is for Shipments
        /// </summary>
        public override EntityType? InputEntityType
        {
            get
            {
                return EntityType.ShipmentEntity;
            }
        }

        /// <summary>
        /// Descriptive label which appears on the task editor
        /// </summary>
        public override string InputLabel
        {
            get
            {
                return "Set status of:";
            }
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
        public override bool SupportsStore(StoreEntity store)
        {
            EtsyStoreEntity etsyStore = store as EtsyStoreEntity;

            if (etsyStore == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Instantiates the editor for the action
        /// </summary>
        public override ActionTaskEditor CreateEditor()
        {
            return new EtsyUploadTaskEditor(this);
        }

        /// <summary>
        /// Executes the task
        /// </summary>
        public override void Run(List<long> inputKeys, ActionStepContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context", "context required");
            }

            if (inputKeys == null)
            {
                throw new ArgumentNullException("inputKeys");
            }

            if (StoreID <= 0)
            {
                throw new ActionTaskRunException("A store has not been configured for the task.");
            }

            EtsyStoreEntity store = StoreManager.GetStore(StoreID) as EtsyStoreEntity;

            if (store == null)
            {
                throw new ActionTaskRunException("The store configured for the task has been deleted.");
            }

            EtsyOnlineUpdater updater = new EtsyOnlineUpdater(store);

            foreach (var shipmentID in inputKeys)
            {
                try
                {
                    updater.UpdateOnlineStatus(shipmentID,
                        MarkAsPaid ? true : (bool?)null,
                        MarkAsShipped ? true : (bool?)null,
                        WithComment ? Comment : string.Empty,
                        context.CommitWork);
                }
                catch (EtsyException ex)
                {
                    throw new ActionTaskRunException(ex.Message, ex);
                }
            }
        }
    }
}