using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using ShipWorks.Core.UI;
using System;
using System.ComponentModel;
using ShipWorks.Shipping.Services;
using ShipWorks.AddressValidation;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using ShipWorks.Data.Model.EntityClasses;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Disposables;
using ShipWorks.Data;
using GalaSoft.MvvmLight.Command;

namespace ShipWorks.Shipping.UI.ShippingPanel.AddressControl
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

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AddressViewModel(IShippingOriginManager shippingOriginManager, IMessageHelper messageHelper,
            IValidatedAddressScope validatedAddressScope, IAddressValidator validator, IAddressSelector addressSelector)
        {
            this.validator = validator;
            this.shippingOriginManager = shippingOriginManager;
            this.messageHelper = messageHelper;
            this.validatedAddressScope = validatedAddressScope;
            this.addressSelector = addressSelector;

            handler = new PropertyChangedHandler(this, () => PropertyChanged, () => PropertyChanging);

            SetupAddressValidationMessagePropertyHandlers();

            AddressSuggestions = Enumerable.Empty<KeyValuePair<string, ValidatedAddressEntity>>();
            ValidateCommand = new RelayCommand(ValidateAddress);
            ShowValidationMessageCommand = new RelayCommand(ShowValidationMessage);
            SelectAddressSuggestionCommand = new RelayCommand<ValidatedAddressEntity>(SelectAddressSuggestion);
        }

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
            PersonAdapter adapter = new PersonAdapter();
            SaveToEntity(adapter);
            ValidatedAddressData validationData = await validator.ValidateAsync(adapter.ConvertTo<AddressAdapter>(), true);

            List<ValidatedAddressEntity> validatedAddresses = new List<ValidatedAddressEntity>();
            int count = 0;

            if (validationData.Original != null)
            {
                validatedAddresses.Add(validationData.Original);
                count = -1;
            }

            if (validationData.Suggestions != null)
            {
                validatedAddresses.AddRange(validationData.Suggestions);
                count += validatedAddresses.Count;
            }

            validatedAddressScope.StoreAddresses(entityId.GetValueOrDefault(), validatedAddresses, prefix);
            AddressSuggestions = BuildDictionary(validatedAddresses);

            SuggestionCount = Math.Max(0, count);

            PopulateAddress(adapter);
            PopulateValidationDetails(adapter);
        }

        /// <summary>
        /// Select the specified address suggestion
        /// </summary>
        private async void SelectAddressSuggestion(ValidatedAddressEntity addressSuggestion)
        {
            addressValidationSubscriptions?.Dispose();

            PersonAdapter person = new PersonAdapter(new ValidatedAddressEntity(), string.Empty);
            SaveToEntity(person);
            
            AddressAdapter changedAddress = await addressSelector.SelectAddress(person.ConvertTo<AddressAdapter>(), addressSuggestion);
            PersonAdapter changedPerson = changedAddress.ConvertTo<PersonAdapter>();

            PopulateAddress(changedPerson);
            ValidationStatus = (AddressValidationStatusType) changedPerson.AddressValidationStatus;

            SetupAddressValidationMessagePropertyHandlers();
        }

        /// <summary>
        /// Build a dictionary of addresses for use in the UI
        /// </summary>
        private IEnumerable<KeyValuePair<string, ValidatedAddressEntity>> BuildDictionary(IEnumerable<ValidatedAddressEntity> validatedAddresses)
        {
            return validatedAddresses.ToDictionary(
                    x => addressSelector.FormatAddress(x),
                    x => x);
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
                handler.Where(x => x == nameof(ValidationStatus)).Subscribe(x => InvalidateValidationProperties()),
                handler.Where(x => validationProperties.Contains(x)).Subscribe(x => ValidationStatus = AddressValidationStatusType.NotChecked)
            );
        }

        /// <summary>
        /// Invalidate the validation properties
        /// </summary>
        private void InvalidateValidationProperties()
        {
            if (entityId.HasValue)
            {
                validatedAddressScope.StoreAddresses(entityId.Value, Enumerable.Empty<ValidatedAddressEntity>(), prefix);
            }

            handler.RaisePropertyChanged(nameof(CanValidateAddress));
            handler.RaisePropertyChanged(nameof(CanShowSuggestions));
            handler.RaisePropertyChanged(nameof(CanShowValidationMessage));
        }

        /// <summary>
        /// Dispose of resources
        /// </summary>
        public void Dispose()
        {
            addressValidationSubscriptions.Dispose();
        }
    }
}
