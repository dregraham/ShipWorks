using System;
using System.Runtime.InteropServices;

namespace ShipWorks.Installer.Behaviors
{
    public static class NativeMethods
    {
        public const int WM_NCCALCSIZE = 0x83;
        public const int WM_NCPAINT = 0x85;

        /// <summary>
        /// Wrapper for the Windows Kernel32.LoadLibrary method
        /// </summary>
        [DllImport("kernel32", SetLastError = true)]
        private static extern IntPtr LoadLibrary(string lpFileName);

        /// <summary>
        /// Wrapper for the Windows Desktop Window Manager DwmIsCompositionEnabled method
        /// </summary>
        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern bool DwmIsCompositionEnabled();

        /// <summary>
        /// Wrapper for the Windows Kernal32.GetProcAddress method
        /// </summary>
        [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        /// <summary>
        /// Struct representing Windows margin values
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct MARGINS
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
        }

        private delegate int DwmExtendFrameIntoClientAreaDelegate(IntPtr hwnd, ref MARGINS margins);

        /// <summary>
        /// Extends the window client area into the non-client frame of the window
        /// </summary>
        public static int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS margins)
        {
            var hModule = LoadLibrary("dwmapi");

            if (hModule == IntPtr.Zero)
            {
                return 0;
            }

            var procAddress = GetProcAddress(hModule, "DwmExtendFrameIntoClientArea");

            if (procAddress == IntPtr.Zero)
            {
                return 0;
            }

            var delegateForFunctionPointer = (DwmExtendFrameIntoClientAreaDelegate) Marshal.GetDelegateForFunctionPointer(procAddress, typeof(DwmExtendFrameIntoClientAreaDelegate));

            return delegateForFunctionPointer(hwnd, ref margins);
        }

        /// <summary>
        /// Determines if the Windows Desktop Window Manager api is available
        /// </summary>
        public static bool IsDwmAvailable()
        {
            if (LoadLibrary("dwmapi") == IntPtr.Zero)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Int pointer values for Dwm api actions
        /// </summary>
        internal enum WVR
        {
            ALIGNTOP = 0x0010,
            ALIGNLEFT = 0x0020,
            ALIGNBOTTOM = 0x0040,
            ALIGNRIGHT = 0x0080,
            HREDRAW = 0x0100,
            VREDRAW = 0x0200,
            VALIDRECTS = 0x0400,
            REDRAW = HREDRAW | VREDRAW
        }
    }
}
