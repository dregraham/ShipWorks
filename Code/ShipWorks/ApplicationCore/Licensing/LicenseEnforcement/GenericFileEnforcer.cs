using System;
using log4net;
using ShipWorks.Editions;
using ShipWorks.Stores;

namespace ShipWorks.ApplicationCore.Licensing.LicenseEnforcement
{
    /// <summary>
    /// Ensure that, if customer has GenericFile installed, it is allowed by tango
    /// </summary>
    public class GenericFileEnforcer : ChannelTypeEnforcer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericFileEnforcer"/> class.
        /// </summary>
        public GenericFileEnforcer(IChannelLimitDlgFactory channelLimitDlgFactory,
            Func<Type, ILog> logFactory,
            IStoreManager storeManager)
            : base(channelLimitDlgFactory, logFactory(typeof (GenericFileEnforcer)), storeManager)
        {
        }

        /// <summary>
        /// The edition feature enforced
        /// </summary>
        public override EditionFeature EditionFeature => EditionFeature.GenericFile;

        /// <summary>
        /// Gets the store type code.
        /// </summary>
        protected override StoreTypeCode StoreTypeCode => StoreTypeCode.GenericFile;
    }
}