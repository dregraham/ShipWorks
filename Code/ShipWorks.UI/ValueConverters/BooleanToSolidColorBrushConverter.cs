using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// Convert a boolean value to a color
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class BooleanToSolidColorBrushConverter : BooleanConverter<SolidColorBrush>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BooleanToSolidColorBrushConverter() : base(Brushes.Black, Brushes.DarkGray)
        {
        }
    }
}
