using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.ApplicationCore.Logging;

namespace ShipWorks.Stores.Platforms.VirtueMart
{
    /// <summary>
    /// VirtueMart integration
    /// </summary>
    class VirtueMartStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public VirtueMartStoreType(StoreEntity store)
            : base(store)
        {

        }

        /// <summary>
        /// Store Type
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.VirtueMart;

        /// <summary>
        /// Log request/responses as VirtueMart
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.VirtueMart;

        /// <summary>
        /// Gets or sets the account settings help URL.
        /// </summary>
        /// <value>
        /// The account settings help URL.
        /// </value>
        public override string AccountSettingsHelpUrl => "http://support.shipworks.com/solution/articles/4000065052";
    }
}
