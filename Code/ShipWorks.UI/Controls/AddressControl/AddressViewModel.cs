using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.UI;
using ShipWorks.AddressValidation;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.Core.UI;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;

namespace ShipWorks.UI.Controls.AddressControl
{
    /// <summary>
    /// View model for use by AddressControl
    /// </summary>
    public partial class AddressViewModel : INotifyPropertyChanged, INotifyPropertyChanging, IDisposable
    {
        private readonly string[] validationProperties = { nameof(Street), nameof(CountryCode), nameof(PostalCode), nameof(StateProvCode), nameof(City) };
        private readonly IAddressValidator validator;
        private readonly PropertyChangedHandler handler;
        private readonly IShippingOriginManager shippingOriginManager;
        private readonly IMessageHelper messageHelper;
        private readonly IValidatedAddressScope validatedAddressScope;
        private readonly IAddressSelector addressSelector;
        private IDisposable addressValidationSubscriptions;
        private long? entityId;
        private string prefix;
        private AddressAdapter lastValidatedAddress;

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary>
        /// Constructor that should only be used by WPF
        /// </summary>
        public AddressViewModel()
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged, () => PropertyChanging);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AddressViewModel(IShippingOriginManager shippingOriginManager, IMessageHelper messageHelper,
            IValidatedAddressScope validatedAddressScope, IAddressValidator validator, IAddressSelector addressSelector) : this()
        {
            this.validator = validator;
            this.shippingOriginManager = shippingOriginManager;
            this.messageHelper = messageHelper;
            this.validatedAddressScope = validatedAddressScope;
            this.addressSelector = addressSelector;

            AddressSuggestions = Enumerable.Empty<KeyValuePair<string, ValidatedAddressEntity>>();
            ValidateCommand = new RelayCommand(ValidateAddress);
            ShowValidationMessageCommand = new RelayCommand(ShowValidationMessage);
            SelectAddressSuggestionCommand = new RelayCommand<ValidatedAddressEntity>(SelectAddressSuggestion);
        }

        /// <summary>
        /// Expose a stream of property changes
        /// </summary>
        public IObservable<string> PropertyChangeStream => handler;

        /// <summary>
        /// Can the current address be validated
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool CanValidateAddress => validator.CanValidate(ValidationStatus);

        /// <summary>
        /// Can suggestions be displayed
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool CanShowSuggestions => validator.CanShowSuggestions(ValidationStatus) && SuggestionCount > 0;

        /// <summary>
        /// Can a validation message be shown
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool CanShowValidationMessage => validator.CanShowMessage(ValidationStatus) && !string.IsNullOrEmpty(ValidationMessage);

        /// <summary>
        /// Load the person
        /// </summary>
        public virtual void Load(PersonAdapter person)
        {
            addressValidationSubscriptions?.Dispose();

            Populate(person);

            if (person.Entity != null)
            {
                entityId = EntityUtility.GetEntityId(person.Entity);
                prefix = person.FieldPrefix;

                IEnumerable<ValidatedAddressEntity> validatedAddresses = validatedAddressScope.LoadValidatedAddresses(entityId.GetValueOrDefault(), prefix);
                AddressSuggestions = BuildDictionary(validatedAddresses);

                PopulateValidationDetails(person);
            }
            else
            {
                entityId = null;
                prefix = null;
            }

            SetupAddressValidationMessagePropertyHandlers();
        }

        /// <summary>
        /// Save the current values to the specified person adapter
        /// </summary>
        public virtual void SaveToEntity(PersonAdapter person)
        {
            person.SaveStreet(Street);
            person.SaveFullName(FullName);
            person.Company = Company;
            person.City = City;
            person.PostalCode = PostalCode;
            person.StateProvCode = Geography.GetStateProvCode(StateProvCode);
            person.CountryCode = CountryCode;
            person.Email = Email;
            person.Phone = Phone;
            person.AddressValidationStatus = (int)ValidationStatus;
            person.AddressValidationError = ValidationMessage;
            person.AddressValidationSuggestionCount = suggestionCount;
        }

        /// <summary>
        /// Set address validation details
        /// </summary>
        private void PopulateValidationDetails(PersonAdapter person)
        {
            ValidationMessage = person.AddressValidationError;
            SuggestionCount = person.AddressValidationSuggestionCount;
            ValidationStatus = (AddressValidationStatusType)person.AddressValidationStatus;

            lastValidatedAddress = new AddressAdapter();
            person.CopyTo(lastValidatedAddress);
            person.ConvertTo<AddressAdapter>().CopyValidationDataTo(lastValidatedAddress);

            ResetValidationProperties();
        }

        /// <summary>
        /// Populates the UI properties
        /// </summary>
        private void Populate(PersonAdapter person)
        {
            PopulateAddress(person);
            PopulatePerson(person);
        }

