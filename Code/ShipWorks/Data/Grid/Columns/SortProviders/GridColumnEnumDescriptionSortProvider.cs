using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using Interapptive.Shared.Utility;

namespace ShipWorks.Data.Grid.Columns.SortProviders
{
    /// <summary>
    /// Sort provider that sorts based on the Description attribute applied to an enum rather than its value
    /// </summary>
    public class GridColumnEnumDescriptionSortProvider<T> : GridColumnSortProvider where T: struct
    {
        Dictionary<T, int> sortIndices;

        /// <summary>
        /// Constructor that provides the field that is used to obtain the underlying sort value
        /// </summary>
        public GridColumnEnumDescriptionSortProvider(EntityField2 sortField)
            : base(sortField)
        {
            sortIndices = EnumHelper.GetEnumList<T>()
                .OrderBy(e => e.Description)
                .Select((e, i) => new { Value = e.Value, Index = i })
                .ToDictionary(a => a.Value, a => a.Index);

            ApplySortExpression(sortField);
        }

        /// <summary>
        /// Apply the expression required to translate the sort to the given field
        /// </summary>
        private void ApplySortExpression(EntityField2 sortField)
        {
            StringBuilder sb = new StringBuilder("CASE {0} ");

            foreach (var pair in sortIndices)
            {
                sb.AppendFormat(" WHEN {0} THEN {1} ", (int) (object) pair.Key, pair.Value);
            }

            sb.AppendFormat(" ELSE -1 END");

            sortField.ExpressionToApply = BuildDbFunctionCall(sortField, sb);
        }

        /// <summary>
        /// Get the local sort value
        /// </summary>
        public override object GetLocalSortValue(EntityBase2 entity)
        {
            object value = base.GetLocalSortValue(entity);

            if (value is int)
            {
                int sortIndex;
                if (sortIndices.TryGetValue((T) Enum.ToObject(typeof(T), value), out sortIndex))
                {
                    return sortIndex;
                }
            }

            return -1;
        }

        /// <summary>
        /// Build the DbFunctionCall that will be used as the sort expression
        /// </summary>
        protected virtual DbFunctionCall BuildDbFunctionCall(EntityField2 sortField, StringBuilder builder)
        {
            return new DbFunctionCall(builder.ToString(), new[] { sortField.Clone() });
        }
    }
}
