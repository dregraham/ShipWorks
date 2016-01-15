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
        protected readonly Func<YahooStoreEntity, IYahooApiWebClient> StoreWebClient;
        protected readonly PropertyChangedHandler Handler;
        public event PropertyChangedEventHandler PropertyChanged;
        private string yahooStoreID;
        private string accessToken;
        private long? backupOrderNumber;
        private YahooOrderNumberValidation isValid;
        private string validationErrorMessage;
        private const int InvalidStatusIDFormat = 10402;

        /// <summary>
        /// Initializes a new instance of the <see cref="YahooApiAccountViewModel"/> class.
        /// </summary>
        /// <param name="storeWebClient">The store web client.</param>
        public YahooApiAccountViewModel(Func<YahooStoreEntity, IYahooApiWebClient> storeWebClient)
        {
            this.StoreWebClient = storeWebClient;
            Handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        /// <summary>
        /// The store's Yahoo Store ID
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string YahooStoreID
        {
            get { return yahooStoreID; }
            set { Handler.Set(nameof(YahooStoreID), ref yahooStoreID, value); }
        }

        /// <summary>
        /// The store's Access Token
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string AccessToken
        {
            get { return accessToken; }
            set { Handler.Set(nameof(AccessToken), ref accessToken, value); }
        }

        /// <summary>
        /// The order number to start from when an invalid start range error is caught
        /// when getting a list of orders.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public long? BackupOrderNumber
        {
            get { return backupOrderNumber; }
            set { Handler.Set(nameof(BackupOrderNumber), ref backupOrderNumber, value); }
        }

        /// <summary>
        /// Enum for determining the validation status of the backup order number. Wired up
        /// to a converter to display an appropriate image on the page.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public YahooOrderNumberValidation IsValid
        {
            get { return isValid; }
            set { Handler.Set(nameof(IsValid), ref isValid, value); }
        }

        /// <summary>
        /// The message to display if credential or order number validation fails
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ValidationErrorMessage
        {
            get { return validationErrorMessage;}
            set { Handler.Set(nameof(ValidationErrorMessage), ref validationErrorMessage, value); }
        }

        /// <summary>
        /// Determines whether the properties we are tracking are all filled in
        /// </summary>
        private bool IsValidationPropertyEntered(string propertyName)
        {
            return !string.IsNullOrWhiteSpace(YahooStoreID) &&
                   !string.IsNullOrWhiteSpace(AccessToken);

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
        private YahooOrderNumberValidation ValidateBackupOrderNumber(string arg)
        {
            if (BackupOrderNumber == null)
            {
                ValidationErrorMessage = string.Empty;
                return YahooOrderNumberValidation.NotValidated;
            }

            try
            {
                YahooResponse response = StoreWebClient(new YahooStoreEntity() { YahooStoreID = YahooStoreID, AccessToken = AccessToken })
                            .GetOrderRange(BackupOrderNumber.GetValueOrDefault());

                CheckCredentialsError(response);

                return response.ErrorResourceList == null ? YahooOrderNumberValidation.Valid : YahooOrderNumberValidation.Invalid;
            }
            catch (Exception)
            {
                return YahooOrderNumberValidation.Invalid;
            }
        }

        /// <summary>
        /// If we are here there is an error in the response, see if it is
        /// because of an invalid store id, access token, or order number
        /// </summary>
        /// <param name="response">The response to check for errors</param>
        protected string CheckCredentialsError(YahooResponse response)
        {
            ValidationErrorMessage = string.Empty;

            // Here we check both ErrorMessages and ErrorResponseList because
            // Yahoo gives the same information, 2 different ways, based on
            // whether you are hitting the order or catalog endpoint.
            if (response.ErrorMessages != null)
            {
                foreach (YahooError error in response.ErrorMessages.Error)
                {
                    switch (error.Code)
                    {
                        case 10010:
                            ValidationErrorMessage = "Invalid Yahoo Store ID";
                            return ValidationErrorMessage;
                        case 10009:
                            ValidationErrorMessage = "Invalid Access Token";
                            return ValidationErrorMessage;
                        case 20021:
                            ValidationErrorMessage = $"Order #{BackupOrderNumber} does not exist";
                            break;
                    }
                }
            }

            if (response.ErrorResourceList != null)
            {
                foreach (YahooError error in response.ErrorResourceList.Error)
                {
                    switch (error.Code)
                    {
                        case 10010:
                            ValidationErrorMessage = "Invalid Yahoo Store ID";
                            return ValidationErrorMessage;
                        case 10009:
                            ValidationErrorMessage = "Invalid Access Token";
                            return ValidationErrorMessage;
                        case InvalidStatusIDFormat:
                            return "StatusID needs to be specified in correct format";
                        case 20021:
                            ValidationErrorMessage = $"Order #{BackupOrderNumber} does not exist";
                            break;
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Handles any changes to the form
        /// </summary>
        protected void HandleChanges()
        {

            Handler.Where(IsValidationProperty)
                .Where(IsValidationPropertyEntered)
                .Do(_ => IsValid = (BackupOrderNumber == null) ? YahooOrderNumberValidation.NotValidated : YahooOrderNumberValidation.Validating)
                .Throttle(TimeSpan.FromMilliseconds(350))
                .ObserveOn(TaskPoolScheduler.Default)
                .Select(ValidateBackupOrderNumber)
                .Select(validationStatus => validationStatus)
                .ObserveOn(DispatcherScheduler.Current)
                .Subscribe(validationStatus => IsValid = validationStatus);
        }

        /// <summary>
        /// If all of the account information entered and valid, an empty string is returned.
        /// If errors occur while validating the information, return the error message.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <returns></returns>
        public virtual string Save(YahooStoreEntity store)
        {
            store.YahooStoreID = YahooStoreID;
            store.AccessToken = AccessToken;
            store.BackupOrderNumber = BackupOrderNumber;

            if (string.IsNullOrWhiteSpace(YahooStoreID))
            {
                return "Please enter your Yahoo Store ID";
            }

            if (string.IsNullOrWhiteSpace(AccessToken))
            {
                return "Please enter your Access Token";
            }

            if (string.IsNullOrWhiteSpace(BackupOrderNumber.ToString()))
            {
                return "Please enter a starting order number. If you do not currently have any orders " +
                       "for this store, enter any number and you can reset it later.";
            }

            if (BackupOrderNumber != null && BackupOrderNumber < 0)
            {
                return "Yahoo only supports positive, numeric order numbers";
            }

            try
            {
                YahooResponse response = StoreWebClient(new YahooStoreEntity() { YahooStoreID = YahooStoreID, AccessToken = AccessToken })
                    .ValidateCredentials();


                string error = CheckCredentialsError(response);

                // If the error message matches this one, then we know the credentials are good.
                return error == "StatusID needs to be specified in correct format" ? string.Empty : error;
            }
            catch (Exception ex)
            {
                return $"Error connecting to Yahoo Api: {ex.Message}";
            }
        }
    }
}
