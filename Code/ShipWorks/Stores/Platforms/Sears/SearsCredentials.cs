using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ShipWorks.Stores.Platforms.Sears
{
    /// <summary>
    /// Provides credentials for making requests to the Sears api
    /// </summary>
    public class SearsCredentials
    {
        private readonly SearsStoreEntity store;
        private readonly HttpVariableRequestSubmitter request;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IEncryptionProviderFactory encryptionProviderFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public SearsCredentials(SearsStoreEntity store,
			HttpVariableRequestSubmitter request,
			IDateTimeProvider dateTimeProvider,
            IEncryptionProviderFactory encryptionProviderFactory)
        {
            MethodConditions.EnsureArgumentIsNotNull(store);
            MethodConditions.EnsureArgumentIsNotNull(request);
            MethodConditions.EnsureArgumentIsNotNull(dateTimeProvider);
            MethodConditions.EnsureArgumentIsNotNull(encryptionProviderFactory);

            this.store = store;
            this.request = request;
            this.dateTimeProvider = dateTimeProvider;
            this.encryptionProviderFactory = encryptionProviderFactory;
        }

        /// <summary>
        /// Adds credentials to the request based on the store
        /// </summary>
        public void AddCredentials()
        {
            HttpVariableCollection credentials = GetCredentialsHttpVariables();
            if (credentials.Any())
            {
                if (request.Verb == HttpVerb.Put)
                {
                    // we send the tracking xml as the body of the put message
                    // the uri needs to be modified to add the credentials
                    request.Uri = new Uri($"{request.Uri}?{QueryStringUtility.GetQueryString(credentials)}");
                }
                else
                {
                    credentials.ToList().ForEach(request.Variables.Add);
                }
            }

            AddCredentialsHeader();
        }

        /// <summary>
        /// Adds the credentials header to the request for authenticating
        /// </summary>
        private void AddCredentialsHeader()
        {
            if (!string.IsNullOrEmpty(store.SellerID))
            {
                // They are using the current authentication method
                string timeStamp = dateTimeProvider.UtcNow.AddMinutes(-15)
                                        .ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'", CultureInfo.InvariantCulture);

                string toHash = $"{store.SellerID}:{store.SearsEmail}:{timeStamp}";
                string signature;

                try
                {
                    IEncryptionProvider encryptionProvider = encryptionProviderFactory.CreateSearsEncryptionProvider();
                    string secretKey = encryptionProvider.Decrypt(store.SecretKey);

                    signature = HashSignature(toHash, secretKey);
                }
                catch (EncryptionException ex)
                {
                    throw new SearsException("An error occurred accessing your secret key. " +
                                             "Enter a new key in your store settings.", ex);
                }

                string headerValue = $"HMAC-SHA256 emailaddress={store.SearsEmail},timestamp={timeStamp},signature={signature}";
                request.Headers.Add("authorization", headerValue);
            }
        }

        /// <summary>
        /// Hashes toHash using HMACSHA256 and secretKey
        /// </summary>
        private static string HashSignature(string toHash, string secretKey)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(secretKey);
            byte[] messageBytes = encoding.GetBytes(toHash);

            using (HMACSHA256 hmacSha256 = new HMACSHA256(keyByte))
            {
                byte[] hash = hmacSha256.ComputeHash(messageBytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }


        /// <summary>
        /// Return the collection of HTTP variables for authenticating
        /// </summary>
        private HttpVariableCollection GetCredentialsHttpVariables()
        {
            HttpVariableCollection credentials = new HttpVariableCollection();

            if (string.IsNullOrEmpty(store.SellerID))
            {
                // They are using the "old" authentication method
                IEncryptionProvider secureTextEncryptionProvider = encryptionProviderFactory.CreateSecureTextEncryptionProvider(store.SearsEmail);

                credentials.Add("email", store.SearsEmail);
                credentials.Add("password", secureTextEncryptionProvider.Decrypt(store.Password));
            }
            else
            {
                // They are using the current authentication method
                credentials.Add("sellerId", store.SellerID);
            }

            return credentials;
        }
    }
}