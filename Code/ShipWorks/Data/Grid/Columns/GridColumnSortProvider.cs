using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Grid.Columns.ValueProviders;

namespace ShipWorks.Data.Grid.Columns
{
    /// <summary>
    /// Provides sorting data to use for a grid column
    /// </summary>
    public class GridColumnSortProvider
    {
        EntityField2 sortField;

        // Function that is used to calculate the sort value
        Func<EntityBase2, object> sortValueFunction;

        // Optional transformation function to be applied before getting the local sort value
        Func<EntityBase2, EntityBase2> entityTransform;

        /// <summary>
        /// Constructor that provides the field that is used to obtain the underlying sort value
        /// </summary>
        public GridColumnSortProvider(EntityField2 sortField)
        {
            this.sortField = sortField;
        }

        /// <summary>
        /// Constructor that provides a function to be used to calculate the local sort value
        /// </summary>
        public GridColumnSortProvider(Func<EntityBase2, object> sortValueFunction)
        {
            this.sortValueFunction = sortValueFunction;
        }

        /// <summary>
        /// The field that is used to sort on.  Can be null if a sorting function is used instead.
        /// </summary>
        public EntityField2 SortField
        {
            get { return sortField; }
        }

        /// <summary>
        /// Optional transformation function to be applied before getting the local sort value
        /// </summary>
        public Func<EntityBase2, EntityBase2> EntityTransform
        {
            get { return entityTransform; }
            set { entityTransform = value; }
        }

        /// <summary>
        /// Get the value to use for local (in memory) sorting for the given entity
        /// </summary>
        public virtual object GetLocalSortValue(EntityBase2 entity)
        {
            if (entity != null && entityTransform != null)
            {
                entity = entityTransform(entity);
            }

            if (entity == null)
            {
                return null;
            }

            if (sortValueFunction != null)
            {
                return sortValueFunction(entity);
            }

            if ((object) sortField == null)
            {
                throw new InvalidOperationException("Cannot determine local sort value since sort field is null.");
            }

            return EntityUtility.GetFieldValue(entity, sortField);
        }

        /// <summary>
        /// Get the collectino of relations necessary to relate to the sort field
        /// </summary>
        public virtual RelationCollection GetDatabaseSortRelations()
        {
            return null;
        }

        /// <summary>
        /// Create the sort clauses needed to sort in the given sort direction
        /// </summary>
        public virtual List<SortClause> GetDatabaseSortClauses(SortOperator direction)
        {
            if ((object) sortField == null)
            {
                throw new InvalidOperationException("Cannot create sort clauses since SortField is null.");
            }

            return new List<SortClause> { new SortClause(SortField, null, direction) };
        }
    }
}
