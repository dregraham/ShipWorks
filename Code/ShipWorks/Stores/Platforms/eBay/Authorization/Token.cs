using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using log4net;

using ShipWorks.Stores.Platforms.Ebay.Authorization.Readers;
using ShipWorks.Stores.Platforms.Ebay.Authorization.Repositories;
using ShipWorks.Stores.Platforms.Ebay.Authorization.Writers;

namespace ShipWorks.Stores.Platforms.Ebay.Authorization
{
    /// <summary>
    /// Class for managing the authorization token between eBay and ShipWorks.
    /// </summary>
    public class Token
    {
        static readonly ILog log = LogManager.GetLogger(typeof(Token));

        private ITokenFactory tokenFactory;
        private TokenData tokenData;
        private string license;

        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// </summary>
        /// <param name="license">The license.</param>
        public Token(string license)
            : this(license, new TokenData(), new EbayTokenFactory())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// </summary>
        /// <param name="license">The license.</param>
        /// <param name="tokenData">The token data.</param>
        public Token(string license, TokenData tokenData)
            : this(license, tokenData, new EbayTokenFactory())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// </summary>
        /// <param name="license">The license.</param>
        /// <param name="tokenFactory">The token factory.</param>
        public Token(string license, TokenData tokenData, ITokenFactory tokenFactory)
        {
            this.tokenData = tokenData; 
            
            this.tokenFactory = tokenFactory;
            this.license = license;
        }

        /// <summary>
        /// Gets the user ID.
        /// </summary>
        public string UserId 
        { 
            get { return tokenData.UserId; } 
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        public string Key
        {
            get { return tokenData.Key; }
        }

        /// <summary>
        /// Gets the expiration date.
        /// </summary>
        public DateTime ExpirationDate
        {
            get { return tokenData.ExpirationDate; }
        }

        /// <summary>
        /// Attempts to load/populate the token data using the repository designated
        /// by the token factory.
        /// </summary>
        /// <param name="license">The license.</param>
        public void Load()
        {
            ITokenRepository repository = tokenFactory.CreateRepository(license);
            string tokenDataFromRepository = repository.GetTokenData();

            ITokenReader reader = tokenFactory.CreateReader(tokenDataFromRepository);
            Load(reader);
        }

        /// <summary>
        /// Attempts to load/populate the token data from the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        public void Load(FileInfo file)
        {
            try
            {
                ITokenReader reader = tokenFactory.CreateReader(file);
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
        /// <param name="reader">The reader.</param>
        private void Load(ITokenReader reader)
        {
            TokenData tokenDataFromReader = reader.Read();
            this.tokenData = tokenDataFromReader;
        }

        /// <summary>
        /// Saves this token to the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        public void Save(FileInfo file)
        {
            ITokenWriter writer = tokenFactory.CreateWriter(file);
            writer.Write(this);
        }
    }
}
