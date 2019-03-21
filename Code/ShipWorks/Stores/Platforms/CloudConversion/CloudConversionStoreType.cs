﻿using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.CloudConversion
{
    /// <summary>
    /// Cloud Conversion StoreType
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.CloudConversion)]
    [Component(RegistrationType.Self)]
    public class CloudConversionStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CloudConversionStoreType"/> class.
        /// </summary>
        /// <param name="store"></param>
        public CloudConversionStoreType(StoreEntity store, IMessageHelper messageHelper, IOrderManager orderManager)
            : base(store, messageHelper, orderManager)
        {
        }

        /// <summary>
        /// The numeric type code of the store.
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.CloudConversion;


        /// <summary>
        /// Gets the log source.
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.CloudConversion;

        /// <summary>
        /// Gets the help URL to use in the account settings.
        /// </summary>
        public override string AccountSettingsHelpUrl => "https://shipworks.zendesk.com/hc/en-us/articles/360022465152";
    }
}
