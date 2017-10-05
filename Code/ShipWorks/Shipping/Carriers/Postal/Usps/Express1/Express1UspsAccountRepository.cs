using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Express1
{
    /// <summary>
    /// Account repository for Express1 Usps accounts
    /// </summary>
    [Component(RegistrationType.Self)]
    [KeyedComponent(typeof(ICarrierAccountRetriever), ShipmentTypeCode.Express1Usps)]
    [KeyedComponent(typeof(ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>), ShipmentTypeCode.Express1Usps)]
    public class Express1UspsAccountRepository : CarrierAccountRepositoryBase<UspsAccountEntity, IUspsAccountEntity>
    {
        /// <summary>
        /// Gets all the Express1 Usps accounts in the system
        /// </summary>
        public override IEnumerable<UspsAccountEntity> Accounts =>
            UspsAccountManager.Express1Accounts;

        /// <summary>
        /// Gets all the Express1 Usps accounts in the system
        /// </summary>
        public override IEnumerable<IUspsAccountEntity> AccountsReadOnly =>
            UspsAccountManager.Express1AccountsReadOnly;

        /// <summary>
        /// Force a check for changes
        /// </summary>
        public override void CheckForChangesNeeded() => UspsAccountManager.CheckForChangesNeeded();

        /// <summary>
        /// Gets the Express1 Usps account with the specified id
        /// </summary>
        /// <param name="accountID">Id of the account to retrieve</param>
        public override UspsAccountEntity GetAccount(long accountID) =>
            Accounts.ToList().FirstOrDefault(a => a.UspsAccountID == accountID);

        /// <summary>
        /// Gets the Express1 Usps account with the specified id
        /// </summary>
        /// <param name="accountID">Id of the account to retrieve</param>
        public override IUspsAccountEntity GetAccountReadOnly(long accountID) =>
            AccountsReadOnly.ToList().FirstOrDefault(a => a.UspsAccountID == accountID);

        /// <summary>
        /// Gets the default profile account.
        /// </summary>
        /// <value>
        /// The default profile account.
        /// </value>
        public override UspsAccountEntity DefaultProfileAccount
        {
            get
            {
                long? accountID = GetPrimaryProfile(ShipmentTypeCode.Express1Usps).Postal.Usps.UspsAccountID;
                return GetProfileAccount(ShipmentTypeCode.Express1Usps, accountID);
            }
        }

        /// <summary>
        /// Saves the specified account.
        /// </summary>
        /// <param name="account">The account.</param>
        public override void Save(UspsAccountEntity account) => UspsAccountManager.SaveAccount(account);

        /// <summary>
        /// Deletes the account.
        /// </summary>
        /// <param name="account">The account.</param>
        public override void DeleteAccount(UspsAccountEntity account) => UspsAccountManager.DeleteAccount(account);

        /// <summary>
        /// Saves the specified account.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="account">The account.</param>
        public override void Save<T>(T account) => Save(account as UspsAccountEntity);

        /// <summary>
        /// Deletes the account.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="account">The account.</param>
        public override void DeleteAccount<T>(T account) => DeleteAccount(account as UspsAccountEntity);

        /// <summary>
        /// Get the account id from a given shipment
        /// </summary>
        protected override long? GetAccountIDFromShipment(IShipmentEntity shipment) =>
            shipment?.Postal?.Usps?.UspsAccountID;
    }
}
