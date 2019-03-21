﻿using System;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.SureDone
{
    /// <summary>
    /// Store specific integration into ShipWorks
    /// </summary>
    [SuppressMessage("CSharp.Analyzers",
        "CA5351: Do not use insecure cryptographic algorithm MD5",
        Justification = "This is what SureDone currently uses")]
    [KeyedComponent(typeof(StoreType), StoreTypeCode.SureDone)]
    [Component(RegistrationType.Self)]
    public class SureDoneStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SureDoneStoreType(StoreEntity store, IMessageHelper messageHelper, IOrderManager orderManager) :
            base(store, messageHelper, orderManager)
        {
        }

        /// <summary>
        /// StoreType enum value
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.SureDone;

        /// <summary>
        /// Log request/responses as SureDone
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.SureDone;

        /// <summary>
        /// Return value that uniquely identifies this store instance
        ///
        /// The only unique thing in the module url is the value of the token param.
        /// So we will get that value, hash it, and return a Base64 string of it.
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {
                GenericModuleStoreEntity genericStore = (GenericModuleStoreEntity) Store;

                string moduleUrl = genericStore.ModuleUrl;
                Uri moduleUri = new Uri(moduleUrl);

                // Parse the query string variables into a NameValueCollection.
                NameValueCollection queryStringParams = HttpUtility.ParseQueryString(moduleUri.Query);
                string token = queryStringParams["token"];

                // We can't use the actual token, because its secure.
                byte[] bytes = Encoding.UTF8.GetBytes(token);

                using (MD5 md5 = new MD5CryptoServiceProvider())
                {
                    // Generate the hash
                    return Convert.ToBase64String(md5.ComputeHash(bytes));
                }
            }
        }

        /// <summary>
        /// Gets the help URL to use in the account settings.
        /// </summary>
        public override string AccountSettingsHelpUrl => "https://shipworks.zendesk.com/hc/en-us/articles/360022466492";
    }
}
