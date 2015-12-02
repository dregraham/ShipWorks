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
    /// <summary>
    /// Base class for the account settings and account page view model
    /// </summary>
    public class YahooApiAccountViewModel
    {
        private readonly Func<YahooStoreEntity, IYahooApiWebClient> storeWebClient;
        protected readonly PropertyChangedHandler handler;
        public event PropertyChangedEventHandler PropertyChanged;
        private string yahooStoreID;
        private string accessToken;
        private long? backupOrderNumber;
        private YahooOrderNumberValidation isValid;

        /// <summary>
        /// Initializes a new instance of the <see cref="YahooApiAccountViewModel"/> class.
        /// </summary>
        /// <param name="storeWebClient">The store web client.</param>
        public YahooApiAccountViewModel(Func<YahooStoreEntity, IYahooApiWebClient> storeWebClient)
        {
            this.storeWebClient = storeWebClient;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        /// <summary>
        /// The store's Yahoo Store ID
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string YahooStoreID
        {
            get { return yahooStoreID; }
            set { handler.Set(nameof(YahooStoreID), ref yahooStoreID, value); }
        }

        /// <summary>
        /// The store's Access Token
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string AccessToken
        {
            get { return accessToken; }
            set { handler.Set(nameof(AccessToken), ref accessToken, value); }
        }

        /// <summary>
        /// The order number to start from when an invalid start range error is caught
        /// when getting a list of orders.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public long? BackupOrderNumber
        {
            get { return backupOrderNumber; }
            set { handler.Set(nameof(BackupOrderNumber), ref backupOrderNumber, value); }
        }

        /// <summary>
        /// Enum for determining the validation status of the backup order number. Wired up
        /// to a converter to display an appropriate image on the page.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public YahooOrderNumberValidation IsValid
        {
            get { return isValid; }
            set { handler.Set(nameof(IsValid), ref isValid, value); }
        }

        /// <summary>
        /// Determines whether the properties we are tracking are all filled in
        /// </summary>
        private bool IsValidationPropertyEntered(string propertyName)
        {
            return !string.IsNullOrWhiteSpace(YahooStoreID) &&
                !string.IsNullOrWhiteSpace(AccessToken) &&
                BackupOrderNumber.HasValue;
        }

        /// <summary>
        /// Determines whether the property changed is one of the ones we
        /// care about for order number validation
        /// </summary>
        private bool IsValidationProperty(string propertyName)
        {
            return propertyName == nameof(BackupOrderNumber) ||
                propertyName == nameof(YahooStoreID) ||
                propertyName == nameof(AccessToken);
        }

        /// <summary>
        /// Validates the backup order number
        /// </summary>
        private bool ValidateBackupOrderNumber(string arg)
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

        /// <summary>
        /// If we are here there is an error in the response, see if it is
        /// because of an invalid store id or access token
        /// </summary>
        /// <param name="response">The response to check for errors</param>
        protected string CheckCredentialsError(YahooResponse response)
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

        /// <summary>
        /// Handles any changes to the form
        /// </summary>
        protected void HandleChanges()
        {
            handler.Where(IsValidationProperty)
                .Where(IsValidationPropertyEntered)
                .Do(_ => IsValid = YahooOrderNumberValidation.Validating)
                .Throttle(TimeSpan.FromMilliseconds(350))
                .ObserveOn(TaskPoolScheduler.Default)
                .Select(ValidateBackupOrderNumber)
                .Select(x => x ? YahooOrderNumberValidation.Valid : YahooOrderNumberValidation.Invalid)
                .ObserveOn(DispatcherScheduler.Current)
                .Subscribe(x => IsValid = x);
        }

        /// <summary>
        /// If all of the account information entered and valid, an empty string is returned.
        /// If errors occur while validating the information, return the error message.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <returns></returns>
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

                return response.ErrorMessages == null ? string.Empty : CheckCredentialsError(response);
            }
            catch (Exception ex)
            {
                return $"Error connecting to Yahoo Api: {ex.Message}";
            }
        }
    }
}
