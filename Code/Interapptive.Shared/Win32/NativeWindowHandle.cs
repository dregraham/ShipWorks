using System;
using System.Windows.Forms;

namespace Interapptive.Shared.Win32
{
    /// <summary>
    /// Utility class for exposing an arbitrary window handle via IWin32Window
    /// </summary>
    public class NativeWindowHandle : IWin32Window
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NativeWindowHandle(IntPtr handle)
        {
            Handle = handle;
        }

        /// <summary>
        /// Window handle
        /// </summary>
        public IntPtr Handle { get; }
    }
}
