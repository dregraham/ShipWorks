using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using System.Collections.Generic;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Interaction with validated address suggestions
    /// </summary>
    public interface IValidatedAddressScope
    {
        /// <summary>
        /// Store a collection of addresses that should be saved
        /// </summary>
        void StoreAddresses(long entityId, IEnumerable<ValidatedAddressEntity> addresses, string fieldPrefix);

        /// <summary>
        /// Save the stored validated addresses to the database
        /// </summary>
        void FlushAddressesToDatabase(SqlAdapter sqlAdapter, long entityId, string prefix);

        /// <summary>
        /// Save the stored validated addresses to the database
        /// </summary>
        void FlushAddressesToDatabase(IAddressValidationDataAccess dataAccess, long entityId, string prefix);
    }
}