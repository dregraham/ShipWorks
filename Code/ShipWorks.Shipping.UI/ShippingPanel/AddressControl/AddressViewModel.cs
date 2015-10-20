using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using ShipWorks.Core.UI;
using System;
using System.ComponentModel;
using ShipWorks.Data.Utility;
using ShipWorks.Shipping.Services;
using ShipWorks.AddressValidation;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using ShipWorks.Shipping.Commands;
using ShipWorks.Data.Model.EntityClasses;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Disposables;

namespace ShipWorks.Shipping.UI.ShippingPanel.AddressControl
{
    /// <summary>
    /// View model for use by AddressControl
    /// </summary>
    public partial class AddressViewModel : INotifyPropertyChanged, INotifyPropertyChanging, IDisposable
    {
        private readonly string[] validationProperties = { nameof(Street), nameof(CountryCode), nameof(PostalCode), nameof(StateProvCode), nameof(City) };
        private readonly AddressValidator validator;
        private readonly PropertyChangedHandler handler;
        private readonly IShippingOriginManager shippingOriginManager;
        private readonly IDisposable subscriptions;
        private readonly IMessageHelper messageHelper;

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
        public AddressViewModel(IShippingOriginManager shippingOriginManager, IMessageHelper messageHelper, AddressValidator validator)
        {
            this.validator = validator;
            this.shippingOriginManager = shippingOriginManager;
            this.messageHelper = messageHelper;

            handler = new PropertyChangedHandler(this, () => PropertyChanged, () => PropertyChanging);

            subscriptions = new CompositeDisposable(
                handler.Where(x => x == nameof(ValidationStatus)).Subscribe(x => InvalidateValidationProperties()),
                handler.Where(x => validationProperties.Contains(x)).Subscribe(x => ValidationStatus = AddressValidationStatusType.NotChecked)
            );

            AddressSuggestions = Enumerable.Empty<ValidatedAddressEntity>();
            ValidateCommand = new RelayCommand(async () => await ValidateAddress());
            ShowValidationMessageCommand = new RelayCommand(ShowValidationMessage);
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
        public void Load(PersonAdapter person)
        {
            FullName = new PersonName(person).FullName;
            Company = person.Company;
            Street = person.StreetAll;
            City = person.City;
            StateProvCode = Geography.GetStateProvName(person.StateProvCode);
            PostalCode = person.PostalCode;
            CountryCode = person.CountryCode;
            Email = person.Email;
            Phone = person.Phone;
            ValidationMessage = person.AddressValidationError;
            SuggestionCount = person.AddressValidationSuggestionCount;
            ValidationStatus = (AddressValidationStatusType)person.AddressValidationStatus;
        }

        /// <summary>
        /// Save the current values to the specified person adapter
        /// </summary>
        public void SaveToEntity(PersonAdapter person)
        {
            SaveStreet(person, Street);
            SaveFullName(person, FullName);
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
        /// Save the street to the specified adapter
        /// </summary>
        private static void SaveStreet(PersonAdapter person, string value)
        {
            int maxStreet1 = EntityFieldLengthProvider.GetMaxLength(EntityFieldLengthSource.PersonStreet1);
            int maxStreet2 = EntityFieldLengthProvider.GetMaxLength(EntityFieldLengthSource.PersonStreet2);
            int maxStreet3 = EntityFieldLengthProvider.GetMaxLength(EntityFieldLengthSource.PersonStreet3);

            string[] lines = value?.Split(new[] { Environment.NewLine }, StringSplitOptions.None) ?? new string[0];

            string line1 = lines.Length > 0 ? lines[0] : string.Empty;
            string line2 = lines.Length > 1 ? lines[1] : string.Empty;
            string line3 = lines.Length > 2 ? lines[2] : string.Empty;

            if (line1.Length > maxStreet1)
            {
                line2 = line1.Substring(maxStreet1) + " " + line2;
                line1 = line1.Substring(0, maxStreet1);
            }

            if (line2.Length > maxStreet2)
            {
                line3 = line2.Substring(maxStreet2) + " " + line3;
                line2 = line2.Substring(0, maxStreet2);
            }

            if (line3.Length > maxStreet3)
            {
                line3 = line3.Substring(0, maxStreet3);
            }

            person.Street1 = line1;
            person.Street2 = line2;
            person.Street3 = line3;
        }

        /// <summary>
        /// Save the full name to the specified person
        /// </summary>
        private static void SaveFullName(PersonAdapter person, string value)
        {
            PersonName name = PersonName.Parse(value);

            int maxFirst = EntityFieldLengthProvider.GetMaxLength(EntityFieldLengthSource.PersonFirst);
            if (name.First.Length > maxFirst)
            {
                name.Middle = name.First.Substring(maxFirst) + name.Middle;
                name.First = name.First.Substring(0, maxFirst);
            }

            int maxMiddle = EntityFieldLengthProvider.GetMaxLength(EntityFieldLengthSource.PersonMiddle);
            if (name.Middle.Length > maxMiddle)
            {
                name.Last = name.Middle.Substring(maxMiddle) + name.Last;
                name.Middle = name.Middle.Substring(0, maxMiddle);
            }

            int maxLast = EntityFieldLengthProvider.GetMaxLength(EntityFieldLengthSource.PersonLast);
            if (name.Last.Length > maxLast)
            {
                name.Last = name.Last.Substring(0, maxLast);
            }

            person.FirstName = name.First;
            person.MiddleName = name.Middle;
            person.LastName = name.LastWithSuffix;
            person.UnparsedName = name.UnparsedName;
            person.NameParseStatus = name.ParseStatus;
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
        private async Task ValidateAddress()
        {
            PersonAdapter adapter = new PersonAdapter();
            SaveToEntity(adapter);
            await validator.ValidateAsync(adapter.ConvertTo<AddressAdapter>(), true, (x, y) =>
            {
                List<ValidatedAddressEntity> suggestions = y.ToList();
                SuggestionCount = suggestions.Count;
                AddressSuggestions = suggestions;

                Load(adapter);
            });
        }

        /// <summary>
        /// Show the validation message
        /// </summary>
        private void ShowValidationMessage() => messageHelper.ShowInformation(ValidationMessage);

        /// <summary>
        /// Invalidate the validation properties
        /// </summary>
        private void InvalidateValidationProperties()
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
            subscriptions.Dispose();
        }
    }
}
