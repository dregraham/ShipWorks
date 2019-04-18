using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Amazon.SWA;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShipWorks.Shipping.Carriers.Amazon.SWA
{
    /// <summary>
    /// Account Repository for AmazonSWA
    /// </summary>
    [Component]
    [KeyedComponent(typeof(ICarrierAccountRetriever), ShipmentTypeCode.AmazonSWA)]
    [KeyedComponent(typeof(ICarrierAccountRepository<AmazonSWAAccountEntity, IAmazonSWAAccountEntity>), ShipmentTypeCode.AmazonSWA)]
    public class AmazonSWAAccountRepository : CarrierAccountRepositoryBase<AmazonSWAAccountEntity, IAmazonSWAAccountEntity>, IAmazonSWAAccountRepository
    {
        /// <summary>
        /// Gets the accounts for the carrier.
        /// </summary>
        public override IEnumerable<AmazonSWAAccountEntity> Accounts =>
            AmazonSWAAccountManager.Accounts;

        /// <summary>
        /// Gets the default profile account.
        /// </summary>
        public override AmazonSWAAccountEntity DefaultProfileAccount => null;

        /// <summary>
        /// Gets the accounts for the carrier.
        /// </summary>
        public override IEnumerable<IAmazonSWAAccountEntity> AccountsReadOnly =>
            AmazonSWAAccountManager.AccountsReadOnly;

        /// <summary>
        /// Force a check for changes
        /// </summary>
        public override void CheckForChangesNeeded() =>
            AmazonSWAAccountManager.CheckForChangesNeeded();

        /// <summary>
        /// Returns a carrier account for the provided accountID.
        /// </summary>
        public override AmazonSWAAccountEntity GetAccount(long accountID) =>
            AmazonSWAAccountManager.GetAccount(accountID);

        /// <summary>
        /// Returns a carrier account for the provided accountID.
        /// </summary>
        public override IAmazonSWAAccountEntity GetAccountReadOnly(long accountID) =>
            AmazonSWAAccountManager.GetAccountReadOnly(accountID);

        /// <summary>
        /// Saves the specified account.
        /// </summary>
        public override void Save(AmazonSWAAccountEntity account) => AmazonSWAAccountManager.SaveAccount(account);

        /// <summary>
        /// Deletes the account.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void DeleteAccount(AmazonSWAAccountEntity account) => AmazonSWAAccountManager.DeleteAccount(account);

        /// <summary>
        /// Saves the specified account.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="account">The account.</param>
        public override void Save<T>(T account) => Save(account as AmazonSWAAccountEntity);

        /// <summary>
        /// Deletes the account.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="account">The account.</param>
        public override void DeleteAccount<T>(T account) => DeleteAccount(account as AmazonSWAAccountEntity);

        /// <summary>
        /// Get the account id from a given shipment
        /// </summary>
        protected override long? GetAccountIDFromShipment(IShipmentEntity shipment)
        {
            return shipment.AmazonSWA.AmazonSWAAccountID;
        }
    }
}
