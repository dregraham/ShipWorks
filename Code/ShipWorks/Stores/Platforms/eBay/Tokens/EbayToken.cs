using System;
using System.IO;
using log4net;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.Ebay.Tokens.Readers;
using ShipWorks.Stores.Platforms.Ebay.Tokens.Writers;

namespace ShipWorks.Stores.Platforms.Ebay.Tokens
{
    /// <summary>
    /// Class for managing the authorization token between eBay and ShipWorks.
    /// </summary>
    public class EbayToken
    {
        static readonly ILog log = LogManager.GetLogger(typeof(EbayToken));

        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// </summary>
        public EbayToken()
        {

        }

        /// <summary>
        /// Create a new instance based on the data in the given store
        /// </summary>
        public static EbayToken FromStore(IEbayStoreEntity store)
        {
            return new EbayToken
            {
                Token = store.EBayToken,
                UserId = store.EBayUserID,
                ExpirationDate = store.EBayTokenExpire
            };
        }

        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        /// <value>
        /// The user ID.
        /// </value>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the expiration date.
        /// </summary>
        /// <value>
        /// The expiration date.
        /// </value>
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// Attempts to load/populate the token data from the specified file.
        /// </summary>
        public void Load(FileInfo file)
        {
            try
            {
                ITokenReader reader = new EbayTokenFactory().CreateReader(file);
                Load(reader);
            }
            catch (EbayException ex)
            {
                log.Error(ex.Message, ex);
                throw;
            }

        }

        /// <summary>
        /// Attempts to load/populate the token data from the specified reader.
        /// </summary>
        private void Load(ITokenReader reader)
        {
            EbayToken token = reader.Read();

            this.Token = token.Token;
            this.UserId = token.UserId;
            this.ExpirationDate = token.ExpirationDate;
        }

        /// <summary>
        /// Saves this token to the specified file.
        /// </summary>
        public void Save(FileInfo file)
        {
            ITokenWriter writer = new EbayTokenFactory().CreateWriter(file);
            writer.Write(this);
        }
    }
}
