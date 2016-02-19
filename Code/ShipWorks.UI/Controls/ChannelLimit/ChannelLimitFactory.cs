using System;
using Autofac.Features.Indexed;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.LicenseEnforcement;
using ShipWorks.Editions;
using ShipWorks.Stores;

namespace ShipWorks.UI.Controls.ChannelLimit
{
    /// <summary>
    /// Factory to create a ChannelLimitControl
    /// </summary>
    public class ChannelLimitFactory : IChannelLimitFactory
    {
        private readonly Func<IChannelLimitControl> channelLimitControlFactory;
        private readonly IChannelLimitViewModel viewModel;
        private readonly IIndex<EditionFeature, IChannelLimitBehavior> behaviorFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelLimitFactory"/> class.
        /// </summary>
        public ChannelLimitFactory(Func<IChannelLimitControl> channelLimitControlFactory,
            IChannelLimitViewModel viewModel,
            IIndex<EditionFeature, IChannelLimitBehavior> behaviorFactory)
        {
            this.channelLimitControlFactory = channelLimitControlFactory;
            this.viewModel = viewModel;
            this.behaviorFactory = behaviorFactory;
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
            viewModel.EnforcementContext = EnforcementContext.ExceedingChannelLimit;

            IChannelLimitControl channelLimitControl = channelLimitControlFactory();
            channelLimitControl.DataContext = viewModel;
            viewModel.Load(customerLicense, behaviorFactory[feature]);

            return channelLimitControl;
        }
    }
}
