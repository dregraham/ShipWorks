using System;
using System.ComponentModel;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reflection;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration;
using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO;

namespace ShipWorks.Stores.UI.Platforms.Yahoo.ApiIntegration
{
    public class YahooApiAccountViewModel
    {
        private readonly Func<YahooStoreEntity, IYahooApiWebClient> storeWebClient;
        protected readonly PropertyChangedHandler handler;
        public event PropertyChangedEventHandler PropertyChanged;
        private string yahooStoreID;
        private string accessToken;
        private long? backupOrderNumber;
        private YahooOrderNumberValidation isValid;

        public YahooApiAccountViewModel(Func<YahooStoreEntity, IYahooApiWebClient> storeWebClient)
        {
            this.storeWebClient = storeWebClient;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        [Obfuscation(Exclude = true)]
        public string YahooStoreID
        {
            get { return yahooStoreID; }
            set { handler.Set(nameof(YahooStoreID), ref yahooStoreID, value); }
        }

        [Obfuscation(Exclude = true)]
        public string AccessToken
        {
            get { return accessToken; }
            set { handler.Set(nameof(AccessToken), ref accessToken, value); }
        }

        [Obfuscation(Exclude = true)]
        public long? BackupOrderNumber
        {
            get { return backupOrderNumber; }
            set { handler.Set(nameof(BackupOrderNumber), ref backupOrderNumber, value); }
        }

        [Obfuscation(Exclude = true)]
        public YahooOrderNumberValidation IsValid
        {
            get { return isValid; }
            set { handler.Set(nameof(IsValid), ref isValid, value); }
        }

        private bool IsValidationPropertyEntered(string propertyName)
        {
            return !string.IsNullOrWhiteSpace(YahooStoreID) &&
                !string.IsNullOrWhiteSpace(AccessToken) &&
                BackupOrderNumber.HasValue;
        }

        private bool IsValidationProperty(string propertyName)
        {
            return propertyName == nameof(BackupOrderNumber) ||
                propertyName == nameof(YahooStoreID) ||
                propertyName == nameof(AccessToken);
        }

        private bool Validate(string arg)
        {
            try
            {
                YahooResponse response = storeWebClient(new YahooStoreEntity() { YahooStoreID = YahooStoreID, AccessToken = AccessToken })
                            .GetOrderRange(BackupOrderNumber.GetValueOrDefault());

                return response.ErrorResourceList == null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected string CheckCredentials(YahooResponse response)
        {
            foreach (YahooError error in response.ErrorMessages.Error)
            {
                switch (error.Code)
                {
                    case 10010:
                        return "Invalid Yahoo Store ID";
                    case 10009:
                        return "Invalid Access Token";
                }
            }

            return string.Empty;
        }

        protected void ValidateBackupOrderNumber()
        {
            handler.Where(IsValidationProperty)
                .Where(IsValidationPropertyEntered)
                .Do(_ => IsValid = YahooOrderNumberValidation.Validating)
                .Throttle(TimeSpan.FromMilliseconds(350))
                .ObserveOn(TaskPoolScheduler.Default)
                .Select(Validate)
                .Select(x => x ? YahooOrderNumberValidation.Valid : YahooOrderNumberValidation.Invalid)
                .ObserveOn(DispatcherScheduler.Current)
                .Subscribe(x => IsValid = x);
        }

        public string Save(YahooStoreEntity store)
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

            if (string.IsNullOrWhiteSpace(BackupOrderNumber.ToString()))
            {
                return "Please enter a starting order number";
            }

            try
            {
                YahooResponse response = storeWebClient(new YahooStoreEntity() { YahooStoreID = YahooStoreID, AccessToken = AccessToken })
                    .ValidateCredentials();

                return response.ErrorMessages == null ? string.Empty : CheckCredentials(response);
            }
            catch (Exception ex)
            {
                return $"Error connecting to Yahoo Api: {ex.Message}";
            }
        }
    }
}
