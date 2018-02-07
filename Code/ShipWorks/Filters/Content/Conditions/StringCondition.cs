using Interapptive.Shared.Collections;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Filters.Content.Editors.ValueEditors;
using ShipWorks.Filters.Content.SqlGeneration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShipWorks.Filters.Content.Conditions
{
    /// <summary>
    /// Base class for conditions that compare against a text value
    /// </summary>
    public abstract class StringCondition : Condition
    {
        // String to compare against
        string targetValue = "";

        // Operator to use for the comparison
        StringOperator op = StringOperator.Equals;

        /// <summary>
        /// Generate a SQL statement for the given column
        /// </summary>
        protected string GenerateSql(string valueExpression, SqlGenerationContext context)
        {
            return GenerateSql(targetValue, op, valueExpression, context);
        }

        /// <summary>
        /// Generate a SQL statement for the given operator and target value as compared to the specified expression.
        /// </summary>
        public static string GenerateSql(string targetValue, StringOperator stringOp, string valueExpression, SqlGenerationContext context)
        {
            if (IsInListOperator(stringOp))
            {
                return GenerateInListSql(targetValue, stringOp, valueExpression, context);
            }

            // We're limiting to 3998 since 4k is the largest number of characters we can use without SQL Server
            // throwing an exception. 3998 allows wildcards to fit in the max while being consistent. If this limit
            // is a problem, we should find out what the customer's use case is.
            string truncatedValue = targetValue.Truncate(3998);

            if (IsLikeOperator(stringOp))
            {
                string param = context.RegisterParameter(GetLikeValue(truncatedValue, stringOp));
                string not = (stringOp == StringOperator.NotContains) ? "NOT " : "";

                return string.Format("{0} {1}LIKE {2}", valueExpression, not, param);
            }
            else
            {
                // Register the parameter
                string param = context.RegisterParameter((stringOp == StringOperator.IsEmpty) ? "" : truncatedValue);

                if (stringOp == StringOperator.Equals || stringOp == StringOperator.IsEmpty)
                {
                    return string.Format("{0} = {1}", valueExpression, param);
                }

                if (stringOp == StringOperator.NotEqual)
                {
                    return string.Format("{0} != {1}", valueExpression, param);
                }

                if (stringOp == StringOperator.Matches)
                {
                    return string.Format("dbo.RegexMatch({0}, {1}) = 1", valueExpression, param);
                }
            }

            throw new InvalidOperationException("Unhanded operator defined for StringCondition.");
        }

        /// <summary>
        /// Generate sql need for in/not in list operations
        /// </summary>
        private static string GenerateInListSql(string targetValue, StringOperator stringOp, string valueExpression, SqlGenerationContext context)
        {
            if (IsInListOperator(stringOp))
            {
                var listOfValues = targetValue
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim().Truncate(3998))
                    .Distinct();

                if (listOfValues?.None() == true)
                {
                    // If there are no values in the list, nothing should match what is in the list
                    // so add a where clause that is always false so no results will be found.
                    return "-1 = 1";
                }

                string not = stringOp == StringOperator.NotIsInList ? "not" : string.Empty;

                string param = string.Join(",", listOfValues.Select(context.RegisterParameter));

                return $"{valueExpression} {not} in ({param})";
            }

            throw new InvalidOperationException($"GenerateInListSql was called with StringOperator value of { EnumHelper.GetDescription(stringOp) }.");
        }

        /// <summary>
        /// Indicates if our current operator utilizes the in list clause
        /// </summary>
        private static bool IsInListOperator(StringOperator op) =>
            op == StringOperator.IsInList || op == StringOperator.NotIsInList;

        /// <summary>
        /// Indicates if our current operator utilizes the LIKE clause
        /// </summary>
        private static bool IsLikeOperator(StringOperator op)
        {
            switch (op)
            {
                case StringOperator.BeginsWith:
                case StringOperator.Contains:
                case StringOperator.EndsWith:
                case StringOperator.NotContains:
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Format our current value as a like statement using the current operator
        /// </summary>
        private static string GetLikeValue(string targetValue, StringOperator stringOp)
        {
            switch (stringOp)
            {
                case StringOperator.BeginsWith:
                    return string.Format("{0}%", targetValue);

                case StringOperator.EndsWith:
                    return string.Format("%{0}", targetValue);

                case StringOperator.Contains:
                case StringOperator.NotContains:
                    return string.Format("%{0}%", targetValue);
            }

            throw new InvalidOperationException("Invalid operator utilized by GetLikeValue.");
        }

        /// <summary>
        /// Create the editor for the string condition type.
        /// </summary>
        public override ValueEditor CreateEditor()
        {
            StringValueEditor editor = new StringValueEditor(this);

            return editor;
        }

        /// <summary>
        /// Value with which to compare the item code
        /// </summary>
        public virtual string TargetValue
        {
            get
            {
                return targetValue;
            }
            set
            {
                targetValue = value;
            }
        }

        /// <summary>
        /// Operator to use in the comparison
        /// </summary>
        public StringOperator Operator
        {
            get
            {
                return op;
            }
            set
            {
                op = value;
            }
        }

        /// <summary>
        /// Provide a list of standard values for the user to choose from.  The user can still
        /// type in whatever they want, in addition to selecting a value. Return null to provide
        /// no standard values.  Returning a non-null collection will result in the editor
        /// displaying a drop down instead of an edit box.
        /// </summary>
        public virtual ICollection<string> GetStandardValues()
        {
            return null;
        }

        /// <summary>
        /// Get a collection of items represented by the given string
        /// </summary>
        public static IEnumerable<string> ValueAsItems(string value)
        {
            return value
                .Replace(@"\,", "\a")
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim().Replace("\a", ","));
        }

        /// <summary>
        /// Gets a value that represents the collection of items
        /// </summary>
        public static string ItemsAsValue(IEnumerable<string> items)
        {
            return items.Select(x => x.Trim().Replace(",", @"\,")).Combine(",");
        }
    }
}
