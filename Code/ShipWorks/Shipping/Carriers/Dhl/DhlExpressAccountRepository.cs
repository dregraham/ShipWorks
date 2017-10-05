using System;
using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    [Component]
    [KeyedComponent(typeof(ICarrierAccountRetriever), ShipmentTypeCode.DhlExpress)]
    [KeyedComponent(typeof(ICarrierAccountRepository<DhlExpressAccountEntity, IDhlExpressAccountEntity>), ShipmentTypeCode.DhlExpress)]
    public class DhlExpressAccountRepository : CarrierAccountRepositoryBase<DhlExpressAccountEntity, IDhlExpressAccountEntity>, IDhlExpressAccountRepository
    {
        /// <summary>
        /// Gets the accounts for the carrier.
        /// </summary>
        public override IEnumerable<DhlExpressAccountEntity> Accounts =>
            DhlExpressAccountManager.Accounts;

        /// <summary>
        /// Gets the default profile account.
        /// </summary>
        public override DhlExpressAccountEntity DefaultProfileAccount => null;

        /// <summary>
        /// Gets the accounts for the carrier.
        /// </summary>
        public override IEnumerable<IDhlExpressAccountEntity> AccountsReadOnly =>
            DhlExpressAccountManager.AccountsReadOnly;

        /// <summary>
        /// Force a check for changes
        /// </summary>
        public override void CheckForChangesNeeded() =>
            DhlExpressAccountManager.CheckForChangesNeeded();

        /// <summary>
        /// Returns a carrier account for the provided accountID.
        /// </summary>
        public override DhlExpressAccountEntity GetAccount(long accountID) =>
            DhlExpressAccountManager.GetAccount(accountID);

        /// <summary>
        /// Returns a carrier account for the provided accountID.
        /// </summary>
        public override IDhlExpressAccountEntity GetAccountReadOnly(long accountID) =>
            DhlExpressAccountManager.GetAccountReadOnly(accountID);

        /// <summary>
        /// Saves the specified account.
        /// </summary>
        public override void Save(DhlExpressAccountEntity account) => DhlExpressAccountManager.SaveAccount(account);

        /// <summary>
        /// Deletes the account.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void DeleteAccount(DhlExpressAccountEntity account) => DhlExpressAccountManager.DeleteAccount(account);

        /// <summary>
        /// Saves the specified account.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="account">The account.</param>
        public override void Save<T>(T account) => Save(account as DhlExpressAccountEntity);

        /// <summary>
        /// Deletes the account.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="account">The account.</param>
        public override void DeleteAccount<T>(T account) => DeleteAccount(account as DhlExpressAccountEntity);

        /// <summary>
        /// Get the account id from a given shipment
        /// </summary>
        protected override long? GetAccountIDFromShipment(IShipmentEntity shipment)
        {
            throw new NotImplementedException();
        }
    }
}
