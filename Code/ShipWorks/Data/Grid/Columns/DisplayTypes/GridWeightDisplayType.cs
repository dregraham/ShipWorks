using ShipWorks.UI.Controls;

namespace ShipWorks.Data.Grid.Columns.DisplayTypes
{
    /// <summary>
    /// Display type for displaying weight information
    /// </summary>
    public class GridWeightDisplayType : GridColumnDisplayType
    {
        /// <summary>
        /// Get formatted weight value
        /// </summary>
        protected override string GetDisplayText(object value)
        {
            if (!(value is double))
            {
                return string.Empty;
            }

            return WeightConverter.Current.FormatWeight((double) value);
        }
    }
}
