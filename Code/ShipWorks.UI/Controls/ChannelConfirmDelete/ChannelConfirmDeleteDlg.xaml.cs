using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ShipWorks.UI.Controls.ChannelLimit;

namespace ShipWorks.UI.Controls.ChannelConfirmDelete
{
    /// <summary>
    /// Interaction logic for ChannelConfirmDeleteDlg.xaml
    /// </summary>
    public partial class ChannelConfirmDeleteDlg : Window, IChannelConfirmDeleteDlg
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
            new WindowInteropHelper(this)
            {
                Owner = owner.Handle
            };
        }

        /// <summary>
        /// The user clicked delete
        /// </summary>
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        /// <summary>
        /// The user clicked cancel
        /// </summary>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
