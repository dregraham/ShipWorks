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
    public class BooleanToColorConverter : BooleanConverter<Color>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BooleanToColorConverter() : 
            base(Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 0, 0, 0))
        {
        }
    }
}
