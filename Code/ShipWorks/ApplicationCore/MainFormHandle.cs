using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows.Forms;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Wrapper to get the ShipWorks MainForm handle
    /// </summary>
    public class MainFormHandle : IWin32Window
    {
        private IntPtr handle = IntPtr.Zero;

        /// <summary>
        /// Get the ShipWorks MainForm handle
        /// </summary>
        public IntPtr Handle
        {
            get
            {
                if (handle == IntPtr.Zero)
                {
                    SetHandle();
                }

                return handle;
            }
        }

        /// <summary>
        /// Sets the local handle
        /// </summary>
        private void SetHandle()
        {
            if (Program.MainForm.InvokeRequired)
            {
                Program.MainForm.Invoke((MethodInvoker)delegate { handle = Program.MainForm.Handle; });
                return;
            }

            handle = Program.MainForm.Handle;
        }
    }
}
