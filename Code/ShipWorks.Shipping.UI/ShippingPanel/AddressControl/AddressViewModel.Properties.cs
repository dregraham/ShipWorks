using ShipWorks.AddressValidation;
using ShipWorks.Data.Model.EntityClasses;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Input;

namespace ShipWorks.Shipping.UI.ShippingPanel.AddressControl
{
    public partial class AddressViewModel
    {
        private string fullName;
        private string phone;
        private string email;
        private string countryCode;
        private string postalCode;
        private string stateProvCode;
        private string city;
        private string street;
        private string company;
        private AddressValidationStatusType validationStatus;
        private string validationMessage;
        private IEnumerable<ValidatedAddressEntity> addressSuggestions;
        private int suggestionCount;

        /// <summary>
        /// Validate address command
        /// </summary>
        public ICommand ValidateCommand { get; private set; }

        /// <summary>
        /// Show the validation message
        /// </summary>
        public ICommand ShowValidationMessageCommand { get; private set; }

        /// <summary>
        /// Full name
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string FullName
        {
            get { return fullName; }
            set { handler.Set(nameof(FullName), ref fullName, value); }
        }

        /// <summary>
        /// Company name
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Company
        {
            get { return company; }
            set { handler.Set(nameof(Company), ref company, value); }
        }

        /// <summary>
        /// Street
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Street
        {
            get { return street; }
            set { handler.Set(nameof(Street), ref street, value); }
        }

        /// <summary>
        /// City
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string City
        {
            get { return city; }
            set { handler.Set(nameof(City), ref city, value); }
        }

        /// <summary>
        /// State
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string StateProvCode
        {
            get { return stateProvCode; }
            set { handler.Set(nameof(StateProvCode), ref stateProvCode, value); }
        }

        /// <summary>
        /// PostalCode
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string PostalCode
        {
            get { return postalCode; }
            set { handler.Set(nameof(PostalCode), ref postalCode, value); }
        }

        /// <summary>
        /// Country
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string CountryCode
        {
            get { return countryCode; }
            set { handler.Set(nameof(CountryCode), ref countryCode, value); }
        }

        /// <summary>
        /// EmailAddress
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Email
        {
            get { return email; }
            set { handler.Set(nameof(Email), ref email, value); }
        }

        /// <summary>
        /// PhoneNumber
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Phone
        {
            get { return phone; }
            set { handler.Set(nameof(Phone), ref phone, value); }
        }

        /// <summary>
        /// Validation status
        /// </summary>
        [Obfuscation(Exclude = true)]
        public AddressValidationStatusType ValidationStatus
        {
            get { return validationStatus; }
            set { handler.Set(nameof(ValidationStatus), ref validationStatus, value); }
        }

        /// <summary>
        /// Message for the most recent validation
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ValidationMessage
        {
            get { return validationMessage; }
            set { handler.Set(nameof(ValidationMessage), ref validationMessage, value); }
        }

        /// <summary>
        /// List of address suggestions
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<ValidatedAddressEntity> AddressSuggestions
        {
            get { return addressSuggestions; }
            set { handler.Set(nameof(AddressSuggestions), ref addressSuggestions, value); }
        }

        /// <summary>
        /// Number of validation suggestions available
        /// </summary>
        [Obfuscation(Exclude = true)]
        public int SuggestionCount
        {
            get { return suggestionCount; }
            set { handler.Set(nameof(SuggestionCount), ref suggestionCount, value); }
        }
    }
}
