using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Utility;

namespace ShipWorks.Data.Grid.Paging
{
    /// <summary>
    /// A gateway that returns entities from a local collection rather than from the database
    /// </summary>
    public sealed class LocalCollectionEntityGateway<T> : IEntityGateway where T : EntityBase2
    {
        static readonly ILog log = LogManager.GetLogger(typeof(LocalCollectionEntityGateway<T>));

        List<T> entities;
        EntityCollection<T> sortedEntities;

        /// <summary>
        /// Constructor
        /// </summary>
        public LocalCollectionEntityGateway(List<T> entities)
        {
            this.entities = entities;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public LocalCollectionEntityGateway(LocalCollectionEntityGateway<T> copy)
        {
            this.entities = copy.entities;
        }

        /// <summary>
        /// Open the gateway, using the specified sort
        /// </summary>
        public void Open(SortDefinition sortDefinition)
        {
            EntityField2 sortField = null;
            ListSortDirection sortDirection = ListSortDirection.Ascending;

            if (sortDefinition.SortExpression.Count >= 1)
            {
                if (sortDefinition.Relations != null)
                {
                    log.InfoFormat("Local gateway ignoring sort relations.");
                }

                EntityField2 field = (EntityField2) sortDefinition.SortExpression[0].FieldToSortCore;

                if (EntityTypeProvider.GetEntityType(field.ContainingObjectName) == EntityUtility.GetEntityType(typeof(T)))
                {
                    sortField = field;
                    sortDirection = sortDefinition.SortExpression[0].SortOperatorToUse == SortOperator.Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending;
                }
                else
                {
                    log.InfoFormat("LocalGateway ignoring sort since from a different entity.");
                }
            }

            sortedEntities = new EntityCollection<T>();
            sortedEntities.AddRange(entities);

            if ((object) sortField != null)
            {
                sortedEntities.Sort(sortField.FieldIndex, sortDirection);
            }
        }

        /// <summary>
        /// Close the gateway
        /// </summary>
        public void Close()
        {

        }

        /// <summary>
        /// Make an exact clone of the state of the gateway
        /// </summary>
        public IEntityGateway Clone()
        {
            return new LocalCollectionEntityGateway<T>(this);
        }

        /// <summary>
        /// Get the total number of rows exposed by the current gateway configuration.
        /// </summary>
        public PagedRowCount GetRowCount()
        {
            return new PagedRowCount(entities.Count, true);
        }

        /// <summary>
        /// Get the entity that corresponds to the specified row, according to the current sort.
        /// </summary>
        public EntityBase2 GetEntityFromRow(int row, TimeSpan? timeout)
        {
            if (row >= 0 && row < sortedEntities.Count)
            {
                return sortedEntities[row];
            }

            return null;
        }

        /// <summary>
        /// Get the entity with the given primary key value
        /// </summary>
        public EntityBase2 GetEntityFromKey(long entityID)
        {
            return entities.FirstOrDefault(e => (long) e.Fields.PrimaryKeyFields[0].CurrentValue == entityID);
        }

        /// <summary>
        /// Get the key value of the given row, or null if the row is out of range
        /// </summary>
        public long? GetKeyFromRow(int row)
        {
            EntityBase2 entity = GetEntityFromRow(row, TimeSpan.Zero);

            return (entity != null) ? (long?) entity.Fields.PrimaryKeyFields[0].CurrentValue : (long?) null;
        }

        /// <summary>
        /// Get an enumerator for the ordered set of keys in the gateway
        /// </summary>
        public IEnumerable<long> GetOrderedKeys()
        {
            return sortedEntities.Select(e => (long) e.Fields.PrimaryKeyFields[0].CurrentValue);
        }
    }
}
