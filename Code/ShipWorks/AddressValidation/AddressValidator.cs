using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Business;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Validates and updates addresses
    /// </summary>
    public class AddressValidator
    {
        private readonly IAddressValidationWebClient webClient;
        static readonly ILog log = LogManager.GetLogger(typeof(AddressValidator));

        /// <summary>
        /// Creates a new instance of the AddressValidator with the default web client
        /// </summary>
        public AddressValidator() :
            this(new AddressValidationWebClient())
        {
            
        }

        /// <summary>
        /// Creates a new instance of the AddressValidator using the specified web client
        /// </summary>
        public AddressValidator(IAddressValidationWebClient webClient)
        {
            this.webClient = webClient;
        }

        /// <summary>
        /// Validates an address with no prefix on the specified entity
        /// </summary>
        /// <param name="addressEntity">Entity whose address should be validated</param>
        /// <param name="addressPrefix"></param>
        /// <param name="canAdjustAddress"></param>
        /// <param name="saveAction">Action that should save changes to the database</param>
        public void Validate(IEntity2 addressEntity, string addressPrefix, bool canAdjustAddress, Action<ValidatedAddressEntity, IEnumerable<ValidatedAddressEntity>> saveAction)
        {
            AddressAdapter adapter = new AddressAdapter(addressEntity, addressPrefix);

            // We don't want to validate already validated addresses because we'll lose the original address
            if (!ShouldValidateAddress(adapter))
            {
                return;
            }

            List<AddressValidationResult> suggestedAddresses = new List<AddressValidationResult>();
            try
            {
                string addressValidationError;
                suggestedAddresses = webClient.ValidateAddress(adapter.Street1, adapter.Street2, adapter.City, adapter.StateProvCode, adapter.PostalCode, out addressValidationError) ??
                                     new List<AddressValidationResult>();

                // Store the original address so that the user can revert later if they want
                ValidatedAddressEntity originalAddress = new ValidatedAddressEntity();
                adapter.CopyTo(originalAddress, string.Empty);

                adapter.AddressValidationError = addressValidationError;

                // Set the validation status based on the settings of the store
                if (canAdjustAddress)
                {
                    SetValidationStatus(suggestedAddresses, adapter);
                    UpdateAddressIfAdjusted(adapter, suggestedAddresses);
                }
                else
                {
                    SetValidationStatusForNotify(suggestedAddresses, adapter);

                    AddressValidationResult validatedAddress = suggestedAddresses.FirstOrDefault(x => x.IsValid);
                    if (validatedAddress != null)
                    {
                        adapter.ResidentialStatus = (int) validatedAddress.ResidentialStatus;
                        adapter.POBox = (int) validatedAddress.POBox;
                        adapter.InternationalTerritory = InternationalTerritoryStatus(validatedAddress.StateProvCode, validatedAddress.CountryCode);
                        adapter.MilitaryAddress = MilitaryAddressStatus(validatedAddress.StateProvCode);
                    }
                }

                adapter.AddressValidationSuggestionCount = suggestedAddresses.Count;

                if (suggestedAddresses.Count > 0)
                {
                    saveAction(originalAddress, suggestedAddresses.Select(CreateEntityFromValidationResult));
                }
                else
                {
                    saveAction(null, new List<ValidatedAddressEntity>());
                }

            }
            catch (AddressValidationException ex)
            {
                log.Warn("Error communicating with Address Validation Server.", ex);
                adapter.AddressValidationError = "Error communicating with Address Validation Server.";
                adapter.AddressValidationStatus = (int)AddressValidationStatusType.Error;
                saveAction(null, new List<ValidatedAddressEntity>());
            }
        }

        /// <summary>
        /// Should the specified address be validated
        /// </summary>
        public static bool ShouldValidateAddress(AddressAdapter adapter)
        {
            return adapter.AddressValidationStatus == (int) AddressValidationStatusType.NotChecked ||
                   adapter.AddressValidationStatus == (int) AddressValidationStatusType.Pending ||
                   adapter.AddressValidationStatus == (int) AddressValidationStatusType.Error;
        }

        /// <summary>
        /// Set the validation status on the entity when we should only notify instead of update
        /// </summary>
        private static void SetValidationStatusForNotify(List<AddressValidationResult> suggestedAddresses, AddressAdapter adapter)
        {
            if (!suggestedAddresses.Any())
            {
                adapter.AddressValidationStatus = (int)AddressValidationStatusType.NotValid;
            }
            else if (suggestedAddresses.Count == 1 && suggestedAddresses[0].IsValid && suggestedAddresses[0].IsEqualTo(adapter))
            {
                adapter.AddressValidationStatus = (int)AddressValidationStatusType.Valid;
            }
            else
            {
                adapter.AddressValidationStatus = (int)AddressValidationStatusType.NeedsAttention;
            }
        }

        /// <summary>
        /// Set the validation status on the entity
        /// </summary>
        private static void SetValidationStatus(List<AddressValidationResult> suggestedAddresses, AddressAdapter adapter)
        {
            if (!suggestedAddresses.Any())
            {
                adapter.AddressValidationStatus = (int)AddressValidationStatusType.NotValid;
            }
            else if (suggestedAddresses.Count == 1 && suggestedAddresses[0].IsValid)
            {
                adapter.AddressValidationStatus = suggestedAddresses[0].IsEqualTo(adapter) ?
                    (int)AddressValidationStatusType.Valid :
                    (int)AddressValidationStatusType.Adjusted;
            }
            else
            {
                adapter.AddressValidationStatus = (int)AddressValidationStatusType.NeedsAttention;
            }
        }

        /// <summary>
        /// Update the adapter's address to the first valid address if its status is adjusted
        /// </summary>
        private static void UpdateAddressIfAdjusted(AddressAdapter adapter, IEnumerable<AddressValidationResult> suggestedAddresses)
        {
            AddressValidationResult adjustedAddress = suggestedAddresses.FirstOrDefault(x => x.IsValid);
            if (adjustedAddress == null)
            {
                return;
            }

            if (adapter.AddressValidationStatus == (int) AddressValidationStatusType.Adjusted)
            {
                adjustedAddress.CopyTo(adapter);
            }

            adapter.ResidentialStatus = (int) adjustedAddress.ResidentialStatus;
            adapter.POBox = (int) adjustedAddress.POBox;
            UpdateInternationalTerritoryAndMilitaryAddress(adapter);
        }

        /// <summary>
        /// Create an AddressEntity from an AddressValidationResult
        /// </summary>
        private static ValidatedAddressEntity CreateEntityFromValidationResult(AddressValidationResult validationResult)
        {
            ValidatedAddressEntity address = new ValidatedAddressEntity();
            AddressAdapter adapter = new AddressAdapter(address, string.Empty);
            
            validationResult.CopyTo(adapter);
            UpdateInternationalTerritoryAndMilitaryAddress(adapter);

            return address;
        }

        /// <summary>
        /// Updates the international territory and military address details
        /// </summary>
        private static void UpdateInternationalTerritoryAndMilitaryAddress(AddressAdapter address)
        {
            address.InternationalTerritory = InternationalTerritoryStatus(address.StateProvCode, address.CountryCode);
            address.MilitaryAddress = MilitaryAddressStatus(address.StateProvCode);
        }

        /// <summary>
        /// Gets the mility address status from the state code
        /// </summary>
        private static int MilitaryAddressStatus(string stateProvCode)
        {
            return (stateProvCode == "AE" || stateProvCode == "AP" || stateProvCode == "AA") ?
                (int) MilitaryAddressType.MilitaryAddress :
                (int) MilitaryAddressType.NotMilitaryAddress;
        }

        /// <summary>
        /// Gets the international territory status form the country code
        /// </summary>
        private static int InternationalTerritoryStatus(string stateProvCode, string countryCode)
        {
            return PostalUtility.IsUSInternationalTerritory(countryCode) || PostalUtility.IsUSInternationalTerritory(stateProvCode) ?
                (int) InternationalTerritoryType.InternationalTerritory :
                (int) InternationalTerritoryType.NotInternationalTerritory;
        }
    }
}
