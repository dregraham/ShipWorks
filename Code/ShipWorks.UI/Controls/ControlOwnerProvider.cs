using System.Windows;
using System.Windows.Interop;
using Interapptive.Shared.Win32;
using WinForms = System.Windows.Forms;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Provide the control's owner
    /// </summary>
    public class ControlOwnerProvider
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ControlOwnerProvider(FrameworkElement frameworkElement)
        {
            frameworkElement.Loaded += OnElementLoaded;
            frameworkElement.Unloaded += OnElementUnloaded;
        }

        /// <summary>
        /// Owner of the control
        /// </summary>
        public WinForms.IWin32Window Owner { get; private set; }

        /// <summary>
        /// The button has been loaded
        /// </summary>
        private void OnElementLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement frameworkElement)
            {
                Owner = GetOwnerWindow(frameworkElement) ?? GetOwnerForm(frameworkElement);
            }
        }

        /// <summary>
        /// Unhook the events when the control is unloaded
        /// </summary>
        private void OnElementUnloaded(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement frameworkElement)
            {
                frameworkElement.Loaded -= OnElementLoaded;
                frameworkElement.Unloaded -= OnElementUnloaded;
            }
        }

        /// <summary>
        /// Get the owner Window, if there is one
        /// </summary>
        private WinForms.IWin32Window GetOwnerWindow(FrameworkElement frameworkElement)
        {
            Window owner = Window.GetWindow(frameworkElement);

            return owner == null ?
                (WinForms.IWin32Window) null :
                new NativeWindowHandle(new WindowInteropHelper(owner).Handle);
        }

        /// <summary>
        /// Get the owner Form, if there is one
        /// </summary>
        private WinForms.IWin32Window GetOwnerForm(FrameworkElement frameworkElement)
        {
            HwndSource wpfHandle = PresentationSource.FromVisual(frameworkElement) as HwndSource;

            //the WPF control is hosted if the wpfHandle is not null
            if (wpfHandle != null)
            {
                WinForms.Control host = WinForms.Control.FromChildHandle(wpfHandle.Handle);

                return host?.FindForm();
            }

            return null;
        }
    }
}
