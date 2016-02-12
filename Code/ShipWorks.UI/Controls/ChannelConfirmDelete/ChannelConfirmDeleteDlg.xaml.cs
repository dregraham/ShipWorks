using System.Windows;

namespace ShipWorks.UI.Controls.ChannelConfirmDelete
{
    /// <summary>
    /// Interaction logic for ChannelConfirmDeleteDlg.xaml
    /// </summary>
    public partial class ChannelConfirmDeleteDlg : IChannelConfirmDeleteDlg
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelConfirmDeleteDlg()
        {
            InitializeComponent();
        }

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
