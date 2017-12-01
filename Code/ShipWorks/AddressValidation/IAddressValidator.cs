using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.Business;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.Data.Model.EntityClasses;

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
        /// Can a message be shown for the given validation status
        /// </summary>
        bool CanShowMessage(AddressValidationStatusType validationStatus);

        /// <summary>
        /// Validates an address with no prefix on the specified entity
        /// </summary>
        /// <param name="addressAdapter">Address that should be validated</param>
        /// <param name="store">The store that owns the order whose address is being validated</param>
        /// <param name="canAdjustAddress"></param>
        /// <param name="saveAction">Action that should save changes to the database</param>
        Task ValidateAsync(AddressAdapter addressAdapter, StoreEntity store, bool canAdjustAddress,  Action<ValidatedAddressEntity, IEnumerable<ValidatedAddressEntity>> saveAction);

        /// <summary>
        /// Validates an address with no prefix on the specified entity
        /// </summary>
        /// <param name="addressAdapter">Address that should be validated</param>
        /// <param name="entityId">The entityid that owns the order whose address is being validated</param>
        /// <param name="canAdjustAddress"></param>
        Task<ValidatedAddressData> ValidateAsync(AddressAdapter addressAdapter, long entityId, bool canAdjustAddress);

        /// <summary>
        /// Validates an address with no prefix on the specified entity
        /// </summary>
        /// <param name="addressAdapter">Address that should be validated</param>
        /// <param name="store">The store that owns the order whose address is being validated</param>
        /// <param name="canAdjustAddress"></param>
        /// <param name="saveAction">Action that should save changes to the database</param>
        Task<ValidatedAddressData> ValidateAsync(AddressAdapter addressAdapter, StoreEntity store, bool canAdjustAddress);
    }
}