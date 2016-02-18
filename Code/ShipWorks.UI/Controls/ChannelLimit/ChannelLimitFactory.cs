using System;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Editions;
using ShipWorks.Stores;

namespace ShipWorks.UI.Controls.ChannelLimit
{
    /// <summary>
    /// Factory to create a ChannelLimitControl
    /// </summary>
    public class ChannelLimitFactory : IChannelLimitFactory
    {
        private readonly Func<ChannelLimitControl> channelLimitControlFactory;
        private readonly IChannelLimitViewModel viewModel;
        private readonly Func<EditionFeature, IChannelLimitBehavior> behaviorFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelLimitFactory"/> class.
        /// </summary>
        public ChannelLimitFactory(Func<ChannelLimitControl> channelLimitControlFactory,
            IChannelLimitViewModel viewModel,
            Func<EditionFeature, IChannelLimitBehavior> behaviorFactory)
        {
            this.channelLimitControlFactory = channelLimitControlFactory;
            this.viewModel = viewModel;
            this.behaviorFactory = behaviorFactory;
        }

        /// <summary>
        /// Creates the ChannelLimitControl.
        /// </summary>
        public IChannelLimitControl CreateControl(ICustomerLicense customerLicense, EditionFeature feature)
        {
            ChannelLimitControl channelLimitControl = channelLimitControlFactory();
            channelLimitControl.DataContext = viewModel;
            viewModel.Load(customerLicense, behaviorFactory(feature));

            return channelLimitControl;
        }

        /// <summary>
        /// Creates the control
        /// </summary>
        /// <remarks>
        /// This instance will take the store being added into account
        /// It shouldn't be available to delete and be counted against the customer's limit.
        /// </remarks>
        public IChannelLimitControl CreateControl(ICustomerLicense customerLicense, StoreTypeCode channelToAdd, EditionFeature feature)
        {
            viewModel.ChannelToAdd = channelToAdd;
            return CreateControl(customerLicense, feature);
        }
    }
}
