using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.Cart66
{
    /// <summary>
    /// Store specific integration into ShipWorks
    /// </summary>
    public class Cart66LiteStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Cart66LiteStoreType(StoreEntity store) :
            base(store)
        {

        }

        /// <summary>
        /// StoreType enum value
        /// </summary>
        public override StoreTypeCode TypeCode
        {
            get
            {
                return StoreTypeCode.Cart66Lite;
            }
        }

        /// <summary>
        /// Log request/responses as Cart66 Lite
        /// </summary>
        public override ApiLogSource LogSource
        {
            get
            {
                return ApiLogSource.Cart66Lite;
            }
        }

        /// <summary>
        /// Gets the help URL to use in the setup wizard.
        /// </summary>
        public override string AccountSettingsHelpUrl
        {
            get { return "http://support.shipworks.com/solution/articles/4000022265-connecting-cart66-lite-or-pro-with"; }
        }
    }
}
