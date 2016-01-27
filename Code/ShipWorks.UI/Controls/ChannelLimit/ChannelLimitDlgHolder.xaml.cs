using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ShipWorks.ApplicationCore.Licensing;

namespace ShipWorks.UI.Controls.ChannelLimit
{
    /// <summary>
    /// Interaction logic for ChannelLimitDlg.xaml
    /// </summary>
    public partial class ChannelLimitDlg : Window
    {
        private readonly ChannelLimitViewModel viewModel;

        public ChannelLimitDlg()
        {
            InitializeComponent();
        }

        public ChannelLimitDlg(ChannelLimitViewModel viewModel) : this()
        {
            this.viewModel = viewModel;
            ChannelLimit.DataContext = viewModel;
            viewModel.Load();
        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            viewModel.Dismiss();
        }
    }
}
