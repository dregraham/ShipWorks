using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShipWorks.Shipping.Carriers.Asendia
{
    /// <summary>
    /// Account Repository for Asendia
    /// </summary>
    [Component]
    [KeyedComponent(typeof(ICarrierAccountRetriever), ShipmentTypeCode.Asendia)]
    [KeyedComponent(typeof(ICarrierAccountRepository<AsendiaAccountEntity, IAsendiaAccountEntity>), ShipmentTypeCode.Asendia)]
    public class AsendiaAccountRepository : CarrierAccountRepositoryBase<AsendiaAccountEntity, IAsendiaAccountEntity>, IAsendiaAccountRepository
    {
        /// <summary>
        /// Gets the accounts for the carrier.
        /// </summary>
        public override IEnumerable<AsendiaAccountEntity> Accounts =>
            AsendiaAccountManager.Accounts;

        /// <summary>
        /// Gets the default profile account.
        /// </summary>
        public override AsendiaAccountEntity DefaultProfileAccount => null;

        /// <summary>
        /// Gets the accounts for the carrier.
        /// </summary>
        public override IEnumerable<IAsendiaAccountEntity> AccountsReadOnly =>
            AsendiaAccountManager.AccountsReadOnly;

        /// <summary>
        /// Force a check for changes
        /// </summary>
        public override void CheckForChangesNeeded() =>
            AsendiaAccountManager.CheckForChangesNeeded();

        /// <summary>
        /// Returns a carrier account for the provided accountID.
        /// </summary>
        public override AsendiaAccountEntity GetAccount(long accountID) =>
            AsendiaAccountManager.GetAccount(accountID);

        /// <summary>
        /// Returns a carrier account for the provided accountID.
        /// </summary>
        public override IAsendiaAccountEntity GetAccountReadOnly(long accountID) =>
            AsendiaAccountManager.GetAccountReadOnly(accountID);

        /// <summary>
        /// Saves the specified account.
        /// </summary>
        public override void Save(AsendiaAccountEntity account) => AsendiaAccountManager.SaveAccount(account);

        /// <summary>
        /// Deletes the account.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void DeleteAccount(AsendiaAccountEntity account) => AsendiaAccountManager.DeleteAccount(account);

        /// <summary>
        /// Saves the specified account.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="account">The account.</param>
        public override void Save<T>(T account) => Save(account as AsendiaAccountEntity);

        /// <summary>
        /// Deletes the account.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="account">The account.</param>
        public override void DeleteAccount<T>(T account) => DeleteAccount(account as AsendiaAccountEntity);

        /// <summary>
        /// Get the account id from a given shipment
        /// </summary>
        protected override long? GetAccountIDFromShipment(IShipmentEntity shipment)
        {
            throw new NotImplementedException("Should return shipment.Asendia.AsendiaAccountID");
        }

    }
}
