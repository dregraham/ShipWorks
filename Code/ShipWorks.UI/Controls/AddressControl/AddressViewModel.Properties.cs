﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.OrderLookup.FieldManager;

namespace ShipWorks.UI.Controls.AddressControl
{
    /// <summary>
    /// View model for use by AddressControl
    /// </summary>
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
        private string validationMessageLabel;
        private IEnumerable<KeyValuePair<string, ValidatedAddressEntity>> addressSuggestions;
        private int suggestionCount;
        private bool isAddressValidationEnabled;

        // Address validation fields.
        SectionLayoutFieldIDs[] addressValidationFields = new SectionLayoutFieldIDs[]
        {
            SectionLayoutFieldIDs.Street,
            SectionLayoutFieldIDs.City,
            SectionLayoutFieldIDs.StateProvince,
            SectionLayoutFieldIDs.PostalCode,
            SectionLayoutFieldIDs.Country
        };

        /// <summary>
        /// Validate address command
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand ValidateCommand { get; private set; }

        /// <summary>
        /// Show the validation message
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand ShowValidationMessageCommand { get; private set; }

        /// <summary>
        /// Select a validated address suggestion
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand SelectAddressSuggestionCommand { get; private set; }

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
        /// Link text to display for viewing the ValidationMessage
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ValidationMessageLabel
        {
            get { return validationMessageLabel; }
            set { handler.Set(nameof(ValidationMessageLabel), ref validationMessageLabel, value); }
        }

        /// <summary>
        /// List of address suggestions
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<KeyValuePair<string, ValidatedAddressEntity>> AddressSuggestions
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

        /// <summary>
        /// Is address validation enabled or not
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsAddressValidationEnabled
        {
            get { return isAddressValidationEnabled; }
            set { handler.Set(nameof(IsAddressValidationEnabled), ref isAddressValidationEnabled, value); }
        }

        /// <summary>
        /// Visibility of address validation
        /// </summary>
        [Obfuscation(Exclude = true)]
        public Visibility AddressValidationVisibility
        {
            get
            {
                var x = FieldLayoutProvider.Fetch()
                    .SelectMany(l => l.SectionFields)
                    .Any(f => f.Selected && addressValidationFields.Contains(f.Id));

                return x ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }
}
