using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using Interapptive.Shared.UI;
using IWin32Window = System.Windows.Forms.IWin32Window;

namespace ShipWorks.UI.Controls.ChannelLimit
{
    /// <summary>
    /// Interaction logic for ChannelLimitDlg.xaml
    /// </summary>
    public partial class ChannelLimitDlg : IWin32Window, IDialog
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelLimitDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Window handle.
        /// </summary>
        public IntPtr Handle { get; set; }

        /// <summary>
        /// Loads the owner.
        /// </summary>
        public void LoadOwner(IWin32Window owner)
        {
            Handle = owner.Handle;

            new WindowInteropHelper(this)
            {
                Owner = owner.Handle
            };
        }

        /// <summary>
        /// Updates the enabled state of the dlg based on the controls state
        /// </summary>
        private void OnEnabledChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            close.IsEnabled = (bool)dependencyPropertyChangedEventArgs.NewValue;
            Cursor = (bool)dependencyPropertyChangedEventArgs.NewValue ? Cursors.Arrow : Cursors.Wait;
        }

        /// <summary>
        /// Called when [click close].
        /// </summary>
        private void OnClickClose(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
