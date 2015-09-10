﻿using ShipWorks.Data.Model.Custom;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Partial class for builtin EndiciaAccountEntity
    /// </summary>
    public partial class EndiciaAccountEntity : ICarrierAccount
    {
        /// <summary>
        /// Indicates if this account was migrated from ShipWorks2, and has not yet been updated for the new API
        /// </summary>
        public bool IsDazzleMigrationPending => 
            string.IsNullOrEmpty(AccountNumber) && string.IsNullOrEmpty(SignupConfirmation);

        /// <summary>
        /// Gets the account id in a generic way
        /// </summary>
        public long AccountId => EndiciaAccountID;

        /// <summary>
        /// Get the shipment type to which this account applies
        /// </summary>
        /// <remarks>Assume Endicia unless explicitly marked as Express1</remarks>
        public int ShipmentType => EndiciaReseller == 1 ? 9 : 2;
    }
}
