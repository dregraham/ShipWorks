﻿using System;
using System.Text.RegularExpressions;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.StageBloc
{
    /// <summary>
    /// Store specific integration into ShipWorks
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.StageBloc)]
    [Component(RegistrationType.Self)]
    public class StageBlocStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StageBlocStoreType(StoreEntity store, IMessageHelper messageHelper, IOrderManager orderManager) :
            base(store, messageHelper, orderManager)
        {

        }

        /// <summary>
        /// StoreType enum value
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.StageBloc;

        /// <summary>
        /// Log request/responses as StageBloc
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.StageBloc;

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

        /// <summary>
        /// Gets the help URL to use in the account settings.
        /// </summary>
        public override string AccountSettingsHelpUrl => "https://shipworks.zendesk.com/hc/en-us/articles/360022654471";
    }
}
