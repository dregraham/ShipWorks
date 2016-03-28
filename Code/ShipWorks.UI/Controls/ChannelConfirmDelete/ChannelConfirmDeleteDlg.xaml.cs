using System;
using System.Windows;
using System.Windows.Interop;
using ShipWorks.ApplicationCore.Licensing;
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
        /// Constructor
        /// </summary>
        public ChannelConfirmDeleteDlg(IWin32Window owner) : this()
        {
            Handle = owner.Handle;

            new WindowInteropHelper(this)
            {
                Owner = owner.Handle
            };
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
    }
}
