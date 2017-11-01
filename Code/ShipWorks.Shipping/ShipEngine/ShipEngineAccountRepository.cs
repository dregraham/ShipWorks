﻿using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// Repository of ShipEngine accounts
    /// </summary>
    [Component]
    public abstract class ShipEngineAccountRepository : CarrierAccountRepositoryBase<ShipEngineAccountEntity, IShipEngineAccountEntity>, IShipEngineAccountRepository

    {
        protected abstract ShipmentTypeCode shipmentType { get; }


        /// <summary>
        /// Gets the accounts for the carrier.
        /// </summary>
        public override IEnumerable<ShipEngineAccountEntity> Accounts =>
            ShipEngineAccountManager.Accounts.Where(a=>a.ShipmentTypeCode == (int) shipmentType);

        /// <summary>
        /// Gets the accounts for the carrier.
        /// </summary>
        public override IEnumerable<IShipEngineAccountEntity> AccountsReadOnly =>
            ShipEngineAccountManager.AccountsReadOnly.Where(a => a.ShipmentTypeCode == (int) shipmentType);

        /// <summary>
        /// Force a check for changes
        /// </summary>
        public override void CheckForChangesNeeded() =>
            ShipEngineAccountManager.CheckForChangesNeeded();

        /// <summary>
        /// Returns a carrier account for the provided accountID.
        /// </summary>
        public override ShipEngineAccountEntity GetAccount(long accountID) =>
            ShipEngineAccountManager.GetAccount(accountID);

        /// <summary>
        /// Returns a carrier account for the provided accountID.
        /// </summary>
        public override IShipEngineAccountEntity GetAccountReadOnly(long accountID) =>
            ShipEngineAccountManager.GetAccountReadOnly(accountID);

        /// <summary>
        /// Saves the specified account.
        /// </summary>
        public override void Save(ShipEngineAccountEntity account) => ShipEngineAccountManager.SaveAccount(account);

        /// <summary>
        /// Deletes the account.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void DeleteAccount(ShipEngineAccountEntity account) => ShipEngineAccountManager.DeleteAccount(account);

        /// <summary>
        /// Saves the specified account.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="account">The account.</param>
        public override void Save<T>(T account) => Save(account as ShipEngineAccountEntity);

        /// <summary>
        /// Deletes the account.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="account">The account.</param>
        public override void DeleteAccount<T>(T account) => DeleteAccount(account as ShipEngineAccountEntity);
    }
}
