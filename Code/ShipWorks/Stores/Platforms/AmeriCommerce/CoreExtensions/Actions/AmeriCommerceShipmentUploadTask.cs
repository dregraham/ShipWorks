using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.AmeriCommerce.CoreExtensions.Actions
{
    /// <summary>
    /// Task for uploading shipment details to AmeriCommerce
    /// </summary>
    [ActionTask("Upload shipment details", "AmeriCommerceShipmentUpload", ActionTaskCategory.UpdateOnline)]
    public class AmeriCommerceShipmentUploadTask : StoreTypeTaskBase
    {
        /// <summary>
        /// Indicates if the task supports the give store type
        /// </summary>
        /// <param name="storeType"></param>
        /// <returns></returns>
        public override bool SupportsType(StoreType storeType)
        {
            return storeType is AmeriCommerceStoreType;
        }

        /// <summary>
        /// The ActionTask should be run async
        /// </summary>
        public override bool IsAsync => true;

        /// <summary>
        /// Descriptive label which appears on the task editor
        /// </summary>
        public override string InputLabel { get; } = "Upload tracking number of:";

        /// <summary>
        /// This task only operates on shipments
        /// </summary>
        public override EntityType? InputEntityType { get; } = EntityType.ShipmentEntity;

        /// <summary>
        /// Instantiates the editor for this action
        /// </summary>
        public override ActionTaskEditor CreateEditor() => new BasicShipmentUploadTaskEditor();

        /// <summary>
        /// Execute the details upload
        /// </summary>
        protected override async Task RunAsync(List<long> inputKeys)
        {
            foreach (long shipmentID in inputKeys)
            {
                List<long> storeKeys = DataProvider.GetRelatedKeys(shipmentID, EntityType.StoreEntity);
                if (storeKeys.Count == 0)
                {
                    // Store or shipment disappeared
                    continue;
                }

                AmeriCommerceStoreEntity store = StoreManager.GetStore(storeKeys[0]) as AmeriCommerceStoreEntity;
                if (store == null)
                {
                    // This isn't a generic store or the store went away
                    continue;
                }

                try
                {
                    using (ILifetimeScope scope = IoC.BeginLifetimeScope())
                    {
                        IAmeriCommerceOnlineUpdater updater = scope.Resolve< IAmeriCommerceOnlineUpdater>();
                        await updater.UploadOrderShipmentDetails(store, shipmentID).ConfigureAwait(false);
                    }
                }
                catch (AmeriCommerceException ex)
                {
                    throw new ActionTaskRunException(ex.Message, ex);
                }
            }
        }
    }
}
