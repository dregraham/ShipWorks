using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// Basic repository for retrieving Endicia accounts
    /// </summary>
    [Component]
    [KeyedComponent(typeof(ICarrierAccountRetriever), ShipmentTypeCode.Endicia)]
    [KeyedComponent(typeof(ICarrierAccountRepository<EndiciaAccountEntity, IEndiciaAccountEntity>), ShipmentTypeCode.Endicia)]
    public class EndiciaAccountRepository : CarrierAccountRepositoryBase<EndiciaAccountEntity, IEndiciaAccountEntity>
    {
        /// <summary>
        /// Returns a list of Endicia accounts.
        /// </summary>
        public override IEnumerable<EndiciaAccountEntity> Accounts => EndiciaAccountManager.EndiciaAccounts.ToList();

        /// <summary>
        /// Returns a list of Endicia accounts.
        /// </summary>
        public override IEnumerable<IEndiciaAccountEntity> AccountsReadOnly => EndiciaAccountManager.EndiciaAccountsReadOnly;

        /// <summary>
        /// Force a check for changes
        /// </summary>
        public override void CheckForChangesNeeded() => EndiciaAccountManager.CheckForChangesNeeded();

        /// <summary>
        /// Returns a Endicia account for the provided accountID.
        /// </summary>
        /// <param name="accountID">The account ID for which to return an account.</param>
        /// <returns>The matching account.</returns>
        public override EndiciaAccountEntity GetAccount(long accountID)
        {
            EndiciaAccountEntity endiciaAccountEntity = EndiciaAccountManager.GetAccount(accountID);

            // return null if found account is not Endicia
            return endiciaAccountEntity != null && endiciaAccountEntity.EndiciaReseller == (int) EndiciaReseller.None ? endiciaAccountEntity : null;
        }

        /// <summary>
        /// Returns a Endicia account for the provided accountID.
        /// </summary>
        /// <param name="accountID">The account ID for which to return an account.</param>
        /// <returns>The matching account.</returns>
        public override IEndiciaAccountEntity GetAccountReadOnly(long accountID)
        {
            IEndiciaAccountEntity endiciaAccountEntity = EndiciaAccountManager.GetAccountReadOnly(accountID);

            // return null if found account is not Endicia
            return endiciaAccountEntity != null && endiciaAccountEntity.EndiciaReseller == (int) EndiciaReseller.None ? endiciaAccountEntity : null;
        }

        /// <summary>
        /// Gets the account associated withe the default profile. A null value is returned
        /// if there is not an account associated with the default profile.
        /// </summary>
        /// <exception cref="ShippingException">An account that no longer exists is associated with the default FedEx profile.</exception>
        public override EndiciaAccountEntity DefaultProfileAccount
        {
            get
            {
                long? accountID = GetPrimaryProfile(ShipmentTypeCode.Endicia).Postal.Endicia.EndiciaAccountID;
                return GetProfileAccount(ShipmentTypeCode.Endicia, accountID);
            }
        }

        /// <summary>
        /// Saves the specified account.
        /// </summary>
        /// <param name="account">The account.</param>
        public override void Save(EndiciaAccountEntity account) => EndiciaAccountManager.SaveAccount(account);

        /// <summary>
        /// Deletes the specified account.
        /// </summary>
        /// <param name="account">The account.</param>
        public override void DeleteAccount(EndiciaAccountEntity account) => EndiciaAccountManager.DeleteAccount(account);

        /// <summary>
        /// Saves the specified account.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="account">The account.</param>
        public override void Save<T>(T account) => Save(account as EndiciaAccountEntity);

        /// <summary>
        /// Deletes the account.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="account">The account.</param>
        public override void DeleteAccount<T>(T account) => Save(account as EndiciaAccountEntity);

        /// <summary>
        /// Get the account id from a given shipment
        /// </summary>
        protected override long? GetAccountIDFromShipment(IShipmentEntity shipment) =>
            shipment?.Postal?.Endicia?.EndiciaAccountID;
    }
}
