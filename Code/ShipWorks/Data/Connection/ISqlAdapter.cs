using System;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Data.Connection
{
    /// <summary>
    /// A custom ShipWorks DataAccessAdapter
    /// </summary>
    public interface ISqlAdapter : IDisposable
    {
        /// <summary>
        /// Save the given entity, and automatically refetch it back. Returns true if there were any entities in the graph that were dirty and saved.  Returns
        /// false if nothing was dirty and thus nothing written to the database.
        /// </summary>
        bool SaveAndRefetch(IEntity2 entity);

        /// <summary>
        /// Transaction is committing
        /// </summary>
        void Commit();

        /// <summary>
        /// Fetch an entity from the database
        /// </summary>
        bool FetchEntity(IEntity2 entity);
    }
}