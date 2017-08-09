using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.BigCommerce.OnlineUpdating;

namespace ShipWorks.Stores.Platforms.BigCommerce.CoreExtensions.Actions
{
    /// <summary>
    /// Task for uploading shipment details to BigCommerce
    /// </summary>
    [ActionTask("Upload shipment details", "BigCommerceShipmentUpload", ActionTaskCategory.UpdateOnline)]
    public class BigCommerceShipmentUploadTask : StoreTypeTaskBase
    {
        /// <summary>
        /// Indicates if the task supports the give store type
        /// </summary>
        /// <param name="storeType"></param>
        /// <returns>True if the store type is BigCommerceStoreType</returns>
        public override bool SupportsType(StoreType storeType) =>
            storeType is BigCommerceStoreType;

        /// <summary>
        /// Descriptive label which appears on the task editor
        /// </summary>
        public override string InputLabel => "Upload tracking number of:";

        /// <summary>
        /// This task only operates on shipments
        /// </summary>
        public override EntityType? InputEntityType => EntityType.ShipmentEntity;

        /// <summary>
        /// Instantiates the editor for this action
        /// </summary>
        public override ActionTaskEditor CreateEditor() => new BasicShipmentUploadTaskEditor();

        /// <summary>
        /// Execute the details upload
        /// </summary>
        protected override async Task RunAsync(List<long> inputKeys)
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                IShipmentDetailsUpdater updater = lifetimeScope.Resolve<IShipmentDetailsUpdater>();
                IStoreManager storeManager = lifetimeScope.Resolve<IStoreManager>();

                foreach (long entityID in inputKeys)
                {
                    BigCommerceStoreEntity storeEntity = storeManager.GetRelatedStore(entityID) as BigCommerceStoreEntity;
                    if (storeEntity == null)
                    {
                        continue;
                    }

                    try
                    {
                        await updater.UpdateShipmentDetails(storeEntity, entityID).ConfigureAwait(false);
                    }
                    catch (BigCommerceException ex)
                    {
                        throw new ActionTaskRunException(ex.Message, ex);
                    }
                }
            }
        }
    }
}
