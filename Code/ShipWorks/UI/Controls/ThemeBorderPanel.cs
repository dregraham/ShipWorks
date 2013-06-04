using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Utility;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// A WindowsForms panel drawn with a themed border (if necessary)
    /// </summary>
    public class ThemeBorderPanel : Panel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ThemeBorderPanel()
        {
            ThemedBorderProvider.Apply(this);
        }
    }
}
