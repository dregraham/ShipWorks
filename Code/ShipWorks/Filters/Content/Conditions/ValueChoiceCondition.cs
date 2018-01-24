using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content.Editors.ValueEditors;
using ShipWorks.Filters.Content.SqlGeneration;
using System.Xml.Serialization;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Filters.Content.Conditions
{
    /// <summary>
    /// Base class for conditions that provide the user with a list of choices from which to choose a value
    /// </summary>
    public abstract class ValueChoiceCondition<T> : Condition where T: struct
    {
        T value = default(T);

        EqualityOperator op = EqualityOperator.Equals;

        /// <summary>
        /// The operator to use when evaluating the condition
        /// </summary>
        public EqualityOperator Operator
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
        /// The target value the condition compares against
        /// </summary>
        public T Value
        {
            get { return value; }
            set { this.value = value; }
        }

        /// <summary>
        /// The list of choices offered to the user
        /// </summary>
        [XmlIgnore]
        public abstract ICollection<ValueChoice<T>> ValueChoices
        {
            get;
        }

        /// <summary>
        /// Generate a SQL statement for the given column
        /// </summary>
        protected virtual string GenerateSql(string valueExpression, SqlGenerationContext context)
        {
            // Register the parameter
            string parm = context.RegisterParameter(value);

            return string.Format("{0} {1} {2}", valueExpression, GetSqlOperator(), parm);
        }

        /// <summary>
        /// Get the SQL version of the configured operator
        /// </summary>
        protected virtual string GetSqlOperator()
        {
            switch (op)
            {
                case EqualityOperator.Equals:
                    return "=";
                case EqualityOperator.NotEqual:
                    return "!=";
            }

            throw new InvalidOperationException("An unhandled operator in GetSqlOperator.");
        }

        /// <summary>
        /// Create the editor used for KeyValue conditions
        /// </summary>
        public override ValueEditor CreateEditor()
        {
            return new ValueChoiceEditor<T>(this);
        }
    }
}
