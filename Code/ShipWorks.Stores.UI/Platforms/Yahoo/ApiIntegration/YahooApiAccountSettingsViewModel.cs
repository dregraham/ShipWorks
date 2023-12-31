﻿using System;
using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Yahoo;
using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration;
using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO;

namespace ShipWorks.Stores.UI.Platforms.Yahoo.ApiIntegration
{
    /// <summary>
    /// View model for the Yahoo Account Settings Page
    /// </summary>
    [Component(RegistrationType.Self)]
    public class YahooApiAccountSettingsViewModel : YahooApiAccountViewModel, INotifyPropertyChanged
    {
        private readonly Func<Type, ILog> logFactory;
        private string helpUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="YahooApiAccountSettingsViewModel"/> class.
        /// </summary>
        /// <param name="storeTypeManager"></param>
        /// <param name="storeWebClient">The store web client.</param>
        public YahooApiAccountSettingsViewModel(IStoreTypeManager storeTypeManager, IYahooApiWebClient webClient, Func<Type, ILog> logFactory) :
            base(webClient, logFactory)
        {
            this.logFactory = logFactory;
            YahooStoreType storeType = storeTypeManager.GetType(StoreTypeCode.Yahoo) as YahooStoreType;
            HelpUrl = storeType?.InvalidAccessTokenHelpUrl;
        }

        /// <summary>
        /// Help link URL
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string HelpUrl
        {
            get { return helpUrl; }
            set { Handler.Set(nameof(HelpUrl), ref helpUrl, value); }
        }

        /// <summary>
        /// Loads the page
        /// </summary>
        /// <param name="storeEntity">The store entity.</param>
        public void Load(YahooStoreEntity storeEntity)
        {
            YahooStoreID = storeEntity.YahooStoreID;
            AccessToken = storeEntity.AccessToken;
            BackupOrderNumber = storeEntity.BackupOrderNumber;

            HandleChanges();
        }

        /// <summary>
        /// If the account information entered is valid, an empty string is returned.
        /// If errors occur while validating the information, return the error message.
        /// </summary>
        /// <param name="store">The store.</param>
        public override string Save(YahooStoreEntity store)
        {
            store.YahooStoreID = YahooStoreID?.Trim() ?? string.Empty;
            store.AccessToken = AccessToken?.Trim() ?? string.Empty;
            store.BackupOrderNumber = BackupOrderNumber;

            if (string.IsNullOrWhiteSpace(YahooStoreID))
            {
                return "Please enter your Yahoo Store ID";
            }

            if (string.IsNullOrWhiteSpace(AccessToken))
            {
                return "Please enter your Access Token";
            }

            if (BackupOrderNumber != null && BackupOrderNumber < 0)
            {
                return "Yahoo only supports positive, numeric order numbers";
            }

            try
            {
                YahooResponse response = webClient.ValidateCredentials(store);

                string error = CheckCredentialsError(response);

                // If the error message matches this one, then we know the credentials are good.
                return error.Equals("StatusID needs to be specified in correct format",
                    StringComparison.InvariantCultureIgnoreCase) ?
                    string.Empty :
                    error;
            }
            catch (Exception ex)
            {
                return $"Error connecting to Yahoo Api: {ex.Message}";
            }
        }
    }
}
