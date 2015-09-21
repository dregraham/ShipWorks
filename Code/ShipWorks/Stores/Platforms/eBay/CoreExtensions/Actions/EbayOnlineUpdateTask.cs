using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Ebay;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using ShipWorks.Data.Model;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Stores.Content;
using ShipWorks.Actions;

namespace ShipWorks.Stores.Platforms.Ebay.CoreExtensions.Actions
{
    /// <summary>
    /// Task for marking eBay orders as shipped or processed
    /// </summary>
    [ActionTask("Update store status", "EbayOnlineUpdate", ActionTaskCategory.UpdateOnline)]
    public class EbayOnlineUpdateTask : StoreTypeTaskBase
    {
        bool markShipped = false;
        bool markPaid = false;

        /// <summary>
        /// Mark the eBay transaction as Shipped
        /// </summary>
        public bool MarkShipped
        {
            get { return markShipped; }
            set { markShipped = value; }
        }

        /// <summary>
        /// Mark the eBay transaction as Paid
        /// </summary>
        public bool MarkPaid
        {
            get { return markPaid; }
            set { markPaid = value; }
        }

        /// <summary>
        /// Contextual hint label 
        /// </summary>
        public override string InputLabel
        {
            get
            {
                return "Update the details of: ";
            }
        }

        /// <summary>
        /// Indicates if a task supports a given store type
        /// </summary>
        public override bool SupportsType(StoreType storeType)
        {
            return storeType is EbayStoreType;
        }

        /// <summary>
        /// This task operates on orders
        /// </summary>
        public override EntityType? InputEntityType
        {
            get
            {
                return EntityType.OrderEntity;
            }
        }

        /// <summary>
        /// Create the UI control for editing the task 
        /// </summary>
        public override ActionTaskEditor CreateEditor()
        {
            return new EbayOnlineUpdateTaskEditor(this);
        }

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
                    // store disappered
                    continue;
                }

                try
                {
                    EbayOnlineUpdater updater = new EbayOnlineUpdater(ebaystore);
                    updater.UpdateOnlineStatus(orderId, markPaid ? true : (bool?)null, markShipped ? true : (bool?)null, context.CommitWork);
                }
                catch (EbayException ex)
                {
                    throw new ActionTaskRunException(ex.Message, ex);
                }
            }
        }
    }
}
