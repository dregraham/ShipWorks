using System;
using ShipWorks.ApplicationCore.Licensing;

namespace ShipWorks.UI.Controls.ChannelLimit
{
    public class ChannelLimitDlgFactory : IChannelLimitDlgFactory
    {
        private readonly Func<IChannelLimitDlg> dlgFactory;
        private readonly IChannelLimitViewModel viewModel;

        public ChannelLimitDlgFactory(Func<IChannelLimitDlg> dlgFactory, IChannelLimitViewModel viewModel)
        {
            this.dlgFactory = dlgFactory;
            this.viewModel = viewModel;
        }

        public IChannelLimitDlg GetChannelLimitDlg(ICustomerLicense customerLicense)
        {
            // load the customer license into the view model
            viewModel.Load(customerLicense);

            // Get the dialog
            IChannelLimitDlg dialog = dlgFactory();
            dialog.DataContext = viewModel;

            return dialog;
        }
    }
}