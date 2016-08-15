using System;
using log4net;
using ShipWorks.Editions;
using ShipWorks.Stores;

namespace ShipWorks.ApplicationCore.Licensing.LicenseEnforcement
{
    /// <summary>
    /// Ensure that, if customer has ODBC installed, it is allowed by tango
    /// </summary>
    public class OdbcStoreTypeEnforcer: ChannelTypeEnforcer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcStoreTypeEnforcer"/> class.
        /// </summary>
        public OdbcStoreTypeEnforcer(IChannelLimitDlgFactory channelLimitDlgFactory,
            Func<Type, ILog> logFactory,
            IStoreManager storeManager)
            : base(channelLimitDlgFactory, logFactory(typeof (OdbcStoreTypeEnforcer)), storeManager)
        {
        }

        /// <summary>
        /// The edition feature enforced
        /// </summary>
        public override EditionFeature EditionFeature => EditionFeature.Odbc;

        /// <summary>
        /// Gets the store type code.
        /// </summary>
        protected override StoreTypeCode StoreTypeCode => StoreTypeCode.Odbc;
    }
}