        /// <summary>
        /// Populates the address UI properties
        /// </summary>
        private void PopulateAddress(PersonAdapter person)
        {
            Street = person.StreetAll;
            City = person.City;
            StateProvCode = Geography.GetStateProvName(person.StateProvCode);
            PostalCode = person.PostalCode;
            CountryCode = person.CountryCode;
        }

        /// <summary>
        /// Populates the person UI properties
        /// </summary>
        private void PopulatePerson(PersonAdapter person)
        {
            FullName = new PersonName(person).FullName;
            Company = person.Company;
            Email = person.Email;
            Phone = person.Phone;
        }

        /// <summary>
        /// Set the address from the specified origin address type
        /// </summary>
        public virtual void SetAddressFromOrigin(long addressId, long orderId, long accountId, ShipmentTypeCode shipmentType)
        {
            PersonAdapter address = shippingOriginManager.GetOriginAddress(addressId, orderId, accountId, shipmentType);
            if (address != null)
            {
                Load(address);
            }
        }

        /// <summary>
        /// Validate the currently entered address
        /// </summary>
        private async void ValidateAddress()
        {
            if (!entityId.HasValue)
            {
                return;
            }

            using (messageHelper.ShowProgressDialog("Validating Addresses", "ShipWorks is validating the addresses."))
            {
                long currentEntityId = entityId.Value;

                PersonAdapter personAdapter = new PersonAdapter();
                AddressAdapter addressAdapter = new AddressAdapter();

                SaveToEntity(personAdapter);
                personAdapter.CopyTo(addressAdapter);

                ValidatedAddressData validationData = await validator.ValidateAsync(addressAdapter, true);

                // See if the loaded address has changed since we started validating
                if (currentEntityId != entityId)
                {
                    return;
                }

                addressAdapter.CopyTo(personAdapter);

                validatedAddressScope.StoreAddresses(entityId.Value, validationData.AllAddresses, prefix);
                AddressSuggestions = BuildDictionary(validationData.AllAddresses);

                PopulateAddress(personAdapter);
                PopulateValidationDetails(personAdapter);
            }
        }

        /// <summary>
        /// Select the specified address suggestion
        /// </summary>
        private async void SelectAddressSuggestion(ValidatedAddressEntity addressSuggestion)
        {
            addressValidationSubscriptions?.Dispose();

            PersonAdapter person = new PersonAdapter();
            SaveToEntity(person);

            AddressAdapter changedAddress = await addressSelector.SelectAddress(person.ConvertTo<AddressAdapter>(), addressSuggestion);
            PersonAdapter changedPerson = changedAddress.ConvertTo<PersonAdapter>();

            PopulateAddress(changedPerson);
            ValidationStatus = (AddressValidationStatusType)changedPerson.AddressValidationStatus;

            SetupAddressValidationMessagePropertyHandlers();
        }

        /// <summary>
        /// Build a dictionary of addresses for use in the UI
        /// </summary>
        private IEnumerable<KeyValuePair<string, ValidatedAddressEntity>> BuildDictionary(IEnumerable<ValidatedAddressEntity> validatedAddresses)
        {
            // Stamps has sent us duplicates, so we can't use a dictionary here
            return validatedAddresses
                .Select(x => new KeyValuePair<string, ValidatedAddressEntity>(addressSelector.FormatAddress(x), x));
        }

        /// <summary>
        /// Show the validation message
        /// </summary>
        private void ShowValidationMessage() => messageHelper.ShowInformation(ValidationMessage);

        /// <summary>
        /// Add property changed handlers for dealing with address validation
        /// </summary>
        private void SetupAddressValidationMessagePropertyHandlers()
        {
            addressValidationSubscriptions?.Dispose();

            addressValidationSubscriptions = new CompositeDisposable(
                handler.Where(x => x == nameof(ValidationStatus)).Subscribe(_ => ResetValidationProperties()),
                handler.Where(x => validationProperties.Contains(x)).Subscribe(_ => InvalidateValidation())
            );
        }

        /// <summary>
        /// Invalidate validation of the address
        /// </summary>
        private void InvalidateValidation()
        {
            if (entityId.HasValue)
            {
                validatedAddressScope.ClearAddresses(entityId.Value, prefix);
            }

            ValidationStatus = AddressValidationStatusType.NotChecked;
        }

        /// <summary>
        /// Invalidate the validation properties
        /// </summary>
        private void ResetValidationProperties()
        {
            handler.RaisePropertyChanged(nameof(CanValidateAddress));
            handler.RaisePropertyChanged(nameof(CanShowSuggestions));
            handler.RaisePropertyChanged(nameof(CanShowValidationMessage));
        }

        /// <summary>
        /// Dispose of resources
        /// </summary>
        public void Dispose()
        {
            addressValidationSubscriptions?.Dispose();
        }
    }
}
