using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Data.Utility
{
    /// <summary>
    /// Defines both the SortExpression and Relations (relative to some outwardly known query) needed to use them
    /// </summary>
    public sealed class SortDefinition
    {
        SortExpression sortExpression;
        RelationCollection sortRelations;

        /// <summary>
        /// Constructor
        /// </summary>
        public SortDefinition(SortExpression sortExpression, RelationCollection sortRelations)
        {
            if (sortExpression == null)
            {
                throw new ArgumentNullException("sortExpression");
            }

            this.sortExpression = sortExpression;
            this.sortRelations = sortRelations;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SortDefinition(List<SortClause> sortClauses, RelationCollection sortRelations)
        {
            this.sortExpression = new SortExpression();
            this.sortRelations = sortRelations;

            if (sortClauses != null)
            {
                foreach (var clause in sortClauses)
                {
                    sortExpression.Add(clause);
                }
            }
        }

        /// <summary>
        /// Create a clone of this SortDefinition
        /// </summary>
        public SortDefinition Clone()
        {
            // First clone the relations
            RelationCollection cloneRelations = null;

            if (sortRelations != null)
            {
                cloneRelations = new RelationCollection();
                cloneRelations.AddRange(sortRelations);

                // Propogate the properties
                cloneRelations.ObeyWeakRelations = sortRelations.ObeyWeakRelations;
                cloneRelations.SelectListAlias = sortRelations.SelectListAlias;
            }

            // Return the definition clone
            return new SortDefinition(this.sortExpression, cloneRelations);
        }

        /// <summary>
        /// The SortExpression
        /// </summary>
        public SortExpression SortExpression
        {
            get { return sortExpression; }
        }

        /// <summary>
        /// The Relations required to get from the Entities in scope to the entities used by the sort
        /// </summary>
        public RelationCollection Relations
        {
            get { return sortRelations; }
        }

        /// <summary>
        /// Get the lookup key string for the given sort
        /// </summary>
        public string GetDescription()
        {
            if (SortExpression.Count == 0)
            {
                return string.Empty;
            }

            //
            // Note: This method does not take into consideration alias's, so the results may not work if trying to actually use it to do a query. I do
            //       believe it will always be unique enough though to ensure correct caching sort-miss behavior.
            //

            StringBuilder sb = new StringBuilder();

            foreach (SortClause clause in SortExpression)
            {
                sb.AppendFormat("[{0}.{1}.{2}]", clause.FieldToSortCore.ContainingObjectName, clause.FieldToSortCore.Name, clause.SortOperatorToUse);
            }

            if (Relations != null)
            {
                foreach (EntityRelation relation in Relations)
                {
                    var startEntity = relation.StartEntityIsPkSide ? relation.GetPKEntityFieldCore(0) : relation.GetFKEntityFieldCore(0);
                    var endEntity = relation.StartEntityIsPkSide ? relation.GetFKEntityFieldCore(0) : relation.GetPKEntityFieldCore(0);

                    sb.AppendFormat("<{0}.{1}->{2}.{3}>", startEntity.ContainingObjectName, startEntity.Name, endEntity.ContainingObjectName, endEntity.Name);
                }
            }

            return sb.ToString();
        }

    }
}
