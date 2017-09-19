using System;
using System.Text.RegularExpressions;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.SolidCommerce
{
    /// <summary>
    /// Store specific integration into ShipWorks
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.SolidCommerce)]
    [Component(RegistrationType.Self)]
    public class SolidCommerceStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SolidCommerceStoreType(StoreEntity store, IMessageHelper messageHelper, IOrderManager orderManager) :
            base(store, messageHelper, orderManager)
        {
        }

        /// <summary>
        /// StoreType enum value
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.SolidCommerce;

        /// <summary>
        /// Log request/responses as CreLoaded
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.SolidCommerce;

        /// <summary>
        /// Gets the help URL to use in the account settings.
        /// </summary>
        public override string AccountSettingsHelpUrl => "http://support.shipworks.com/support/solutions/articles/4000019078";

        /// <summary>
        /// Identifies this store type
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {
                GenericModuleStoreEntity genericStore = (GenericModuleStoreEntity) Store;

                string moduleUrl = genericStore.ModuleUrl;

                Uri moduleUri;
                if (Uri.TryCreate(moduleUrl, UriKind.Absolute, out moduleUri))
                {
                    moduleUrl = moduleUri.AbsoluteUri;
                }

                string identifier = moduleUrl.ToLowerInvariant();

                // Remove any "?querystring"
                identifier = Regex.Replace(identifier, @"(\?.*)", "", RegexOptions.IgnoreCase);

                // Remove final "/"
                identifier = Regex.Replace(identifier, @"/$", "", RegexOptions.IgnoreCase);

                return identifier;
            }
        }
    }
}
