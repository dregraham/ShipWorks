using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Business;
using SD.LLBLGen.Pro.LinqSupportClasses;
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
            if (adapter.AddressValidationStatus != (int)AddressValidationStatusType.NotChecked)
            {
                return;
            }

            List<AddressValidationResult> suggestedAddresses = webClient.ValidateAddress(adapter.Street1, adapter.Street2, adapter.City, adapter.StateProvCode, adapter.PostalCode) ??
                new List<AddressValidationResult>();

            if (!suggestedAddresses.Any())
            {
                adapter.AddressValidationStatus = (int)AddressValidationStatusType.NotValid;   
            }

            // Store the original address so that the user can revert later if they want
            AddressEntity originalAddress = new AddressEntity();
            adapter.CopyTo(originalAddress, string.Empty);

            saveAction(originalAddress, suggestedAddresses.Select(CreateEntityFromValidationResult));
        }

        /// <summary>
        /// Create an AddressEntity from an AddressValidationResult
        /// </summary>
        private static AddressEntity CreateEntityFromValidationResult(AddressValidationResult validationResult)
        {
            PersonAdapter adapter = new PersonAdapter
            {
                Street1 = validationResult.Street1,
                Street2 = validationResult.Street2,
                Street3 = validationResult.Street3,
                City = validationResult.City,
                StateProvCode = validationResult.StateProvCode,
                PostalCode = validationResult.PostalCode,
                CountryCode = validationResult.CountryCode
            };

            AddressEntity address = new AddressEntity();
            adapter.CopyTo(address, string.Empty);
            return address;
        }
    }
}
