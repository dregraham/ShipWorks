using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using System.Collections.Generic;
using System.Linq;

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
        public void DeleteStore(StoreEntity store)
        {
            if (store == null)
            {
                return;
            }

            // Remove the store from ShipWorks
            DeletionService.DeleteStore(store);
        }

        /// <summary>
        /// Deletes all the stores for the given channel
        /// </summary>
        public void DeleteChannel(StoreTypeCode channel)
        {
            // Get all of the local stores that match the type we want to remove
            List<StoreEntity> localStoresToDelete = StoreManager.GetAllStores().Where(s => s.TypeCode == (int)channel).ToList();

            // if there are no local stores of that type return 
            if (localStoresToDelete.Any())
            {
                // remove the local stores individually 
                localStoresToDelete.ForEach(DeleteStore);
            }
        }
    }
}
