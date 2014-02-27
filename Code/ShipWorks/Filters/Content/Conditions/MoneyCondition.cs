using System.Data.SqlTypes;
using ShipWorks.Filters.Content.Editors.ValueEditors;

namespace ShipWorks.Filters.Content.Conditions
{
    /// <summary>
    /// Abstract condition to be used for money data types.
    /// </summary>
    public abstract class MoneyCondition : NumericCondition<decimal>
    {
        /// <summary>
        /// Creates the editor.
        /// </summary>
        public override ValueEditor CreateEditor()
        {
            NumericValueEditor<decimal> editor = new NumericValueEditor<decimal>(this);

            editor.Format = format;
            editor.MinimumValue = SqlMoney.MinValue.ToDecimal();
            editor.MaximumValue = SqlMoney.MaxValue.ToDecimal();

            return editor;
        }
    }
}
