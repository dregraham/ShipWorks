using System;
using System.Windows;
using System.Windows.Interop;
using Interapptive.Shared.UI;
using IWin32Window = System.Windows.Forms.IWin32Window;

namespace ShipWorks.UI.Controls.ChannelConfirmDelete
{
    /// <summary>
    /// Interaction logic for ChannelConfirmDeleteDlg.xaml
    /// </summary>
    public partial class ChannelConfirmDeleteDlg : IWin32Window, IDialog
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelConfirmDeleteDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Window handle.
        /// </summary>
        public IntPtr Handle { get; set; }

        /// <summary>
        /// The user clicked delete
        /// </summary>
        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        /// <summary>
        /// The user clicked cancel
        /// </summary>
        private void CancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        public void LoadOwner(IWin32Window owner)
        {
            Handle = owner.Handle;

            new WindowInteropHelper(this)
            {
                Owner = owner.Handle
            };
        }
    }
}
