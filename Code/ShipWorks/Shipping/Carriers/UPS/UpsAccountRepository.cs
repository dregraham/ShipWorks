using System;
using System.Collections.Generic;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.UPS.OpenAccount;

namespace ShipWorks.Shipping.Carriers.UPS
{
    [Component]
    [KeyedComponent(typeof(ICarrierAccountRetriever), ShipmentTypeCode.UpsOnLineTools)]
    public class UpsAccountRepository : CarrierAccountRepositoryBase<UpsAccountEntity, IUpsAccountEntity>, IUpsOpenAccountRepository
    {
        /// <summary>
        /// Returns a list of accounts for the carrier.
        /// </summary>
        public override IEnumerable<UpsAccountEntity> Accounts => UpsAccountManager.Accounts;

        /// <summary>
        /// Returns a read only list of accounts
        /// </summary>
        public override IEnumerable<IUpsAccountEntity> AccountsReadOnly => UpsAccountManager.AccountsReadOnly;

        /// <summary>
        /// Force a check for changes
        /// </summary>
        public override void CheckForChangesNeeded() => UpsAccountManager.CheckForChangesNeeded();

        /// <summary>
        /// Returns a carrier account for the provided accountID.
        /// </summary>
        /// <param name="accountID">The account ID for which to return an account.</param>
        /// <returns>The matching account as IEntity2.</returns>
        public override UpsAccountEntity GetAccount(long accountID)
        {
            return UpsAccountManager.GetAccount(accountID);
        }

        /// <summary>
        /// Gets a read only version of the specified account
        /// </summary>
        /// <param name="uspsAccountID"></param>
        /// <returns></returns>
        public override IUpsAccountEntity GetAccountReadOnly(long accountID) =>
            UpsAccountManager.GetAccountReadOnly(accountID);

        /// <summary>
        ///  Returns the default account as defined by the primary profile
        ///  </summary>
        public override UpsAccountEntity DefaultProfileAccount
        {
            get
            {
                long? accountID = GetPrimaryProfile(ShipmentTypeCode.UpsOnLineTools).Ups.UpsAccountID;
                return GetProfileAccount(ShipmentTypeCode.UpsOnLineTools, accountID);
            }
        }

        /// <summary>
        /// Saves the given UPS account entity to the underlying data source.
        /// </summary>
        /// <param name="account">The account.</param>
        public override void Save(UpsAccountEntity account)
        {
            if (account == null)
            {
                throw new ArgumentNullException("account");
            }

            UpsAccountManager.SaveAccount(account);
        }

        /// <summary>
        /// Get the account id from a given shipment
        /// </summary>
        protected override long? GetAccountIDFromShipment(IShipmentEntity shipment) =>
            shipment?.Ups?.UpsAccountID;
    }
}
