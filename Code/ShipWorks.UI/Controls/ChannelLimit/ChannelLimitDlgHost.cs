using System;
using System.Windows.Forms;
using ShipWorks.ApplicationCore.Licensing;

namespace ShipWorks.UI.Controls.ChannelLimit
{
    /// <summary>
    /// Winforms element for hosting the WPF ChannelLimitDlg
    /// </summary>
    public partial class ChannelLimitDlgHost : Form, IChannelLimitDlgHost
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelLimitDlgHost(ChannelLimitViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
            Load += OnDlgLoad;
            Closed += OnDlgClose;
        }

        /// <summary>
        /// Called when [dialog close].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnDlgClose(object sender, EventArgs e)
        {
            ViewModel.Dismiss();
        }

        /// <summary>
        /// Called when [dialog load].
        /// </summary>
        private void OnDlgLoad(object sender, EventArgs e)
        {
            ViewModel.Load();

            ChannelLimitDlg page = new ChannelLimitDlg
            {
                DataContext = ViewModel
            };

            elementHost.Child = page;

            ViewModel.Load();
        }

        /// <summary>
        /// The ViewModel
        /// </summary>
        public ChannelLimitViewModel ViewModel { get; }
    }
}
