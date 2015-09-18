using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.Shipping.UI
{
    /// <summary>
    /// Extension methods for Control
    /// </summary>
    public static class ControlExtensions
    {
        /// <summary>
        /// Checks to see if an invoke is required, and if so, Invokes.  Otherwise, just calls the method.
        /// </summary>
        public static void InvokeIfRequired(this Control control, MethodInvoker action, bool returnIfNotVisible)
        {
            if (!control.Visible && returnIfNotVisible)
            {
                return;
            }

            if (control.InvokeRequired)
            {
                control.Invoke(action);
            }
            else
            {
                action();
            }
        }
    }
}
