using System;
using System.Collections.Generic;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.ComponentModel;
using System.Data;
using ShipWorks.Data.Model.EntityClasses;
using System.Data.SqlClient;
using System.Diagnostics;
using log4net;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters;
using Interapptive.Shared;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.ApplicationCore.Crashes;
using System.Collections;
using System.Threading;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Utility;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.ApplicationCore;
using System.Linq;
using Interapptive.Shared.Data;
using System.Threading.Tasks;

namespace ShipWorks.Data.Grid.Paging
{
    /// <summary>
    /// Provides the means to retrieve entities in a filtered, sorted, and paged fashion.  If the entity has a timestamp column,
    /// it will be utilized to ensure the entity is up-to-date each time it is requested.
    /// </summary>
    public abstract class PagedEntityGateway : IEntityGateway
    {        
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(PagedEntityGateway));

        // Size of a page to pull back
        const int pageSize = 25;

        // Used to fetch entities once we know their ID's
        IEntityProvider entityProvider;
        EntityField2 primaryKeyField;

        // The primary keys of the current gateway in sorted order
        PagedSortedKeys sortedKeys = null;
        volatile bool closed = false;

        /// <summary>
        /// Creates a cache based on the specified entity and prefetch
        /// </summary>
        protected PagedEntityGateway(EntityType entityType)
            : this(new DataProviderEntityProvider(entityType))
        {

        }

        /// <summary>
        /// Creates a new gateway for fetching entities of the given type, and uses the given provider to fetch specific entities.
        /// </summary>
        protected PagedEntityGateway(IEntityProvider entityProvider)
        {
            if (entityProvider == null)
            {
                throw new ArgumentNullException("entityProvider");
            }

            this.entityProvider = entityProvider;

            IEntityFactory2 entityFactory = GeneralEntityFactory.Create(entityProvider.EntityType).GetEntityFactory();
            this.primaryKeyField = entityFactory.CreateFields().OfType<EntityField2>().FirstOrDefault(field => field.IsPrimaryKey);
        }

        /// <summary>
        /// Copy constructor used for cloning
        /// </summary>
        protected PagedEntityGateway(PagedEntityGateway copy)
            : this(copy.entityProvider)
        {

        }

        /// <summary>
        /// Create a new instance of the gateway with the same definition, but none of the cached data
        /// </summary>
        public abstract IEntityGateway Clone();

        /// <summary>
        /// Initialize the gateway and it's sorting to the specified field in the given direction. relations can be null if no non-standard relations are required
        /// to get from the primary table to the sort field table.  Once the gateway is initialized it should be considered immutable.
        /// </summary>
        public void Open(SortDefinition sortDefinition)
        {
            if (sortedKeys != null)
            {
                throw new InvalidOperationException("The gateway has already been opened.");
            }

            // We need to update the sort definition a bit to provide a sort tiebreaker for consistant sorts, and to ensure
            // all needed relations
            sortedKeys = QuerySortedKeys(ResolveSortDefinition(sortDefinition));
        }

        /// <summary>
        /// Close the gateway by canceling any pending data fetch.  Data that already is loaded will continue to be returned.
        /// </summary>
        public void Close()
        {
            if (sortedKeys != null)
            {
                sortedKeys.CancelLoadingIfIncomplete();
            }

            closed = true;
        }

        /// <summary>
        /// Determine which entities are contained in the given page
        /// </summary>
        protected virtual PagedSortedKeys QuerySortedKeys(SortDefinition sortDefinition)
        {
            RelationPredicateBucket bucket = GetQueryFilter();

            // We need to ensure we have a bucket
            if (bucket == null)
            {
                bucket = new RelationPredicateBucket();
            }
            else
            {
                bucket = EntityUtility.ClonePredicateBucket(bucket);
            }

            // Add in the sort stuff
            bucket.Relations.AddRange(sortDefinition.Relations);

            return new PagedSortedKeys(primaryKeyField, bucket, sortDefinition.SortExpression);
        }

        /// <summary>
        /// Returns the list of keys visible to the gateway in sort order
        /// </summary>
        public long? GetKeyFromRow(int row)
        {
            return sortedKeys.GetKeyFromIndex(row);
        }

        /// <summary>
        /// Get the entity that has the given primary key value
        /// </summary>
        public EntityBase2 GetEntityFromKey(long entityID)
        {
            return entityProvider.GetEntity(entityID);
        }

        /// <summary>
        /// Get an enumerator for the ordered set of keys in the gateway
        /// </summary>
        public IEnumerable<long> GetOrderedKeys()
        {
            return sortedKeys;
        }

