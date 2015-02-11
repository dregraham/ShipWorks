﻿using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.CsCart
{
    /// <summary>
    /// Store specific integration into ShipWorks
    /// </summary>
    public class CsCartStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CsCartStoreType(StoreEntity store) :
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
                return StoreTypeCode.CsCart;
            }
        }

        /// <summary>
        /// Log request/responses as CsCart
        /// </summary>
        public override ApiLogSource LogSource
        {
            get
            {
                return ApiLogSource.CsCart;
            }
        }

        /// <summary>
        /// Gets the help URL to use in the account settings.
        /// </summary>
        public override string AccountSettingsHelpUrl
        {
            get { return "http://support.shipworks.com/solution/articles/4000042631-connecting-cs-cart"; }
        }
    }
}
