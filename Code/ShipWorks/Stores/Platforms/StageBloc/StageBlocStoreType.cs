using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.StageBloc
{
    /// <summary>
    /// Store specific integration into ShipWorks
    /// </summary>
    public class StageBlocStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StageBlocStoreType(StoreEntity store) :
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
                return StoreTypeCode.StageBloc;
            }
        }

        /// <summary>
        /// Log request/responses as StageBloc
        /// </summary>
        public override ApiLogSource LogSource
        {
            get
            {
                return ApiLogSource.StageBloc;
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

        /// <summary>
        /// Gets the help URL to use in the account settings.
        /// </summary>
        public override string AccountSettingsHelpUrl
        {
            get { return "http://support.shipworks.com/support/solutions/articles/4000045732-connecting-stagebloc-with-shipworks"; }
        }
    }
}
