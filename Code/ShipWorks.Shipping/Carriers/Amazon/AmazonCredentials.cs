﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.Api;
using ShipWorks.Stores;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Model for working with Amazon shipping credentials
    /// </summary>
    public class AmazonCredentials : IAmazonCredentials, INotifyPropertyChanged
    {
        private readonly PropertyChangedHandler handler;
        private readonly IAmazonShippingWebClient webClient;
        private readonly IStoreManager storeManager;

        private string merchantId;
        private string authToken;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonCredentials(IAmazonShippingWebClient webClient, IStoreManager storeManager)
        {
            handler = new PropertyChangedHandler(PropertyChanged);

            this.webClient = webClient;
            this.storeManager = storeManager;
        }

        /// <summary>
        /// Initialize the class
        /// </summary>
        public void PopulateFromStore()
        {
            List<AmazonStoreEntity> stores = storeManager.GetAllStores()
                .OfType<AmazonStoreEntity>()
                .Where(x => x.Enabled)
                .GroupBy(x => new { x.MerchantID, x.AuthToken })
                .Select(x => x.FirstOrDefault())
                .ToList();

            AmazonStoreEntity store = stores.Count == 1 ? stores[0] : null;

            MerchantId = store != null ? store.MerchantID : string.Empty;
            AuthToken = store != null ? store.AuthToken : string.Empty;
        }

        /// <summary>
        /// A property has changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Amazon account merchant id
        /// </summary>
        public string MerchantId
        {
            get { return merchantId; }
            set { handler.Set(() => MerchantId, ref merchantId, value); }
        }

        /// <summary>
        /// Amazon account authentication token
        /// </summary>
        public string AuthToken
        {
            get { return authToken; }
            set { handler.Set(() => AuthToken, ref authToken, value); }
        }

        /// <summary>
        /// Was the validation successful
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Message from result of validation
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Validate the credentials
        /// </summary>
        public void Validate()
        {
            if (!string.IsNullOrWhiteSpace(MerchantId) && !string.IsNullOrWhiteSpace(AuthToken))
            {
                AmazonValidateCredentialsResponse response = webClient.ValidateCredentials(MerchantId, AuthToken);

                Success = response.Success;
                Message = response.Message;
            }
            else
            {
                Success = false;
                Message = "MerchantId and AuthToken are required";
            }
        }

        /// <summary>
        /// Populate the given account with the credential data
        /// </summary>
        public void PopulateAccount(AmazonAccountEntity account)
        {
            MethodConditions.EnsureArgumentIsNotNull(account, nameof(account));

            if (!Success)
            {
                throw new InvalidOperationException("Cannot update account before credentials are validated");
            }

            account.MerchantID = MerchantId;
            account.AuthToken = AuthToken;
        }
    }
}
