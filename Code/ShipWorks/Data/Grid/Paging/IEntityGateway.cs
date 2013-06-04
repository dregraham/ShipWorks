using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.ComponentModel;
using ShipWorks.Data.Utility;

namespace ShipWorks.Data.Grid.Paging
{
    /// <summary>
    /// Defines the interface for loading entities from a gateway
    /// </summary>
    public interface IEntityGateway
    {
        /// <summary>
        /// Initialize the gateway and it's sorting to the specified field in the given direction. relations can be null if no non-standard relations are required
        /// to get from the primary table to the sort field table.  Once the gateway is initialized it should be considered immutable.
        /// </summary>
        void Open(SortDefinition sortDefinition);

        /// <summary>
        /// Close the gateway so that no more new data will be pulled from it.  Existing data that is already loaded will still be returned.
        /// </summary>
        void Close();

        /// <summary>
        /// Get the number of rows exposed by the current gateway configuration.  They may not be all loaded yet, which will be indicated in the result.
        /// </summary>
        PagedRowCount GetRowCount();

        /// <summary>
        /// Get the entity that corresponds to the specified row, according to the current sort.  If timeout is non-zero, the gateway waits to determine what entity
        /// the row corresponds to and to load the entity before returning.  If it times out and the corresponding to the row is not known or not cached, then
        /// null is immediately returned.  If the row index is invalid or entity has been deleted, null is returned either way.
        /// </summary>
        EntityBase2 GetEntityFromRow(int row, TimeSpan? timeout);

        /// <summary>
        /// Get the entity with the given primary key value
        /// </summary>
        EntityBase2 GetEntityFromKey(long entityID);

        /// <summary>
        /// Get the key value represented by the given row.  If the row index is out of range, null is returned.
        /// </summary>
        long? GetKeyFromRow(int row);

        /// <summary>
        /// Get an enumerator for enumerating over every key in the result gateway.
        /// </summary>
        IEnumerable<long> GetOrderedKeys();

        /// <summary>
        /// Create an unitialized clone of the gateway
        /// </summary>
        IEntityGateway Clone();
    }
}
