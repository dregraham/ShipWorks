using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.DhlEcommerce
{
    [Component]
    [KeyedComponent(typeof(ICarrierAccountRetriever), ShipmentTypeCode.DhlEcommerce)]
    [KeyedComponent(typeof(ICarrierAccountRepository<DhlEcommerceAccountEntity, IDhlEcommerceAccountEntity>), ShipmentTypeCode.DhlEcommerce)]
    public class DhlEcommerceAccountRepository : CarrierAccountRepositoryBase<DhlEcommerceAccountEntity, IDhlEcommerceAccountEntity>, IDhlEcommerceAccountRepository
    {
        /// <summary>
        /// Gets the accounts for the carrier.
        /// </summary>
        public override IEnumerable<DhlEcommerceAccountEntity> Accounts
        {
            get
            {
                return DhlEcommerceAccountManager.Accounts;
            }
        }

        /// <summary>
        /// Gets the default profile account.
        /// </summary>
        public override DhlEcommerceAccountEntity DefaultProfileAccount => null;

        /// <summary>
        /// Gets the accounts for the carrier.
        /// </summary>
        public override IEnumerable<IDhlEcommerceAccountEntity> AccountsReadOnly
        {
            get
            {
                return DhlEcommerceAccountManager.AccountsReadOnly;
            }
        }

        /// <summary>
        /// Force a check for changes
        /// </summary>
        public override void CheckForChangesNeeded()
        {
            DhlEcommerceAccountManager.CheckForChangesNeeded();
        }

        /// <summary>
        /// Initialize
        /// </summary>
        public override void Initialize()
        {
            DhlEcommerceAccountManager.Initialize();
        }

        /// <summary>
        /// Returns a carrier account for the provided accountID.
        /// </summary>
        public override DhlEcommerceAccountEntity GetAccount(long accountID)
        {
            return DhlEcommerceAccountManager.GetAccount(accountID);
        }

        /// <summary>
        /// Returns a carrier account for the provided accountID.
        /// </summary>
        public override IDhlEcommerceAccountEntity GetAccountReadOnly(long accountID)
        {
            return DhlEcommerceAccountManager.GetAccountReadOnly(accountID);
        }

        /// <summary>
        /// Saves the specified account.
        /// </summary>
        public override void Save(DhlEcommerceAccountEntity account)
        {
            DhlEcommerceAccountManager.SaveAccount(account);
        }

        /// <summary>
        /// Deletes the account.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void DeleteAccount(DhlEcommerceAccountEntity account)
        {
            DhlEcommerceAccountManager.DeleteAccount(account);
        }

        /// <summary>
        /// Saves the specified account.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="account">The account.</param>
        public override void Save<T>(T account) => Save(account as DhlEcommerceAccountEntity);

        /// <summary>
        /// Deletes the account.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="account">The account.</param>
        public override void DeleteAccount<T>(T account) => DeleteAccount(account as DhlEcommerceAccountEntity);

        /// <summary>
        /// Get the account id from a given shipment
        /// </summary>
        protected override long? GetAccountIDFromShipment(IShipmentEntity shipment)
        {
            return shipment.DhlEcommerce.DhlEcommerceAccountID;
        }
    }
}
