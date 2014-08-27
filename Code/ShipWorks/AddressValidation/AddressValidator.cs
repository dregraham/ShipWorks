﻿using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Business;
using log4net;
using Microsoft.Web.Services3.Addressing;
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
        public void Validate(IEntity2 addressEntity, string addressPrefix, bool canAdjustAddress, Action<ValidatedAddressEntity, IEnumerable<ValidatedAddressEntity>> saveAction)
        {
            Validate(new AddressAdapter(addressEntity, addressPrefix), canAdjustAddress, saveAction);
        }

        /// <summary>
        /// Validates an address with no prefix on the specified entity
        /// </summary>
        /// <param name="addressAdapter">Address that should be validated</param>
        /// <param name="canAdjustAddress"></param>
        /// <param name="saveAction">Action that should save changes to the database</param>
        public void Validate(AddressAdapter addressAdapter, bool canAdjustAddress, Action<ValidatedAddressEntity, IEnumerable<ValidatedAddressEntity>> saveAction)
        {
            // We don't want to validate already validated addresses because we'll lose the original address
            if (!ShouldValidateAddress(addressAdapter))
            {
                return;
            }

            try
            {
                AddressValidationWebClientValidateAddressResult validationResult = webClient.ValidateAddress(addressAdapter.Street1, addressAdapter.Street2, addressAdapter.City, addressAdapter.StateProvCode, addressAdapter.PostalCode);

                // Store the original address so that the user can revert later if they want
                ValidatedAddressEntity originalAddress = new ValidatedAddressEntity();
                addressAdapter.CopyTo(originalAddress, string.Empty);
                originalAddress.IsOriginal = true;
                originalAddress.AddressPrefix = addressAdapter.FieldPrefix;

                addressAdapter.AddressValidationError = validationResult.AddressValidationError;

                // Set the validation status based on the settings of the store
                if (canAdjustAddress)
                {
                    SetValidationStatus(validationResult.AddressValidationResults, addressAdapter);
                    UpdateAddressIfAdjusted(addressAdapter, validationResult.AddressValidationResults);
                }
                else
                {
                    SetValidationStatusForNotify(validationResult.AddressValidationResults, addressAdapter);

                    AddressValidationResult validatedAddress = validationResult.AddressValidationResults.FirstOrDefault(x => x.IsValid);
                    if (validatedAddress != null)
                    {
                        addressAdapter.ResidentialStatus = (int) validatedAddress.ResidentialStatus;
                        addressAdapter.POBox = (int) validatedAddress.POBox;
                        addressAdapter.USTerritory = InternationalTerritoryStatus(validatedAddress.StateProvCode, validatedAddress.CountryCode);
                        addressAdapter.MilitaryAddress = MilitaryAddressStatus(validatedAddress.StateProvCode);
                    }
                }

                addressAdapter.AddressValidationSuggestionCount = validationResult.AddressValidationResults.Count;

                if (validationResult.AddressValidationResults.Count > 0)
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
                addressAdapter.AddressValidationError = "Error communicating with Address Validation Server.";
                addressAdapter.AddressValidationStatus = (int)AddressValidationStatusType.Error;
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
        /// Update the addressAdapter's address to the first valid address if its status is adjusted
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
            address.USTerritory = InternationalTerritoryStatus(address.StateProvCode, address.CountryCode);
            address.MilitaryAddress = MilitaryAddressStatus(address.StateProvCode);
        }

        /// <summary>
        /// Gets the mility address status from the state code
        /// </summary>
        private static int MilitaryAddressStatus(string stateProvCode)
        {
            return (stateProvCode == "AE" || stateProvCode == "AP" || stateProvCode == "AA") ?
                (int) ValidationDetailStatusType.True :
                (int) ValidationDetailStatusType.False;
        }

        /// <summary>
        /// Gets the international territory status form the country code
        /// </summary>
        private static int InternationalTerritoryStatus(string stateProvCode, string countryCode)
        {
            return PostalUtility.IsUSInternationalTerritory(countryCode) || PostalUtility.IsUSInternationalTerritory(stateProvCode) ?
                (int) ValidationDetailStatusType.True :
                (int) ValidationDetailStatusType.False;
        }
    }
}
