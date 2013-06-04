using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.UI.Controls.Design
{
    /// <summary>
    /// Class for detecting if a control - or any of its parent controls - are in DesignMode
    /// </summary>
    public static class DesignModeDetector
    {
        /// <summary>
        /// Determines if the given control, or any of it's parent controls, are hosted by a designer.
        /// </summary>
        public static bool IsDesignerHosted(Control control)
        {
            if (control != null)
            {
                if (control.Site != null && control.Site.DesignMode)
                {
                     return true;
                }

                if (IsDesignerHosted(control.Parent))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
