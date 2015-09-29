using Interapptive.Shared.Business;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Shipping;

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
        public ShipmentTypeCode ShipmentType => EndiciaReseller == 1 ? ShipmentTypeCode.Express1Endicia : ShipmentTypeCode.Endicia;

        /// <summary>
        /// Get the address of the account
        /// </summary>
        public PersonAdapter Address => new PersonAdapter(this, string.Empty);

        /// <summary>
        /// Applies account to shipment
        /// </summary>
        public void ApplyTo(ShipmentEntity shipment) => 
            shipment.Postal.Endicia.EndiciaAccountID = AccountId;
    }
}
