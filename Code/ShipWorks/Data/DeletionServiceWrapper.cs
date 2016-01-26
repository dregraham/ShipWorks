using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Data
{
    /// <summary>
    /// Wrapper for the static Deletion Service
    /// </summary>
    class DeletionServiceWrappe : IDeletionService
    {
        /// <summary>
        /// Deletes the given store entity
        /// </summary>
        /// <param name="store"></param>
        public void DeleteStore(StoreEntity store)
        {
            DeletionService.DeleteStore(store);
        }
    }
}
