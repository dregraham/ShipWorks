﻿using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using System.Collections.Generic;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// FedEx account repository
    /// </summary>
    [Component]
    [KeyedComponent(typeof(ICarrierAccountRetriever), ShipmentTypeCode.FedEx)]
    [KeyedComponent(typeof(ICarrierAccountRepository<FedExAccountEntity, IFedExAccountEntity>), ShipmentTypeCode.FedEx)]
    public class FedExAccountRepository : CarrierAccountRepositoryBase<FedExAccountEntity, IFedExAccountEntity>, IFedExAccountRepository
    {
        /// <summary>
        /// Gets the accounts for the carrier.
        /// </summary>
        public override IEnumerable<FedExAccountEntity> Accounts => FedExAccountManager.Accounts;

        /// <summary>
        /// Force a check for changes
        /// </summary>
        public override void CheckForChangesNeeded() => FedExAccountManager.CheckForChangesNeeded();

        /// <summary>
        /// Initialize
        /// </summary>
        public override void Initialize()
        {
            FedExAccountManager.InitializeForCurrentSession();
        }

        /// <summary>
        /// Returns a carrier account for the provided accountID.
        /// </summary>
        /// <param name="accountID">The account ID for which to return an account.</param>
        /// <returns>The matching account as IEntity2.</returns>
        public override FedExAccountEntity GetAccount(long accountID)
        {
            return FedExAccountManager.GetAccount(accountID);
        }

        /// <summary>
        /// Gets the account associated withe the default profile. A null value is returned
        /// if there is not an account associated with the default profile.
        /// </summary>
        /// <exception cref="ShippingException">An account that no longer exists is associated with the default FedEx profile.</exception>
        public override FedExAccountEntity DefaultProfileAccount
        {
            get
            {
                long? accountID = GetPrimaryProfile(ShipmentTypeCode.FedEx).FedEx.FedExAccountID;
                return GetProfileAccount(ShipmentTypeCode.FedEx, accountID);
            }
        }

        /// <summary>
        /// Get a collection of readonly accounts
        /// </summary>
        public override IEnumerable<IFedExAccountEntity> AccountsReadOnly => FedExAccountManager.AccountsReadOnly;

        /// <summary>
        /// Saves the specified account.
        /// </summary>
        /// <param name="account">The account.</param>
        public override void Save(FedExAccountEntity account) => FedExAccountManager.SaveAccount(account);

        /// <summary>
        /// Deletes the account.
        /// </summary>
        /// <param name="account">The account.</param>
        public override void DeleteAccount(FedExAccountEntity account) => FedExAccountManager.DeleteAccount(account);

        /// <summary>
        /// Saves the specified account.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="account">The account.</param>
        public override void Save<T>(T account) => Save(account as FedExAccountEntity);

        /// <summary>
        /// Deletes the account.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="account">The account.</param>
        public override void DeleteAccount<T>(T account) => DeleteAccount(account as FedExAccountEntity);

        /// <summary>
        /// Get a readonly version of the specified account
        /// </summary>
        public override IFedExAccountEntity GetAccountReadOnly(long accountID) =>
            FedExAccountManager.GetAccountReadOnly(accountID);

        /// <summary>
        /// Get the account id from a given shipment
        /// </summary>
        protected override long? GetAccountIDFromShipment(IShipmentEntity shipment) =>
            shipment?.FedEx?.FedExAccountID;
    }
}