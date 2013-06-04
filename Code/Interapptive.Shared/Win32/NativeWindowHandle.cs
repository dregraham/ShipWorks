using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Interapptive.Shared.Win32
{
    /// <summary>
    /// Utility class for exposing an arbirtary window handle via IWin32Window
    /// </summary>
    public class NativeWindowHandle : IWin32Window
    {
        IntPtr handle;

        /// <summary>
        /// Constructor
        /// </summary>
        public NativeWindowHandle(IntPtr handle)
        {
            this.handle = handle;
        }

        public IntPtr Handle
        {
            get { return handle; }
        }
    }
}
