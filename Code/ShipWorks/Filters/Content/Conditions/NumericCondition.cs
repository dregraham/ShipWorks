using System;
using ShipWorks.Filters.Content.Editors.ValueEditors;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions
{
    /// <summary>
    /// Base class for conditions that compare against a numeric value
    /// </summary>
    abstract public class NumericCondition<T> : Condition where T: struct, IComparable
    {
        T value1;
        T value2;

        NumericOperator op = NumericOperator.Equal;

        // Used when creating the editor.  Can be overridden by derived classes
        protected string format = "G";
        protected T? minimumValue = null;
        protected T? maximumValue = null;

        /// <summary>
        /// Generate a SQL statement for the given column
        /// </summary>
        protected virtual string GenerateSql(string valueExpression, SqlGenerationContext context)
        {
            // Between \ Not Between
            if (op == NumericOperator.Between || op == NumericOperator.NotBetween)
            {
                // Not operator, if necessary
                string not = (op == NumericOperator.NotBetween) ? "NOT " : "";

                // Register the parameters
                string parm1 = context.RegisterParameter(value1);
                string parm2 = context.RegisterParameter(value2);

                return $"{valueExpression} {not}BETWEEN {parm1} AND {parm2}";
            }
            else
            {
                // Register the parameter
                string parm = context.RegisterParameter(value1);

                return $"{valueExpression} {GetSqlOperator()} {parm}";
            }
        }

        /// <summary>
        /// Get the SQL syntax for the operator we represent
        /// </summary>
        private string GetSqlOperator()
        {
            switch (op)
            {
                case NumericOperator.Equal: return "=";
                case NumericOperator.NotEqual: return "!=";
                case NumericOperator.LessThan: return "<";
                case NumericOperator.LessThanOrEqual: return "<=";
                case NumericOperator.GreaterThan: return ">";
                case NumericOperator.GreaterThanOrEqual: return ">=";
            }

            throw new InvalidOperationException("Invalid operator evaluated in GetSqlOperator.");
        }

        /// <summary>
        /// Primary value to be evaluated.
        /// </summary>
        public T Value1
        {
            get
            {
                return value1;
            }
            set
            {
                value1 = value;
            }
        }
        
        /// <summary>
        /// Second value to be evaluated.  Only used for the Between \ NotBetween operators.
        /// </summary>
        public T Value2
        {
            get
            {
                return value2;
            }
            set
            {
                value2 = value;
            }
        }

        /// <summary>
        /// The operator used for the comparison.
        /// </summary>
        public NumericOperator Operator
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
        /// Create our editor
        /// </summary>
        public override ValueEditor CreateEditor()
        {
            NumericValueEditor<T> editor = new NumericValueEditor<T>(this);

            editor.Format = format;
            editor.MinimumValue = minimumValue;
            editor.MaximumValue = maximumValue;

            return editor;
        }
    }
}
