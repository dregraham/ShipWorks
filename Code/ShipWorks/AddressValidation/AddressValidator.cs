using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Business;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Validates and updates addresses
    /// </summary>
    public class AddressValidator
    {
        private readonly IAddressValidationWebClient webClient;

        /// <summary>
        /// Creates a new instance of the AddressValidator with the default web client
        /// </summary>
        public AddressValidator() :
            this(new DummyAddressValidationWebClient())
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
        /// <param name="saveAction">Action that should save changes to the database</param>
        public void Validate(IEntity2 addressEntity, string addressPrefix, Action<AddressEntity, IEnumerable<AddressEntity>> saveAction)
        {
            PersonAdapter adapter = new PersonAdapter(addressEntity, addressPrefix);

            // If the address has already been validated, don't bother validating it again
            if (adapter.AddressValidationStatus != (int) AddressValidationStatusType.NotChecked)
            {
                return;
            }

            List<AddressValidationResult> suggestedAddresses;

            try
            {
                suggestedAddresses = webClient.ValidateAddress(adapter.Street1, adapter.Street2, adapter.City, adapter.StateProvCode, adapter.PostalCode) ??
                    new List<AddressValidationResult>();
            }
            catch (AddressValidationException)
            {
                return;
            }

            // Store the original address so that the user can revert later if they want
            AddressEntity originalAddress = new AddressEntity();
            adapter.CopyTo(originalAddress, string.Empty);

            SetValidationStatus(suggestedAddresses, adapter);
            UpdateAddressIfAdjusted(adapter, suggestedAddresses);

            saveAction(originalAddress, suggestedAddresses.Select(CreateEntityFromValidationResult));
        }

        /// <summary>
        /// Set the validation status on the entity
        /// </summary>
        private static void SetValidationStatus(List<AddressValidationResult> suggestedAddresses, PersonAdapter adapter)
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
        private static void UpdateAddressIfAdjusted(PersonAdapter adapter, IEnumerable<AddressValidationResult> suggestedAddresses)
        {
            if (adapter.AddressValidationStatus != (int) AddressValidationStatusType.Adjusted)
            {
                return;
            }

            AddressValidationResult adjustedAddress = suggestedAddresses.FirstOrDefault(x => x.IsValid);
            if (adjustedAddress != null)
            {
                adjustedAddress.CopyTo(adapter);
            }
        }

        /// <summary>
        /// Create an AddressEntity from an AddressValidationResult
        /// </summary>
        private static AddressEntity CreateEntityFromValidationResult(AddressValidationResult validationResult)
        {
            AddressEntity address = new AddressEntity();
            validationResult.CopyTo(new PersonAdapter(address, string.Empty));
            return address;
        }
    }
}
