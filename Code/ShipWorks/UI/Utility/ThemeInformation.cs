using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.VisualStyles;

namespace ShipWorks.UI.Utility
{
    public static class ThemeInformation
    {
        public static bool VisualStylesEnabled
        {
            get
            {
                return VisualStyleRenderer.IsSupported;
            }
        }
    }
}
