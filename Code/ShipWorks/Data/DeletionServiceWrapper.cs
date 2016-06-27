using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.Users.Audit;
using ShipWorks.Users.Security;

namespace ShipWorks.Data
{
    /// <summary>
    /// Wrapper for the static Deletion Service
    /// </summary>
    class DeletionServiceWrapper : IDeletionService
    {
        /// <summary>
        /// Deletes the given store entity
        /// </summary>
        public void DeleteStore(StoreEntity store, ISecurityContext securityContext)
        {
            if (store == null)
            {
                return;
            }

            using (new AuditBehaviorScope(AuditState.Disabled))
            {
                // Remove the store from ShipWorks
                DeletionService.DeleteStore(store, securityContext);
            }
        }

        /// <summary>
        /// Deletes all the stores for the given channel
        /// </summary>
        public void DeleteChannel(StoreTypeCode channel, ISecurityContext securityContext)
        {
            // Get all of the local stores that match the type we want to remove
            List<StoreEntity> localStoresToDelete = StoreManager.GetAllStores().Where(s => s.TypeCode == (int) channel).ToList();

            // if there are no local stores of that type return
            if (localStoresToDelete.Any())
            {
                // remove the local stores individually
                localStoresToDelete.ForEach(x => DeleteStore(x, securityContext));
            }
        }
    }
}
