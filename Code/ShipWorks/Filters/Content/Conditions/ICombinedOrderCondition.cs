using ShipWorks.Filters.Content.Editors.ValueEditors;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions
{
    /// <summary>
    /// Interface for combined order conditions
    /// </summary>
    public interface ICombinedOrderCondition
    {
        /// <summary>
        /// Generate the filter SQL
        /// </summary>
        string GenerateSql(SqlGenerationContext context);

        /// <summary>
        /// Create the custom editor
        /// </summary>
        ValueEditor CreateEditor();

        /// <summary>
        /// Indicates if the condition is numeric or string based.
        /// </summary>
        bool IsNumeric { get; set; }

        /// <summary>
        /// Value with which to compare the item code
        /// </summary>
        string StringValue { get; set; }

        /// <summary>
        /// Operator to use in the comparison
        /// </summary>
        StringOperator StringOperator { get; set; }

        /// <summary>
        /// Primary value to be evaluated.
        /// </summary>
        long Value1 { get; set; }

        /// <summary>
        /// Second value to be evaluated.  Only used for the Between \ NotBetween operators.
        /// </summary>
        long Value2 { get; set; }

        /// <summary>
        /// The operator used for the comparison.
        /// </summary>
         NumericOperator Operator { get; set; }
    }
}
