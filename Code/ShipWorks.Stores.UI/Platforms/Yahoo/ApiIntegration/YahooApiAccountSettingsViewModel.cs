using System;
using System.ComponentModel;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration;

namespace ShipWorks.Stores.UI.Platforms.Yahoo.ApiIntegration
{
    public class YahooApiAccountSettingsViewModel : YahooApiAccountViewModel, INotifyPropertyChanged
    {
       public YahooApiAccountSettingsViewModel(Func<YahooStoreEntity, IYahooApiWebClient> storeWebClient) : base(storeWebClient)
        {
        }

        public void Load(YahooStoreEntity storeEntity)
        {
            YahooStoreID = storeEntity.YahooStoreID;
            AccessToken = storeEntity.AccessToken;
            BackupOrderNumber = storeEntity.BackupOrderNumber;

            ValidateBackupOrderNumber();
        }

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
            }

            return string.Empty;
        }
    }
}
