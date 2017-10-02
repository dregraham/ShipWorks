using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// Default implementation of the online update command creator
    /// </summary>
    /// <remarks>
    /// This implementation just delegates to the shipment type so that we don't
    /// have to implement all the command creators at once.
    /// </remarks>
    [Component]
    public class DefaultOnlineUpdateCommandCreator : IOnlineUpdateCommandCreator
    {
        private readonly IStoreTypeManager storeTypeManager;
        private readonly StoreTypeCode storeTypeCode;

        /// <summary>
        /// Constructor
        /// </summary>
        public DefaultOnlineUpdateCommandCreator(StoreTypeCode storeTypeCode, IStoreTypeManager storeTypeManager)
        {
            this.storeTypeCode = storeTypeCode;
            this.storeTypeManager = storeTypeManager;
        }

        /// <summary>
        /// Create any MenuCommand's that are applied to the whole StoreType - regardless of how many specific store instances there are.
        /// </summary>
        public IEnumerable<IMenuCommand> CreateOnlineUpdateCommonCommands() =>
            storeTypeManager.GetType(storeTypeCode).CreateOnlineUpdateCommonCommands();

        /// <summary>
        /// Create menu commands for upload shipment details
        /// </summary>
        public IEnumerable<IMenuCommand> CreateOnlineUpdateInstanceCommands(StoreEntity store) =>
            storeTypeManager.GetType(store).CreateOnlineUpdateInstanceCommands();
    }
}