        /// <summary>
        /// Get the entity that corresponds to the specified row, according to the current sort.
        /// </summary>
        [NDependIgnoreLongMethod]
        public EntityBase2 GetEntityFromRow(int row, TimeSpan? timeout)
        {
            Stopwatch timer = Stopwatch.StartNew();

            // Index into the page of sorted rows to determine the ID we are looking for
            long entityID = sortedKeys.GetKeyFromIndex(row, timeout) ?? -1;
                
            // If null (in whish case we would have set to -1 above) was returned then it indicates an invalid row index
            if (entityID == -1)
            {
                return null;
            }

            // If the entity is already in our cache return it, or if the consumer doesnt want to fetch it just return either way
            EntityBase2 entity = entityProvider.GetEntity(entityID, false);
            if (entity != null || closed)
            {
                return entity;
            }

            int page = 1 + (row - (row % pageSize)) / pageSize;

            // Determine the first and last rows in the given page
            int pageStart = (page - 1) * pageSize;
            int pageEnd = page * pageSize - 1;

            // We will query for a whole 'page' of entities at a time
            List<long> keysInPage = new List<long>();
            for (int i = pageStart; i <= pageEnd; i++)
            {
                long? keyToFetch = sortedKeys.GetKeyFromIndex(i, (timeout == null) ? null : timeout - timer.Elapsed);

                if (keyToFetch != null)
                {
                    keysInPage.Add(keyToFetch.Value);
                }
                else
                {
                    // Passed the last index in the key list
                    break;
                }
            }

            List<EntityBase2> entitiesInPage = null;

            // If there was a timeout, we have to execute the fetch asynchronously
            if (timeout != null)
            {
                TimeSpan timeRemaining = timeout.Value - timer.Elapsed;

                // Only bother if there is still time remaining
                if (timeRemaining > TimeSpan.Zero)
                {
                    // Kickoff the background task
                    var task = Task.Factory.StartNew(() =>
                        {
                            return entityProvider.GetEntities(keysInPage);
                        });

                    // SpinWait until its completed, or until the timeout expires.  Don't use events here, b\c they pump (due to COM STA), which 
                    // can then make this re-entrant.
                    SpinWait.SpinUntil(() => task.IsCompleted || (timeout - timer.Elapsed) < TimeSpan.Zero);

                    // If it actually finished, grab the results
                    if (task.IsCompleted)
                    {
                        entitiesInPage = task.Result;
                    }
                }
            }
            else
            {
                // Timeout doesn't matter, just do it
                entitiesInPage = entityProvider.GetEntities(keysInPage);
            }

            if (entitiesInPage != null)
            {
                // Get it from the fetched list instead of th cache - with the cache there could be a race condition where it gets removed right away
                entity = entitiesInPage.FirstOrDefault(e => (long) e.Fields.PrimaryKeyFields[0].CurrentValue == entityID);
            }

            return entity;
        }

        /// <summary>
        /// Get the number of rows exposed by the current gateway configuration.  The full number may not be known yet if still loading in the background,
        /// in which case the result set will indicate that.
        /// </summary>
        public virtual PagedRowCount GetRowCount()
        {
            return sortedKeys.GetCount();
        }

        /// <summary>
        /// The primary key field of the table the gateway is for
        /// </summary>
        protected EntityField2 PrimaryKeyField
        {
            get { return primaryKeyField; }
        }

        /// <summary>
        /// Get the relation\predicate that will be used to filter the query that requests records.
        /// </summary>
        protected virtual RelationPredicateBucket GetQueryFilter()
        {
            return null;
        }

        /// <summary>
        /// Generate a SortDefinition based on the given clauses and relations
        /// </summary>
        [NDependIgnoreLongMethod]
        private SortDefinition ResolveSortDefinition(SortDefinition sortDefinition)
        {
            SortExpression sortExpression = new SortExpression();
            RelationCollection sortRelations = sortDefinition.Relations;

            // Copy all the clauses
            foreach (SortClause clause in sortDefinition.SortExpression)
            {
                sortExpression.Add(clause);
            }

            // If relations weren't supplied already, try to find them
            if (sortRelations == null)
            {
                EntityType sortEntityType = entityProvider.EntityType;
                List<EntityType> relationsAdded = new List<EntityType>();

                // Add in each SortCaluse into the expression
                foreach (SortClause clause in sortExpression)
                {
                    EntityType clauseEntityType = EntityTypeProvider.GetEntityType(clause.FieldToSortCore.ContainingObjectName);

                    // If relations were not supplied and we need them, we try to find them
                    if (clauseEntityType != sortEntityType)
                    {
                        if (sortRelations == null)
                        {
                            sortRelations = new RelationCollection();
                        }

                        if (!relationsAdded.Contains(clauseEntityType))
                        {
                            // Add relations if it doesnt already exist
                            AddRelationChain(sortEntityType, clauseEntityType, sortRelations);

                            // Mark that we've added a relation to this entity type
                            relationsAdded.Add(clauseEntityType);
                        }
                    }
                }

                // If relations were not supplied and we added are own, we need to make them all LEFT OUTER JOIN's so the sort
                // doesn't filter out result sets.
                if (sortRelations != null)
                {
                    foreach (EntityRelation relation in sortRelations)
                    {
                        relation.HintForJoins = JoinHint.Left;
                    }
                }
            }

            IEntityFactory2 entityFactory = GeneralEntityFactory.Create(entityProvider.EntityType).GetEntityFactory();
            EntityField2 sortTieBreaker = entityFactory.CreateFields().OfType<EntityField2>().FirstOrDefault(field => field.IsPrimaryKey);

            // Add in tiebreaker, if the column to use as a tiebreaker isn't already present in the sort set
            if (!sortExpression.Cast<SortClause>().Any(c => EntityUtility.IsSameField(sortTieBreaker, c.FieldToSortCore)))
            {
                SortOperator direction = sortExpression.Count > 0 ? sortExpression[0].SortOperatorToUse : SortOperator.Descending;

                sortExpression.Add(new SortClause(sortTieBreaker, null, direction));
            }

            return new SortDefinition(sortExpression, sortRelations);
        }

        /// <summary>
        /// Add the chain of relations that will get us from the fromEntity to the toEntity.
        /// </summary>
        private void AddRelationChain(EntityType fromEntityType, EntityType toEntityType, RelationCollection relations)
        {
            // Don't allow one to many. For that we use roll up columns, such as the rolled up
            // ItemName column on the Order table.
            RelationCollection found = EntityUtility.FindRelationChain(fromEntityType, toEntityType, false);

            if (found == null)
            {
                throw new InvalidOperationException(string.Format("Cannot find relation chain from {0} to {1}", fromEntityType, toEntityType));
            }

            relations.AddRange(found);
        }
    }
}
