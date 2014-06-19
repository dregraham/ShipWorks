using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.SolidCommerce
{
    /// <summary>
    /// Store specific integration into ShipWorks
    /// </summary>
    public class SolidCommerceStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SolidCommerceStoreType(StoreEntity store)
            : base(store)
        {
        }

        /// <summary>
        /// StoreType enum value
        /// </summary>
        public override StoreTypeCode TypeCode
        {
            get
            {
                return StoreTypeCode.SolidCommerce;
            }
        }

        /// <summary>
        /// Log request/responses as CreLoaded
        /// </summary>
        public override ApiLogSource LogSource
        {
            get
            {
                return ApiLogSource.SolidCommerce;
            }
        }

        /// <summary>
        /// Identifies this store type
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {
                GenericModuleStoreEntity genericStore = (GenericModuleStoreEntity)Store;

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
