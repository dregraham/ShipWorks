using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    /// <summary>
    /// Account repository for OnTrac
    /// </summary>
    [Component]
    [KeyedComponent(typeof(ICarrierAccountRetriever), ShipmentTypeCode.OnTrac)]
    public class OnTracAccountRepository : CarrierAccountRepositoryBase<OnTracAccountEntity, IOnTracAccountEntity>
    {
        /// <summary>
        /// Gets the accounts for the carrier.
        /// </summary>
        public override IEnumerable<OnTracAccountEntity> Accounts => OnTracAccountManager.Accounts;

        /// <summary>
        /// Gets the accounts for the carrier.
        /// </summary>
        public override IEnumerable<IOnTracAccountEntity> AccountsReadOnly => OnTracAccountManager.AccountsReadOnly;

        /// <summary>
        /// Force a check for changes
        /// </summary>
        public override void CheckForChangesNeeded() => OnTracAccountManager.CheckForChangesNeeded();

        /// <summary>
        /// Returns a carrier account for the provided accountID.
        /// </summary>
        /// <param name="accountID">The account ID for which to return an account.</param>
        /// <returns>The matching account as IEntity2.</returns>
        public override OnTracAccountEntity GetAccount(long accountID)
        {
            return OnTracAccountManager.GetAccount(accountID);
        }

        /// <summary>
        /// Returns a carrier account for the provided accountID.
        /// </summary>
        /// <param name="accountID">The account ID for which to return an account.</param>
        /// <returns>The matching account as IEntity2.</returns>
        public override IOnTracAccountEntity GetAccountReadOnly(long accountID)
        {
            return OnTracAccountManager.GetAccountReadOnly(accountID);
        }

        /// <summary>
        /// Gets the default profile account.
        /// </summary>
        /// <value>
        /// The default profile account.
        /// </value>
        public override OnTracAccountEntity DefaultProfileAccount
        {
            get
            {
                long? accountID = GetPrimaryProfile(ShipmentTypeCode.OnTrac).OnTrac.OnTracAccountID;
                return GetProfileAccount(ShipmentTypeCode.OnTrac, accountID);
            }
        }

        /// <summary>
        /// Saves the specified account.
        /// </summary>
        /// <param name="account">The account.</param>
        public override void Save(OnTracAccountEntity account) => OnTracAccountManager.SaveAccount(account);

        /// <summary>
        /// Deletes the account.
        /// </summary>
        /// <param name="account">The account.</param>
        public override void DeleteAccount(OnTracAccountEntity account) => OnTracAccountManager.DeleteAccount(account);

        /// <summary>
        /// Get the account id from a given shipment
        /// </summary>
        protected override long? GetAccountIDFromShipment(IShipmentEntity shipment) =>
            shipment?.OnTrac?.OnTracAccountID;
    }
}
