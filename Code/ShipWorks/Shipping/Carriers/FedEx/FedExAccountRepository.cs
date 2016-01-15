using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    public class FedExAccountRepository : CarrierAccountRepositoryBase<FedExAccountEntity>, ICarrierAccountRepository<FedExAccountEntity>
    {
        /// <summary>
        /// Gets the accounts for the carrier.
        /// </summary>
        public override IEnumerable<FedExAccountEntity> Accounts => FedExAccountManager.Accounts;

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
        /// Saves the specified account.
        /// </summary>
        /// <param name="account">The account.</param>
        public override void Save(FedExAccountEntity account)
        {
            FedExAccountManager.SaveAccount(account);
        }
    }
}