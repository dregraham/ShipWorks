﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.SureDone
{
    /// <summary>
    /// Store specific integration into ShipWorks
    /// </summary>
    public class SureDoneStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SureDoneStoreType(StoreEntity store)
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
                return StoreTypeCode.SureDone;
            }
        }

        /// <summary>
        /// Log request/responses as SureDone
        /// </summary>
        public override ApiLogSource LogSource   
        {
            get
            {
                return ApiLogSource.SureDone;
            }
        }

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
                GenericModuleStoreEntity genericStore = (GenericModuleStoreEntity)Store;

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
    }
}
