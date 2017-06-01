using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using IWin32Window = System.Windows.Forms.IWin32Window;

namespace Interapptive.Shared.UI
{
    /// <summary>
    /// Base class for creating Wpf windows with a winforms owner
    /// </summary>
    /// <seealso cref="System.Windows.Window" />
    /// <seealso cref="Interapptive.Shared.UI.IDialog" />
    /// <seealso cref="System.Windows.Forms.IWin32Window" />
    public class InteropWindow : Window, IDialog, IWin32Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InteropWindow"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        protected InteropWindow(IWin32Window owner)
        {
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            WindowStyle = WindowStyle.ToolWindow;
            ShowInTaskbar = false;
            ResizeMode = ResizeMode.NoResize;
            FontFamily = new FontFamily("Tahoma");
            FontSize = 11;
            Topmost = true;

            LoadOwner(owner);
        }

        /// <summary>
        /// The handle.
        /// </summary>
        public IntPtr Handle { get; private set; }

        /// <summary>
        /// Loads owner.
        /// </summary>
        /// <param name="owner"></param>
        public void LoadOwner(IWin32Window owner)
        {
            Handle = owner.Handle;

            WindowInteropHelper interopHelper = new WindowInteropHelper(this);
            interopHelper.Owner = owner.Handle;
        }
    }
}