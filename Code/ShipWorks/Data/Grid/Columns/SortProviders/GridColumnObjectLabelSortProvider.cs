using System.Collections.Generic;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Data.Grid.Columns.SortProviders
{
    /// <summary>
    /// Class that supports sorting on the Label column of the ObjectLabel table as related to the given ObjectID field
    /// </summary>
    public class GridColumnObjectLabelSortProvider : GridColumnAdvancedSortProvider
    {
        /// <summary>
        /// Constructor, takes the field of the displayed table with the object key
        /// </summary>
        public GridColumnObjectLabelSortProvider(EntityField2 objectKeyField)
            : base(ObjectLabelFields.Label, ObjectLabelFields.EntityID, objectKeyField, JoinHint.Right)
        {

        }

        /// <summary>
        /// Create the sort clauses used to sort
        /// </summary>
        public override List<SortClause> GetDatabaseSortClauses(SortOperator direction)
        {
            List<SortClause> sortClauses = new List<SortClause>();

            sortClauses.Add(new SortClause(CreateObjectTypeSortField(), null, direction));
            sortClauses.Add(new SortClause(CreateLabelSortField(), null, direction));

            return sortClauses;
        }

        /// <summary>
        /// Create the field to sort on the label
        /// </summary>
        private EntityField2 CreateLabelSortField()
        {
            EntityField2 labelSortField = ObjectLabelFields.Label;

            string padExpression = string.Format("CASE WHEN {0} = {1} AND DATALENGTH({2}) < {3} THEN REPLICATE('0', {3} - DATALENGTH({2})) + {2} ELSE {2} END",
                "{0}",
                EntityUtility.GetEntitySeed(EntityType.OrderEntity),
                "{1}",
                12
                );

            labelSortField.ExpressionToApply = new DbFunctionCall(
                padExpression,
                new object[] { ObjectLabelFields.ObjectType, ObjectLabelFields.Label });

            return labelSortField;
        }

        /// <summary>
        /// Create the sort field to be used for Store.TypeCode
        /// </summary>
        private static EntityField2 CreateObjectTypeSortField()
        {
            EntityField2 objecteTypeSortField = ObjectLabelFields.ObjectType;

            StringBuilder sb = new StringBuilder("CASE {0} ");

            int index = 0;
            foreach (EntityType entityType in GetEntityTypeSortOrder())
            {
                index++;

                sb.AppendFormat(" WHEN {0} THEN {1} ", EntityUtility.GetEntitySeed(entityType), index);
            }

            sb.AppendFormat(" ELSE 100 END");

            objecteTypeSortField.ExpressionToApply = new DbFunctionCall(sb.ToString(), new object[] { ObjectLabelFields.ObjectType });

            return objecteTypeSortField;
        }

        /// <summary>
        /// Get a map that maps EntityType's to how they should be sorted
        /// </summary>
        private static List<EntityType> GetEntityTypeSortOrder()
        {
            return ObjectLabel.SortOrderByEntity;
        }
    }
}
