using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.NetworkSolutions.OnlineUpdating;

namespace ShipWorks.Stores.Platforms.NetworkSolutions.CoreExtensions.Actions
{
    /// <summary>
    /// Task for uploading shipment details to NetworkSolutions
    /// </summary>
    [ActionTask("Upload shipment details", "NetworkSolutionsShipmentUpload", ActionTaskCategory.UpdateOnline)]
    public class NetworkSolutionsShipmentUploadTask : StoreTypeTaskBase
    {
        private readonly IShipmentDetailsUpdater shipmentDetailsUpdater;
        private readonly IDataProvider dataProvider;
        private readonly IStoreManager storeManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public NetworkSolutionsShipmentUploadTask(IShipmentDetailsUpdater shipmentDetailsUpdater, IDataProvider dataProvider, IStoreManager storeManager)
        {
            this.storeManager = storeManager;
            this.dataProvider = dataProvider;
            this.shipmentDetailsUpdater = shipmentDetailsUpdater;
        }

        /// <summary>
        /// Should the task run async
        /// </summary>
        public override bool IsAsync => true;

        /// <summary>
        /// Indicates if the task supports the give store type
        /// </summary>
        /// <param name="storeType"></param>
        /// <returns></returns>
        public override bool SupportsType(StoreType storeType) =>
            storeType is NetworkSolutionsStoreType;

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
        protected override async Task RunAsync(List<long> inputKeys)
        {
            foreach (long entityID in inputKeys)
            {
                IEnumerable<long> storeKeys = dataProvider.GetRelatedKeys(entityID, EntityType.StoreEntity);
                if (storeKeys.None())
                {
                    // Store or shipment disappeared
                    continue;
                }

                INetworkSolutionsStoreEntity store = storeManager.GetStore(storeKeys.First()) as INetworkSolutionsStoreEntity;
                if (store == null)
                {
                    // This isn't a generic store or the store went away
                    continue;
                }

                try
                {
                    await shipmentDetailsUpdater.UploadShipmentDetails(store, entityID).ConfigureAwait(false);
                }
                catch (NetworkSolutionsException ex)
                {
                    throw new ActionTaskRunException(ex.Message, ex);
                }
            }
        }
    }
}
