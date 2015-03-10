using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.RelationClasses;

namespace ShipWorks.Data.Grid.Columns.SortProviders
{
    /// <summary>
    /// A value provider that provides the field to sort on as well as the relation required to get to it
    /// </summary>
    public class GridColumnAdvancedSortProvider : GridColumnSortProvider
    {
        RelationCollection relations;
           
        /// <summary>
        /// Constructor
        /// </summary>
        public GridColumnAdvancedSortProvider(EntityField2 sortField, EntityField2 primaryKey, EntityField2 foreignKey)
            : this(sortField, primaryKey, foreignKey, JoinHint.None)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public GridColumnAdvancedSortProvider(EntityField2 sortField, EntityField2 primaryKey, EntityField2 foreignKey, JoinHint joinType)
            : base(sortField)
        {
            EntityRelation relation = new EntityRelation(primaryKey, foreignKey, RelationType.OneToMany, true, string.Empty);
            relation.HintForJoins = joinType;

            relations = new RelationCollection();
            relations.Add(relation);
        }

        /// <summary>
        /// Get the collection of relations necessary to relate to the sort field
        /// </summary>
        public override RelationCollection GetDatabaseSortRelations()
        {
            return relations;
        }

        /// <summary>
        /// Create the sort clauses needed to sort in the given sort direction
        /// </summary>
        public override List<SortClause> GetDatabaseSortClauses(SortOperator direction)
        {
            return SortFields.Select(x => new SortClause(x, null, direction) ).ToList();
        }
    }
}
