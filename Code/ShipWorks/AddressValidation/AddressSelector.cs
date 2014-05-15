using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Business;
using Interapptive.Shared.UI;
using Microsoft.Web.Services3.Addressing;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.Linq;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Responsible for displaying possible validated addresses
    /// </summary>
    public class AddressSelector
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

        public AddressSelector(string addressPrefix)
        {
            this.addressPrefix = addressPrefix;
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
                return false;
            }

            switch ((AddressValidationStatusType)addressAdapter.AddressValidationStatus)
            {
                case AddressValidationStatusType.Adjusted:
                case AddressValidationStatusType.NeedsAttention:
                case AddressValidationStatusType.Overridden:
                case AddressValidationStatusType.SuggestedSelected:
                    return addressAdapter.AddressValidationSuggestionCount > 0;
                case AddressValidationStatusType.NotValid:
                case AddressValidationStatusType.WillNotValidate:
                case AddressValidationStatusType.Error:
                    return !string.IsNullOrEmpty(addressAdapter.AddressValidationError);
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

            switch ((AddressValidationStatusType)addressAdapter.AddressValidationStatus)
            {
                case AddressValidationStatusType.Valid:
                case AddressValidationStatusType.NotChecked:
                case AddressValidationStatusType.Pending:
                    return string.Empty;
                case AddressValidationStatusType.Adjusted:
                case AddressValidationStatusType.NeedsAttention:
                case AddressValidationStatusType.Overridden:
                case AddressValidationStatusType.SuggestedSelected:
                    return string.Format("{0} Suggestion{1}", addressAdapter.AddressValidationSuggestionCount, addressAdapter.AddressValidationSuggestionCount != 1 ? "s" : string.Empty);
                case AddressValidationStatusType.NotValid:
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
            if (entityAdapter.AddressValidationStatus == (int)AddressValidationStatusType.WillNotValidate ||
                entityAdapter.AddressValidationStatus == (int)AddressValidationStatusType.NotValid ||
                entityAdapter.AddressValidationStatus == (int)AddressValidationStatusType.Error)
            {
                MessageHelper.ShowInformation(Program.MainForm, entityAdapter.AddressValidationError);
                return;
            }

            List<ValidatedAddressEntity> validatedAddresses = getValidatedAddresses();
            ValidatedAddressEntity originalValidatedAddress = validatedAddresses.FirstOrDefault(x => x.IsOriginal);
            List<ValidatedAddressEntity> suggestedAddresses = validatedAddresses.Where(x => !x.IsOriginal).ToList();

            var menu = BuildMenu(entityAdapter, originalValidatedAddress, suggestedAddresses);

            menu.Show(owner, displayPosition);
        }

        /// <summary>
        /// Build the context menu
        /// </summary>
        private ContextMenu BuildMenu(AddressAdapter entityAdapter, ValidatedAddressEntity originalValidatedAddress, List<ValidatedAddressEntity> suggestedAddresses)
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

            return new ContextMenu(menuItems.ToArray());
        }

        /// <summary>
        /// Create a menu item from a validated address
        /// </summary>
        private MenuItem CreateMenuItem(ValidatedAddressEntity validatedAddress, AddressAdapter entityAdapter)
        {
            string title = FormatAddress(validatedAddress) +
                (validatedAddress.IsOriginal ? " (Original)" : string.Empty);

            return new MenuItem(title, (sender, args) => SelectAddress(entityAdapter, validatedAddress));
        }

        /// <summary>
        /// Select an address to copy into the entity's shipping address
        /// </summary>
        private void SelectAddress(AddressAdapter entityAdapter, ValidatedAddressEntity validatedAddressEntity)
        {
            OnAddressSelecting();

            AddressAdapter originalAddress = new AddressAdapter();
            entityAdapter.CopyTo(originalAddress);

            AddressAdapter.Copy(validatedAddressEntity, string.Empty, entityAdapter);

            entityAdapter.AddressValidationStatus = validatedAddressEntity.IsOriginal ?
                (int)AddressValidationStatusType.Overridden :
                (int)AddressValidationStatusType.SuggestedSelected;

            OnAddressSelected(entityAdapter, originalAddress);
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
        private static string FormatAddress(ValidatedAddressEntity address)
        {
            StringBuilder format = new StringBuilder();
            AddAddressPart(format, address.Street1);
            AddAddressPart(format, address.Street2);
            AddAddressPart(format, address.Street3);
            AddAddressPart(format, address.City);
            AddAddressPart(format, address.StateProvCode);
            AddAddressPart(format, address.PostalCode);
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
