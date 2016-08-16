using System;
using log4net;
using ShipWorks.Editions;
using ShipWorks.Stores;

namespace ShipWorks.ApplicationCore.Licensing.LicenseEnforcement
{
    /// <summary>
    /// Ensure that, if customer has a GenericModule installed, it is allowed by tango
    /// </summary>
    public class GenericModuleEnforcer : ChannelTypeEnforcer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericModuleEnforcer"/> class.
        /// </summary>
        public GenericModuleEnforcer(IChannelLimitDlgFactory channelLimitDlgFactory,
            Func<Type, ILog> logFactory,
            IStoreManager storeManager)
            : base(channelLimitDlgFactory, logFactory(typeof (GenericModuleEnforcer)), storeManager)
        {
        }

        /// <summary>
        /// The edition feature enforced
        /// </summary>
        public override EditionFeature EditionFeature => EditionFeature.GenericModule;

        /// <summary>
        /// Gets the store type code.
        /// </summary>
        protected override StoreTypeCode StoreTypeCode => StoreTypeCode.GenericModule;
    }
}