using System.Collections.Generic;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// Class that creates online update commands
    /// </summary>
    public interface IOnlineUpdateCommandCreator
    {
        /// <summary>
        /// Create any MenuCommand's that are applied to the whole StoreType - regardless of how many specific store instances there are.
        /// </summary>
        IEnumerable<IMenuCommand> CreateOnlineUpdateCommonCommands();

        /// <summary>
        /// Create menu commands for upload shipment details
        /// </summary>
        IEnumerable<IMenuCommand> CreateOnlineUpdateInstanceCommands(StoreEntity store);
    }
}
