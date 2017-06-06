using System;
using System.Text.RegularExpressions;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Security;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.ShopSite
{
    /// <summary>
    /// Allows interaction with the ShopSite identifier
    /// </summary>
    [Component]
    public class ShopSiteIdentifier : IShopSiteIdentifier
    {
        readonly IDatabaseSpecificEncryptionProvider encryptionProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopSiteIdentifier(IDatabaseSpecificEncryptionProvider encryptionProvider)
        {
            this.encryptionProvider = encryptionProvider;
        }

        /// <summary>
        /// Get the identifier from the given store
        /// </summary>
        public string Get(IShopSiteStoreEntity typedStore)
        {
            return encryptionProvider.Decrypt(typedStore.Identifier);
        }

        /// <summary>
        /// Set the identifier on the given store
        /// </summary>
        public ShopSiteStoreEntity Set(ShopSiteStoreEntity store, string apiUrl)
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
            string cleansedIdentifier = originalIdentifier.ToLowerInvariant();

            // Remove the db_xml.cgi part (for existing stores)
            cleansedIdentifier = cleansedIdentifier.Replace("db_xml.cgi", "");

            // Remove the authorize.cgi part (for new OAuth stores)
            cleansedIdentifier = cleansedIdentifier.Replace("authorize.cgi", "");

            return cleansedIdentifier;
        }
    }
}
