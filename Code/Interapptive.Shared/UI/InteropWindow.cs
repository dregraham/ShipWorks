using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using Interapptive.Shared.Win32;
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
        /// Loads the non wpf owner
        /// </summary>
        /// <remarks>
        /// According to this MSDN article https://blogs.msdn.microsoft.com/wpfsdk/2007/04/03/centering-wpf-windows-with-wpf-and-non-wpf-owner-windows/
        /// "if the owned WPF window has its WindowStartupLocation property set to WindowStartupLocation.CenterOwner,
        /// WPF does not center the owned WPF window over the non-WPF owner window"
        /// 
        /// So we have to manually set the startup position of the new wpf window to be centered 
        /// </remarks>
        /// <param name="owner">The non WPF owner window</param>
        public void LoadOwner(IWin32Window owner)
        {
            Handle = owner.Handle;

            WindowInteropHelper interopHelper = new WindowInteropHelper(this) {Owner = owner.Handle};

            // Need HwndSource to get handle to owned window,
            // and the handle only exists when SourceInitialized has been raised
            SourceInitialized += delegate
            {
                HwndSource source = HwndSource.FromHwnd(interopHelper.Handle);
                
                if (source?.CompositionTarget != null)
                {
                    // Get transform matrix to transform non-WPF owner window
                    // size and location units into device-independent WPF size and location units
                    Matrix matrix = source.CompositionTarget.TransformFromDevice;

                    NativeMethods.RECT rect;
                    NativeMethods.GetWindowRect(owner.Handle, out rect);

                    // Get WPF size and location for non-WPF owner window
                    int ownerLeft = rect.left;
                    int ownerTop = rect.top;
                    int ownerWidth = rect.right - rect.left;
                    int ownerHeight = rect.bottom - rect.top;

                    Point ownerPosition = matrix.Transform(new Point(ownerLeft, ownerTop));
                    Point ownerSize = matrix.Transform(new Point(ownerWidth, ownerHeight));

                    // Center WPF window
                    WindowStartupLocation = WindowStartupLocation.Manual;
                    Left = ownerPosition.X + (ownerSize.X - Width) / 2;
                    Top = ownerPosition.Y + (ownerSize.Y - Height) / 2;
                }
            };
        }
    }
}