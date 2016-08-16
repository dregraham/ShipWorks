using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.Users.Security;

namespace ShipWorks.Data
{
    /// <summary>
    /// Service for deleting things from the database
    /// </summary>
    public interface IDeletionService
    {
        /// <summary>
        /// Delete the given store
        /// </summary>
        void DeleteStore(StoreEntity store, ISecurityContext securityContext);

        /// <summary>
        /// Deletes all the stores for the given channel
        /// </summary>
        void DeleteChannel(StoreTypeCode channel, ISecurityContext securityContext);
    }
}