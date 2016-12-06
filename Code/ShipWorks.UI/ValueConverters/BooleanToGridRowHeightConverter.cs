using ShipWorks.UI.Controls.Design;
using System.Windows;
using System.Windows.Data;

namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// Convert a boolean to * for true, 0 for false.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(GridLength))]
    public class BooleanToGridRowHeightConverter : BooleanConverter<GridLength>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BooleanToGridRowHeightConverter()
            : base(new GridLength(1, GridUnitType.Star), new GridLength(0), DesignModeDetector.IsDesignerHosted())
        {
        }
    }
}
