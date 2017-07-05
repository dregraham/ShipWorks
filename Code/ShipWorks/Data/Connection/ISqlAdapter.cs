﻿using System;
using System.Threading.Tasks;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;

namespace ShipWorks.Data.Connection
{
    /// <summary>
    /// A custom ShipWorks DataAccessAdapter
    /// </summary>
    public interface ISqlAdapter : IDataAccessAdapter
    {
        /// <summary>
        /// Save the given entity, and automatically refetch it back. Returns true if there were any entities in the graph that were dirty and saved.  Returns
        /// false if nothing was dirty and thus nothing written to the database.
        /// </summary>
        bool SaveAndRefetch(IEntity2 entity);
    }
}