﻿using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.Cart66
{
    /// <summary>
    /// Store specific integration into ShipWorks
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.Cart66Lite)]
    [Component(RegistrationType.Self)]
    public class Cart66LiteStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Cart66LiteStoreType(StoreEntity store, IMessageHelper messageHelper, IOrderManager orderManager)
            : base(store, messageHelper, orderManager)
        {

        }

        /// <summary>
        /// StoreType enum value
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.Cart66Lite;

        /// <summary>
        /// Log request/responses as Cart66 Lite
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.Cart66Lite;

        /// <summary>
        /// Gets the help URL to use in the setup wizard.
        /// </summary>
        public override string AccountSettingsHelpUrl => "https://shipworks.zendesk.com/hc/en-us/articles/360022464992";
    }
}
