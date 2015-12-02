using System;
using System.ComponentModel;
using System.Net;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reflection;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Yahoo;
using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration;
using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO;

namespace ShipWorks.Stores.UI.Platforms.Yahoo.ApiIntegration.WizardPages
{
    public class YahooApiAccountPageViewModel : INotifyPropertyChanged
    {
        private readonly IStoreTypeManager storeTypeManager;
        private readonly Func<YahooStoreEntity, IYahooApiWebClient> storeWebClient;
        private readonly PropertyChangedHandler handler;
        public event PropertyChangedEventHandler PropertyChanged;
        private string yahooStoreID;
        private string accessToken;
        private string helpUrl;
        private long? backupOrderNumber;
        private YahooOrderNumberValidation isValid;

        public YahooApiAccountPageViewModel(IStoreTypeManager storeTypeManager, Func<YahooStoreEntity, IYahooApiWebClient> storeWebClient)
        {
            this.storeTypeManager = storeTypeManager;
            this.storeWebClient = storeWebClient;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
            YahooStoreType storeType = storeTypeManager.GetType(StoreTypeCode.Yahoo) as YahooStoreType;
            HelpUrl = storeType?.AccountSettingsHelpUrl;
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
        public string HelpUrl
        {
            get { return helpUrl; }
            set { handler.Set(nameof(HelpUrl), ref helpUrl, value); }
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

        public void Load(YahooStoreEntity storeEntity)
        {
            HelpUrl = ((YahooStoreType) storeTypeManager.GetType(StoreTypeCode.Yahoo)).AccountSettingsHelpUrl;
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

            try
            {
                YahooResponse response = storeWebClient(new YahooStoreEntity() {YahooStoreID = YahooStoreID, AccessToken = AccessToken})
                    .ValidateCredentials();

                return response.ErrorMessages == null ? string.Empty : CheckCredentials(response);
            }
            catch (Exception ex)
            {
                return $"Error connecting to Yahoo Api: {ex.Message}";
            }
        }

        private string CheckCredentials(YahooResponse response)
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
    }
}