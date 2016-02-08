using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ShipWorks.ApplicationCore.Licensing;

namespace ShipWorks.UI.Controls.ChannelLimit
{
    /// <summary>
    /// Factory to create a ChannelLimitControl
    /// </summary>
    public class ChannelLimitFactory : IChannelLimitFactory
    {
        private readonly Func<ChannelLimitControl> channelLimitControlFactory;
        private readonly IChannelLimitViewModel viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelLimitFactory"/> class.
        /// </summary>
        public ChannelLimitFactory(Func<ChannelLimitControl> channelLimitControlFactory, IChannelLimitViewModel viewModel)
        {
            this.channelLimitControlFactory = channelLimitControlFactory;
            this.viewModel = viewModel;
        }

        /// <summary>
        /// Creates the ChannelLimitControl.
        /// </summary>
        public IChannelLimitControl CreateControl(ICustomerLicense customerLicense)
        {
            ChannelLimitControl channelLimitControl = channelLimitControlFactory();
            channelLimitControl.DataContext = viewModel;
            viewModel.Load(customerLicense);

            return channelLimitControl;
        }
    }
}
