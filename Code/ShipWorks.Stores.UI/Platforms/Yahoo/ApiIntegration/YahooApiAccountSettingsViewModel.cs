using System;
using System.ComponentModel;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration;
using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO;

namespace ShipWorks.Stores.UI.Platforms.Yahoo.ApiIntegration
{
    /// <summary>
    /// View model for the Yahoo Account Settings Page
    /// </summary>
    public class YahooApiAccountSettingsViewModel : YahooApiAccountViewModel, INotifyPropertyChanged
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="YahooApiAccountSettingsViewModel"/> class.
        /// </summary>
        /// <param name="storeWebClient">The store web client.</param>
        public YahooApiAccountSettingsViewModel(Func<YahooStoreEntity, IYahooApiWebClient> storeWebClient) : base(storeWebClient)
        {
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
        public new string Save(YahooStoreEntity store)
        {
            store.YahooStoreID = YahooStoreID;
            store.AccessToken = AccessToken;
            store.BackupOrderNumber = BackupOrderNumber;

            if (string.IsNullOrWhiteSpace(YahooStoreID))
            {
                return "Please enter your Store URL";
            }

            if (string.IsNullOrWhiteSpace(AccessToken))
            {
                return "Please enter your Access Token";
            }

            string error;

            try
            {
                YahooResponse response = StoreWebClient(new YahooStoreEntity() { YahooStoreID = YahooStoreID, AccessToken = AccessToken })
                    .ValidateCredentials();

                error = response.ErrorMessages == null ? string.Empty : CheckCredentialsError(response);
            }
            catch (Exception ex)
            {
                return $"Error connecting to Yahoo Api: {ex.Message}";
            }

            return error;
        }
    }
}
