using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.ApplicationCore.Logging;

namespace ShipWorks.Stores.Platforms.XCart
{
    /// <summary>
    /// X-Cart integration point
    /// </summary>
    class XCartStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public XCartStoreType(StoreEntity store) : base(store)
        {
        }

        /// <summary>
        /// Gets the code to identify this store as XCart
        /// </summary>
        public override StoreTypeCode TypeCode
        {
            get { return StoreTypeCode.XCart; }
        }

        /// <summary>
        /// Log request/responses as XCart
        /// </summary>
        public override ApiLogSource LogSource
        {
            get
            {
                return ApiLogSource.XCart;
            }
        }

        /// <summary>
        /// Gets the account settings help URL.
        /// </summary>
        public override string AccountSettingsHelpUrl
        {
            get { return "http://support.shipworks.com/support/solutions/articles/4000029976"; }
        }
    }
}
