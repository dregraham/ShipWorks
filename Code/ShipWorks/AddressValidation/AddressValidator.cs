using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.Data.Model.EntityClasses;
using System.Threading.Tasks;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Validates and updates addresses
    /// </summary>
    public class AddressValidator : IAddressValidator
    {
        private readonly IAddressValidationWebClient webClient;
        static readonly ILog log = LogManager.GetLogger(typeof(AddressValidator));

        /// <summary>
        /// Creates a new instance of the AddressValidator with the default web client
        /// </summary>
        public AddressValidator() :
            this(new StampsAddressValidationWebClient())
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
        public Task ValidateAsync(IEntity2 addressEntity, string addressPrefix, bool canAdjustAddress, Action<ValidatedAddressEntity, IEnumerable<ValidatedAddressEntity>> saveAction)
        {
            return ValidateAsync(new AddressAdapter(addressEntity, addressPrefix), canAdjustAddress, saveAction);
        }

        /// <summary>
        /// Can suggestions be shown for the given validation status
        /// </summary>
        public bool CanShowSuggestions(AddressValidationStatusType status)
        {
            switch(status)
            {
                case AddressValidationStatusType.Fixed:
                case AddressValidationStatusType.HasSuggestions:
                case AddressValidationStatusType.SuggestionIgnored:
                case AddressValidationStatusType.SuggestionSelected:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Can a message be shown fo the given validation status
        /// </summary>
        public bool CanShowMessage(AddressValidationStatusType status)
        {
            switch(status)
            {
                case AddressValidationStatusType.BadAddress:
                case AddressValidationStatusType.WillNotValidate:
                case AddressValidationStatusType.Error:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Validates an address with no prefix on the specified entity
        /// </summary>
        /// <param name="addressAdapter">Address that should be validated</param>
        /// <param name="canAdjustAddress"></param>
        /// <param name="saveAction">Action that should save changes to the database</param>
        [NDependIgnoreLongMethod]
        public async Task ValidateAsync(AddressAdapter addressAdapter, bool canAdjustAddress, Action<ValidatedAddressEntity, IEnumerable<ValidatedAddressEntity>> saveAction)
        {
            // We don't want to validate already validated addresses because we'll lose the original address
            if (!ShouldValidateAddress(addressAdapter))
            {
                return;
            }

            // It is possible that existing international orders can be "Not Checked." This code updates the order.
            if (!ValidatedAddressManager.EnsureAddressCanBeValidated(addressAdapter))
            {
                saveAction(null, new List<ValidatedAddressEntity>());
                return;
            }

            try
            {
                AddressValidationWebClientValidateAddressResult validationResult = await webClient.ValidateAddressAsync(addressAdapter);

                // Store the original address so that the user can revert later if they want
                ValidatedAddressEntity originalAddress = new ValidatedAddressEntity();
                addressAdapter.CopyTo(originalAddress, string.Empty);
                originalAddress.IsOriginal = true;
                originalAddress.AddressPrefix = addressAdapter.FieldPrefix;

                addressAdapter.AddressValidationError = validationResult.AddressValidationError;

                // Set the validation status based on the settings of the store
                if (canAdjustAddress)
                {
                    SetValidationStatus(validationResult, addressAdapter);
                    UpdateAddressIfAdjusted(addressAdapter, validationResult.AddressValidationResults);
                }
                else
                {
                    SetValidationStatusForNotify(validationResult, addressAdapter);

                    AddressValidationResult validatedAddress = validationResult.AddressValidationResults.FirstOrDefault(x => x.IsValid);
                    if (validatedAddress != null)
                    {
                        addressAdapter.ResidentialStatus = (int) validatedAddress.ResidentialStatus;
                        addressAdapter.POBox = (int) validatedAddress.POBox;
                        addressAdapter.USTerritory = InternationalTerritoryStatus(validatedAddress);
                        addressAdapter.MilitaryAddress = MilitaryAddressStatus(validatedAddress.StateProvCode);
                    }
                }

                addressAdapter.AddressValidationSuggestionCount = validationResult.AddressValidationResults.Count;

                if (validationResult.AddressValidationResults.Any())
                {
                    saveAction(originalAddress, validationResult.AddressValidationResults.Select(address => CreateEntityFromValidationResult(address, "Ship")));
                }
                else
                {
                    saveAction(null, new List<ValidatedAddressEntity>());
                }
            }
            catch (AddressValidationException ex)
            {
                log.Warn("Error communicating with Address Validation Server.", ex);
                addressAdapter.AddressValidationError = string.Format("Error communicating with Address Validation Server.\r\n{0}", ex.Message);
                addressAdapter.AddressValidationStatus = (int)AddressValidationStatusType.Error;
                addressAdapter.AddressType = (int) AddressType.Error;
                saveAction(null, new List<ValidatedAddressEntity>());
            }
        }

        /// <summary>
        /// Validates an address with no prefix on the specified entity
        /// </summary>
        public async Task<ValidatedAddressData> ValidateAsync(AddressAdapter addressAdapter, bool canAdjustAddress)
        {
            ValidatedAddressData data = ValidatedAddressData.NotSet;

            await ValidateAsync(addressAdapter, canAdjustAddress, (original, suggestions) =>
            {
                data = original == null ?
                    ValidatedAddressData.Empty :
                    new ValidatedAddressData(original, suggestions);
            });

            return data;
        }

        /// <summary>
        /// Can the given status be validated
        /// </summary>
        public bool CanValidate(AddressValidationStatusType status) => ShouldValidateAddress(status);

        /// <summary>
        /// Should the specified address be validated
        /// </summary>
        public static bool ShouldValidateAddress(AddressAdapter adapter) =>
            ShouldValidateAddress((AddressValidationStatusType)adapter.AddressValidationStatus);

        /// <summary>
        /// Can the given status be validated
        /// </summary>
        private static bool ShouldValidateAddress(AddressValidationStatusType status)
        {
            return status == AddressValidationStatusType.NotChecked ||
                   status == AddressValidationStatusType.Pending ||
                   status == AddressValidationStatusType.Error;
        }

        /// <summary>
        /// Set the validation status on the entity when we should only notify instead of update
        /// </summary>
        private static void SetValidationStatusForNotify(AddressValidationWebClientValidateAddressResult validationResult, AddressAdapter adapter)
        {
            List<AddressValidationResult> suggestedAddresses = validationResult.AddressValidationResults;

            adapter.AddressType = (int)validationResult.AddressType;

            if (!suggestedAddresses.Any())
            {
                adapter.AddressValidationStatus = (int)AddressValidationStatusType.BadAddress;    
            }
            else if (suggestedAddresses.Count == 1 && suggestedAddresses[0].IsValid && suggestedAddresses[0].IsEqualTo(adapter))
            {
                adapter.AddressValidationStatus = (int)AddressValidationStatusType.Valid;
            }
            else
            {
                adapter.AddressValidationStatus = (int)AddressValidationStatusType.HasSuggestions;
            }
        }

        /// <summary>
        /// Set the validation status on the entity
        /// </summary>
        private static void SetValidationStatus(AddressValidationWebClientValidateAddressResult validationResult, AddressAdapter adapter)
        {
            List<AddressValidationResult> suggestedAddresses = validationResult.AddressValidationResults;

            adapter.AddressType = (int)validationResult.AddressType;
            
            if (!suggestedAddresses.Any())
            {
                adapter.AddressValidationStatus = (int)AddressValidationStatusType.BadAddress;
            }
            else if (suggestedAddresses.Count == 1 && suggestedAddresses[0].IsValid)
            {
                adapter.AddressValidationStatus = suggestedAddresses[0].IsEqualTo(adapter) ?
                    (int)AddressValidationStatusType.Valid :
                    (int)AddressValidationStatusType.Fixed;
            }
            else
            {
                adapter.AddressValidationStatus = (int)AddressValidationStatusType.HasSuggestions;
            }
        }

        /// <summary>
        /// Update the addressAdapter's address to the first valid address if its status is adjusted
        /// </summary>
        private static void UpdateAddressIfAdjusted(AddressAdapter adapter, IEnumerable<AddressValidationResult> suggestedAddresses)
        {
            AddressValidationResult adjustedAddress = suggestedAddresses.FirstOrDefault(x => x.IsValid);
            if (adjustedAddress == null)
            {
                return;
            }

            if (adapter.AddressValidationStatus == (int) AddressValidationStatusType.Fixed)
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
        private static ValidatedAddressEntity CreateEntityFromValidationResult(AddressValidationResult validationResult, string fieldPrefix)
        {
            ValidatedAddressEntity address = new ValidatedAddressEntity
            {
                AddressPrefix = string.Empty
            };
            AddressAdapter adapter = new AddressAdapter(address, "");
            
            validationResult.CopyTo(adapter);
            UpdateInternationalTerritoryAndMilitaryAddress(adapter);
            address.IsOriginal = false;
            address.AddressPrefix = fieldPrefix;

            return address;
        }

        /// <summary>
        /// Updates the international territory and military address details
        /// </summary>
        private static void UpdateInternationalTerritoryAndMilitaryAddress(AddressAdapter address)
        {
            address.USTerritory = InternationalTerritoryStatus(address);
            address.MilitaryAddress = MilitaryAddressStatus(address.StateProvCode);
        }

        /// <summary>
        /// Gets the mility address status from the state code
        /// </summary>
        private static int MilitaryAddressStatus(string stateProvCode)
        {
            return (stateProvCode == "AE" || stateProvCode == "AP" || stateProvCode == "AA") ?
                (int) ValidationDetailStatusType.Yes :
                (int) ValidationDetailStatusType.No;
        }

        /// <summary>
        /// Gets the international territory status form the country code
        /// </summary>
        private static int InternationalTerritoryStatus(IAddressAdapter address)
        {
            return address.IsUSInternationalTerritory() ?
                (int) ValidationDetailStatusType.Yes :
                (int) ValidationDetailStatusType.No;
        }
    }
}
