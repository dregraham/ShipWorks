using System;
using System.Text.RegularExpressions;
using Common.Logging;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Security;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.BigCommerce
{
    /// <summary>
    /// Allows interaction with the BigCommerce identifier
    /// </summary>
    [Component]
    public class BigCommerceIdentifier : IBigCommerceIdentifier
    {
        readonly IDatabaseSpecificEncryptionProvider encryptionProvider;
        readonly ISqlAdapterFactory sqlAdapterFactory;
        static readonly ILog log = LogManager.GetLogger(typeof(BigCommerceIdentifier));

        /// <summary>
        /// Constructor
        /// </summary>
        public BigCommerceIdentifier(IDatabaseSpecificEncryptionProvider encryptionProvider, ISqlAdapterFactory sqlAdapterFactory)
        {
            this.encryptionProvider = encryptionProvider;
            this.sqlAdapterFactory = sqlAdapterFactory;
        }

        /// <summary>
        /// Get the identifier from the given store
        /// </summary>
        public string Get(IBigCommerceStoreEntity typedStore)
        {
            string identifier;
            try
            {
                identifier = encryptionProvider.Decrypt(typedStore.Identifier);
            }
            catch (EncryptionException)
            {
                // If the identifier can't be decrypted, try generating a new one (See TP #31374 / Zendesk #7732)
                log.Info("Invalid BigCommerce identifier. Generating new identifier.");

                BigCommerceStoreEntity store = (BigCommerceStoreEntity) typedStore;
                store.Identifier = string.Empty;
                Set(store);

                using (ISqlAdapter sqlAdapter = sqlAdapterFactory.CreateTransacted())
                {
                    sqlAdapter.SaveEntity(store, true);
                    sqlAdapter.Commit();
                }

                identifier = encryptionProvider.Decrypt(typedStore.Identifier);
            }
            return identifier;
        }

        /// <summary>
        /// Set the identifier on the given store
        /// </summary>
        public BigCommerceStoreEntity Set(BigCommerceStoreEntity store)
        {
            if (string.IsNullOrWhiteSpace(store.Identifier))
            {
                string cleansedIdentifier = CleanseIdentifier(store.ApiUrl);
                store.Identifier = encryptionProvider.Encrypt(cleansedIdentifier);
            }

            return store;
        }

        /// <summary>
        /// Translate the license URL
        /// </summary>
        private string CleanseIdentifier(string originalIdentifier)
        {
            string identifier = originalIdentifier;

            int indexOfApiPath = identifier.IndexOf("api/v2/", 0, StringComparison.OrdinalIgnoreCase);
            if (indexOfApiPath > 0)
            {
                identifier = identifier.Substring(0, indexOfApiPath);
            }

            identifier = Regex.Replace(identifier, @"(admin/)?[^/]*(\.)?[^/]+$", "", RegexOptions.IgnoreCase);

            // The regex above will return just the scheme if there's no ending /, so check for that and
            // reset to the StoreUrl
            if (identifier.EndsWith(Uri.SchemeDelimiter, StringComparison.OrdinalIgnoreCase))
            {
                identifier = originalIdentifier;
            }

            return identifier;
        }
    }
}
