using System;
using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.Filters.Content.Editors.ValueEditors;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions
{
    /// <summary>
    /// Base class for conditions that compare against a text value
    /// </summary>
    abstract public class StringCondition : Condition
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

            throw new InvalidOperationException("Unhandled operator defined for StringCondition.");
        }

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
        /// displaying a dropdown instead of an edit box.
        /// </summary>
        public virtual ICollection<string> GetStandardValues()
        {
            return null;
        }
    }
}
