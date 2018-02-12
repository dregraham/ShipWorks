using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using System.Xml.Serialization;
using Interapptive.Shared.Utility;
using ShipWorks.Filters.Content.Editors.ValueEditors;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions
{
    /// <summary>
    /// Base class for conditions that provide options from a list enum values
    /// </summary>
    public abstract class EnumCondition<T> : Condition where T : struct
    {
        /// <summary>
        /// The operator to use when evaluating the condition
        /// </summary>
        public virtual EnumEqualityOperator Operator { get; set; } = EnumEqualityOperator.Equals;

        /// <summary>
        /// The target value the condition compares against
        /// </summary>
        public T Value { get; set; } = default(T);

        /// <summary>
        /// When in list mode, the list of selected values.
        /// </summary>
        public IEnumerable<T> SelectedValues = new List<T>();

        /// <summary>
        /// Get the value choices the user will be provided with
        /// </summary>
		[XmlIgnore]
        public virtual ICollection<ValueChoice<T>> ValueChoices
        {
            get
            {
                return EnumHelper.GetEnumList<T>().Select(e => new ValueChoice<T>(e.Description, e.Value)).ToList();
            }
        }

        /// <summary>
        /// Generate a SQL statement for the given column
        /// </summary>
        protected virtual string GenerateSql(string valueExpression, SqlGenerationContext context)
        {
            if (IsInListOperator(Operator))
            {
                return GenerateInListSql(valueExpression, context);
            }

            // Register the parameter
            string parm = context.RegisterParameter(Value);

            return $"{valueExpression} {GetSqlOperator()} {parm}";
        }

        /// <summary>
        /// Generate sql need for in/not in list operations
        /// </summary>
        private string GenerateInListSql(string valueExpression, SqlGenerationContext context)
        {
            if (IsInListOperator(Operator))
            {
                if (SelectedValues == null || SelectedValues.None())
                {
                    // If there are no values in the list, nothing should match what is in the list
                    // so add a where clause that is always false so no results will be found.
                    return "-1 = 1";
                }

                string not = Operator == EnumEqualityOperator.NotIsInList ? "not" : string.Empty;

                string param = string.Join(",", SelectedValues.Select(v => context.RegisterParameter(v)));

                return $"{valueExpression} {not} in ({param})";
            }

            throw new InvalidOperationException($"GenerateInListSql was called with StringOperator value of { EnumHelper.GetDescription(Operator) }.");
        }

        /// <summary>
        /// Indicates if our current operator utilizes the in list clause
        /// </summary>
        private static bool IsInListOperator(EnumEqualityOperator op) =>
            op == EnumEqualityOperator.IsInList || op == EnumEqualityOperator.NotIsInList;

        /// <summary>
        /// Get the SQL version of the configured operator
        /// </summary>
        protected virtual string GetSqlOperator()
        {
            switch (Operator)
            {
                case EnumEqualityOperator.Equals:
                    return "=";
                case EnumEqualityOperator.NotEqual:
                    return "!=";
            }

            throw new InvalidOperationException("An unhandled operator in GetSqlOperator.");
        }

        /// <summary>
        /// Create the editor used for KeyValue conditions
        /// </summary>
        public override ValueEditor CreateEditor()
        {
            //return new ValueChoiceEditor<T>(this);
            return null;
        }

    }
}
