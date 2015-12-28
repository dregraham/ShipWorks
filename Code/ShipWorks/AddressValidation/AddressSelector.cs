using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.Business;
using Interapptive.Shared.UI;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Core.Common.Threading;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.Users;
using ShipWorks.Users.Security;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Responsible for displaying possible validated addresses
    /// </summary>
    public class AddressSelector : IAddressSelector
    {
        private readonly string addressPrefix;

        /// <summary>
        /// An address has just been selected from the list of options
        /// </summary>
        public event EventHandler<AddressSelectedEventArgs> AddressSelected;

        /// <summary>
        /// An address is about to be selected from the list of options
        /// </summary>
        public event EventHandler AddressSelecting;

        /// <summary>
        /// Constructor
        /// </summary>
        public AddressSelector() : this(string.Empty)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressSelector"/> class.
        /// </summary>
        public AddressSelector(string addressPrefix)
        {
            this.addressPrefix = addressPrefix;
        }

        /// <summary>
        /// Gets the ready to ship status types
        /// </summary>
        public static List<AddressValidationStatusType> ReadyToShip
        {
            get
            {
                return new List<AddressValidationStatusType>()
                {
                    AddressValidationStatusType.Valid,
                    AddressValidationStatusType.SuggestionIgnored,
                    AddressValidationStatusType.Fixed,
                    AddressValidationStatusType.SuggestionSelected,
                    AddressValidationStatusType.WillNotValidate
                };
            }
        }

        /// <summary>
        /// Gets the not validated statuses
        /// </summary>
        public static List<AddressValidationStatusType> NotValidated
        {
            get
            {
                return new List<AddressValidationStatusType>()
                {
                    AddressValidationStatusType.Error,
                    AddressValidationStatusType.HasSuggestions,
                    AddressValidationStatusType.BadAddress,
                    AddressValidationStatusType.Pending,
                    AddressValidationStatusType.NotChecked
                };
            }
        }

        /// <summary>
        /// Get the address prefix
        /// </summary>
        protected string AddressPrefix
        {
            get
            {
                return addressPrefix;
            }
        }

        /// <summary>
        /// Checks whether the validation suggestion hyperlink should be enabled
        /// </summary>
        public bool IsValidationSuggestionLinkEnabled(object arg)
        {
            AddressAdapter addressAdapter = GetAddressAdapterFromObject(arg);
            if (addressAdapter == null)
            {
                return true;
            }

            switch ((AddressValidationStatusType) addressAdapter.AddressValidationStatus)
            {
                case AddressValidationStatusType.Fixed:
                case AddressValidationStatusType.HasSuggestions:
                case AddressValidationStatusType.SuggestionIgnored:
                case AddressValidationStatusType.SuggestionSelected:
                    return addressAdapter.AddressValidationSuggestionCount > 0;
                case AddressValidationStatusType.BadAddress:
                case AddressValidationStatusType.WillNotValidate:
                case AddressValidationStatusType.Error:
                    return !string.IsNullOrEmpty(addressAdapter.AddressValidationError);
                case AddressValidationStatusType.NotChecked:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Displays the validation suggestion link text
        /// </summary>
        public string DisplayValidationSuggestionLabel(object arg)
        {
            AddressAdapter addressAdapter = GetAddressAdapterFromObject(arg);
            if (addressAdapter == null)
            {
                return string.Empty;
            }

            switch ((AddressValidationStatusType) addressAdapter.AddressValidationStatus)
            {
                case AddressValidationStatusType.Valid:
                case AddressValidationStatusType.NotChecked:
                case AddressValidationStatusType.Pending:
                    return string.Empty;
                case AddressValidationStatusType.Fixed:
                case AddressValidationStatusType.HasSuggestions:
                case AddressValidationStatusType.SuggestionIgnored:
                case AddressValidationStatusType.SuggestionSelected:
                    return string.Format("{0} Suggestion{1}", addressAdapter.AddressValidationSuggestionCount, addressAdapter.AddressValidationSuggestionCount != 1 ? "s" : string.Empty);
                case AddressValidationStatusType.BadAddress:
                case AddressValidationStatusType.WillNotValidate:
                case AddressValidationStatusType.Error:
                    return string.IsNullOrEmpty(addressAdapter.AddressValidationError) ? string.Empty : "Details...";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Gets an address adapter from an object, if possible.
        /// </summary>
        private AddressAdapter GetAddressAdapterFromObject(object arg)
        {
            AddressAdapter addressAdapter = arg as AddressAdapter;

            if (addressAdapter == null)
            {
                IEntity2 entity = arg as IEntity2;
                if (entity != null)
                {
                    addressAdapter = new AddressAdapter(entity, addressPrefix);
                }
            }

            return addressAdapter;
        }

        /// <summary>
        /// Display the list of available addresses
        /// </summary>
        public void ShowAddressOptionMenu(Control owner, AddressAdapter entityAdapter, Point displayPosition, Func<List<ValidatedAddressEntity>> getValidatedAddresses)
        {
            // If we won't validate, an error occured, or the address isn't valid, let the user know why and don't show the address selection menu
            if (entityAdapter.AddressValidationStatus == (int) AddressValidationStatusType.WillNotValidate ||
                entityAdapter.AddressValidationStatus == (int) AddressValidationStatusType.BadAddress ||
                entityAdapter.AddressValidationStatus == (int) AddressValidationStatusType.Error)
            {
                MessageHelper.ShowInformation(owner, entityAdapter.AddressValidationError);
                return;
            }

            List<ValidatedAddressEntity> validatedAddresses = getValidatedAddresses();
            ValidatedAddressEntity originalValidatedAddress = validatedAddresses.FirstOrDefault(x => x.IsOriginal);
            List<ValidatedAddressEntity> suggestedAddresses = validatedAddresses.Where(x => !x.IsOriginal).ToList();

            ContextMenu menu = BuildMenu(owner, entityAdapter, originalValidatedAddress, suggestedAddresses);

            menu.Show(owner, displayPosition);
        }

        /// <summary>
        /// Build the context menu
        /// </summary>
        private ContextMenu BuildMenu(Control owner, AddressAdapter entityAdapter, ValidatedAddressEntity originalValidatedAddress, List<ValidatedAddressEntity> suggestedAddresses)
        {
            List<MenuItem> menuItems = new List<MenuItem>();

            if (originalValidatedAddress != null)
            {
                menuItems.Add(CreateMenuItem(originalValidatedAddress, entityAdapter));
            }

            if (suggestedAddresses.Any())
            {
                if (originalValidatedAddress != null)
                {
                    menuItems.Add(new MenuItem("-"));
                }

                menuItems.AddRange(suggestedAddresses.Select(x => CreateMenuItem(x, entityAdapter)).OrderBy(x => x.Text));
            }

            // If has a single suggestion and haven't set any store to ValidateAndApply, show this menu to set all stores to ValidateAndApply
            if (UserSession.Security.HasPermission(PermissionType.ManageStores) &&
                entityAdapter.AddressValidationStatus == (int) AddressValidationStatusType.HasSuggestions &&
                suggestedAddresses.Count == 1 &&
                StoreManager.GetAllStores().All(store => store.AddressValidationSetting != (int) AddressValidationStoreSettingType.ValidateAndApply))
            {
                menuItems.Add(new MenuItem("Always Fix Addresses For All Stores", 
                    async (sender, args) => await AlwaysFixAddressesSelected(owner, suggestedAddresses.First(), entityAdapter)));
            }

            return new ContextMenu(menuItems.ToArray());
        }

        /// <summary>
        /// Select the address and set AutoFix to be default for all stores.
        /// </summary>
        private async Task AlwaysFixAddressesSelected(Control owner, ValidatedAddressEntity validatedAddress, AddressAdapter entityAdapter)
        {
            DialogResult isSureResult = MessageHelper.ShowQuestion(owner, "Are you sure you want to always fix addresses for all stores?");
            if (isSureResult != DialogResult.OK)
            {
                return;
            }

            await SelectAddress(entityAdapter, validatedAddress);

            using (SqlAdapter sqlAdapter = new SqlAdapter())
            {
                List<StoreEntity> allStores = StoreManager.GetAllStores();
                allStores.ForEach(store =>
                {
                    store.AddressValidationSetting = (int) AddressValidationStoreSettingType.ValidateAndApply;
                    StoreManager.SaveStore(store, sqlAdapter);
                });

                sqlAdapter.Commit();
            }
        }

        /// <summary>
        /// Create a menu item from a validated address
        /// </summary>
        private MenuItem CreateMenuItem(ValidatedAddressEntity validatedAddress, AddressAdapter entityAdapter)
        {
            string title = FormatAddress(validatedAddress);

            return new MenuItem(title, async (sender, args) => await SelectAddress(entityAdapter, validatedAddress));
        }

        /// <summary>
        /// Select an address to copy into the entity's shipping address
        /// </summary>
        public async Task<AddressAdapter> SelectAddress(AddressAdapter addressToUpdate, ValidatedAddressEntity selectedAddress)
        {
            OnAddressSelecting();

            await UpdateSelectedAddressIfRequred(selectedAddress);

            AddressAdapter originalAddress = new AddressAdapter();
            addressToUpdate.CopyTo(originalAddress);

            AddressAdapter.Copy(selectedAddress, string.Empty, addressToUpdate);

            addressToUpdate.AddressValidationStatus = selectedAddress.IsOriginal ?
                (int) AddressValidationStatusType.SuggestionIgnored :
                (int) AddressValidationStatusType.SuggestionSelected;

            OnAddressSelected(addressToUpdate, originalAddress);

            return addressToUpdate;
        }

        /// <summary>
        /// When a suggested address is selected, it might not have a value for residential or PO Box
        /// </summary>
        private static Task UpdateSelectedAddressIfRequred(ValidatedAddressEntity selectedAddress)
        {
            if (!selectedAddress.IsOriginal &&
                selectedAddress.ResidentialStatus == (int) ValidationDetailStatusType.Unknown &&
                selectedAddress.POBox == (int) ValidationDetailStatusType.Unknown)
            {
                AddressValidator addressValidator = new AddressValidator();
                return addressValidator.ValidateAsync(selectedAddress, string.Empty, true, (entity, entities) =>
                {
                    // If we have updated statuses save them.
                    if ((selectedAddress.ResidentialStatus != (int) ValidationDetailStatusType.Unknown ||
                        selectedAddress.POBox != (int) ValidationDetailStatusType.Unknown) &&
                        !selectedAddress.IsNew)
                    {
                        using (SqlAdapter sqlAdapter = SqlAdapter.Default)
                        {
                            sqlAdapter.SaveAndRefetch(selectedAddress);
                            sqlAdapter.Commit();
                        }
                    }
                });
            }

            return TaskUtility.CompletedTask;
        }

        /// <summary>
        /// An address is about to be selected
        /// </summary>
        private void OnAddressSelecting()
        {
            if (AddressSelecting != null)
            {
                AddressSelecting(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// An address was selected
        /// </summary>
        protected virtual void OnAddressSelected(AddressAdapter entityAdapter, AddressAdapter originalAddress)
        {
            if (AddressSelected != null)
            {
                AddressSelected(this, new AddressSelectedEventArgs(entityAdapter));
            }
        }

        /// <summary>
        /// Format the address for display in the menu
        /// </summary>
        public string FormatAddress(ValidatedAddressEntity address)
        {
            StringBuilder format = new StringBuilder();
            AddAddressPart(format, address.Street1);
            AddAddressPart(format, address.Street2);
            AddAddressPart(format, address.Street3);
            AddAddressPart(format, address.City);
            AddAddressPart(format, address.StateProvCode);
            AddAddressPart(format, address.PostalCode);

            if (address.IsOriginal)
            {
                format.Append(" (Original)");
            }

            return format.ToString();
        }

        /// <summary>
        /// Add a part of the address to the string builder if it is not empty
        /// </summary>
        private static void AddAddressPart(StringBuilder format, string addressPart)
        {
            if (string.IsNullOrWhiteSpace(addressPart))
            {
                return;
            }

            if (format.Length > 0)
            {
                format.Append(", ");
            }

            format.Append(addressPart);
        }
    }
}
