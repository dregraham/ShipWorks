using System;
using System.ComponentModel;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration;

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
            string error = base.Save(store);

            if (!string.IsNullOrWhiteSpace(error))
            {
                return error;
            }

            switch (IsValid)
            {
                case YahooOrderNumberValidation.Validating:
                    return "Please wait while ShipWorks validates the order number you entered";
                case YahooOrderNumberValidation.Invalid:
                    return "The order number you entered does not exist for this Yahoo store. Please enter an existing order number to start downloading from.";
                default:
                    return string.Empty;
            }
        }
    }
}
