using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content.Editors.ValueEditors;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions
{
    /// <summary>
    /// A condition that is based on a numeric field, but allows also searching it like a string (such as "Starts With")
    /// </summary>
    abstract public class NumericStringCondition<T> : NumericCondition<T> where T: struct, IComparable
    {
        bool isNumeric = true;

        // String to compare against
        string stringValue = "";

        // Operator to use for the comparison
        StringOperator stringOp = StringOperator.Equals;

        /// <summary>
        /// Generate the SQL that evaluates the condition
        /// </summary>
        protected override string GenerateSql(string valueExpression, SqlGenerationContext context)
        {
            if (IsNumeric)
            {
                return base.GenerateSql(valueExpression, context);
            }
            else
            {
                return StringCondition.GenerateSql(StringValue, StringOperator, string.Format("CONVERT(varchar(20), {0})", valueExpression), context);
            }
        }

        /// <summary>
        /// Create the custom editor
        /// </summary>
        public override ValueEditor CreateEditor()
        {
            return new NumericStringValueEditor<T>(this);
        }

        /// <summary>
        /// Indicates if the condition is numeric or string based.
        /// </summary>
        public bool IsNumeric
        {
            get
            {
                return isNumeric;
            }
            set
            {
                isNumeric = value;
            }
        }

        /// <summary>
        /// Value with which to compare the item code
        /// </summary>
        public string StringValue
        {
            get
            {
                return stringValue;
            }
            set
            {
                stringValue = value;
            }
        }

        /// <summary>
        /// Operator to use in the comparison
        /// </summary>
        public StringOperator StringOperator
        {
            get
            {
                return stringOp;
            }
            set
            {
                stringOp = value;
            }
        }
    }
}
