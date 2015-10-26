using System;
using System.Threading.Tasks;
using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using System.Collections.Generic;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Validates and updates addresses
    /// </summary>
    public interface IAddressValidator
    {
        /// <summary>
        /// Can the given status be validated
        /// </summary>
        bool CanValidate(AddressValidationStatusType validationStatus);

        /// <summary>
        /// Can suggestions be shown for the given validation status
        /// </summary>
        bool CanShowSuggestions(AddressValidationStatusType validationStatus);

        /// <summary>
        /// Can a message be shown fo the given validation status
        /// </summary>
        bool CanShowMessage(AddressValidationStatusType validationStatus);
        
        /// <summary>
        /// Validates an address with no prefix on the specified entity
        /// </summary>
        /// <param name="addressAdapter">Address that should be validated</param>
        /// <param name="canAdjustAddress"></param>
        /// <param name="saveAction">Action that should save changes to the database</param>
        Task ValidateAsync(AddressAdapter addressAdapter, bool canAdjustAddress, Action<ValidatedAddressEntity, IEnumerable<ValidatedAddressEntity>> saveAction);

        /// <summary>
        /// Validates an address with no prefix on the specified entity
        /// </summary>
        /// <param name="addressAdapter">Address that should be validated</param>
        /// <param name="canAdjustAddress"></param>
        /// <param name="saveAction">Action that should save changes to the database</param>
        Task<ValidatedAddressData> ValidateAsync(AddressAdapter addressAdapter, bool canAdjustAddress);
    }
}