﻿using Interapptive.Shared.Net;
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
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IEncryptionProvider encryptionProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public SearsCredentials(IDateTimeProvider dateTimeProvider, IEncryptionProvider encryptionProvider)
        {
            MethodConditions.EnsureArgumentIsNotNull(dateTimeProvider);

            this.dateTimeProvider = dateTimeProvider;
            this.encryptionProvider = encryptionProvider;
        }

        /// <summary>
        /// Adds credentials to the request based on the store
        /// </summary>
        public void AddCredentials(SearsStoreEntity store, HttpVariableRequestSubmitter request)
        {
            GetCredentialsHttpVariables(store).ToList().ForEach(v => request.Variables.Add(v));
            AddCredentialsHeader(store, request);
        }

        /// <summary>
        /// Adds the credentials header to the request for authenticating
        /// </summary>
        private void AddCredentialsHeader(SearsStoreEntity store, HttpVariableRequestSubmitter request)
        {
            if (!string.IsNullOrEmpty(store.SellerID))
            {
                // They are using the current authentication method
                string timeStamp = dateTimeProvider.UtcNow.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'",
                    CultureInfo.InvariantCulture);
                string toHash = $"{store.SellerID}:{store.Email}:{timeStamp}";
                string signature = HashSignature(toHash, encryptionProvider.Decrypt(store.SecretKey));
                string headerValue =
                    $"HMAC-SHA256 emailaddress={store.Email},timestamp={timeStamp},signature={signature}";

                request.Headers.Add("authorization", headerValue);
            }
        }

        /// <summary>
        /// Hashes toHash using HMACSHA256 and secretKey
        /// </summary>
        private string HashSignature(string toHash, string secretKey)
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
        private static HttpVariableCollection GetCredentialsHttpVariables(SearsStoreEntity store)
        {
            HttpVariableCollection credentials = new HttpVariableCollection();

            if (string.IsNullOrEmpty(store.SellerID))
            {
                // They are using the "old" authentication method
                credentials.Add("email", store.Email);
                credentials.Add("password", SecureText.Decrypt(store.Password, store.Email));
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