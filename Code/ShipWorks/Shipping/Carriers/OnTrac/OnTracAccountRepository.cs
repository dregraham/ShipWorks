using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    public class OnTracAccountRepository : CarrierAccountRepositoryBase<OnTracAccountEntity>, ICarrierAccountRepository<OnTracAccountEntity>
    {
        /// <summary>
        /// Gets the accounts for the carrier.
        /// </summary>
        public override IEnumerable<OnTracAccountEntity> Accounts => OnTracAccountManager.Accounts;

        /// <summary>
        /// Returns a carrier account for the provided accountID.
        /// </summary>
        /// <param name="accountID">The account ID for which to return an account.</param>
        /// <returns>The matching account as IEntity2.</returns>
        public override OnTracAccountEntity GetAccount(long accountID)
        {
            return OnTracAccountManager.GetAccount(accountID);
        }

        /// <summary>
        /// Gets the default profile account.
        /// </summary>
        /// <value>
        /// The default profile account.
        /// </value>
        public override OnTracAccountEntity DefaultProfileAccount
        {
            get
            {
                long? accountID = GetPrimaryProfile(ShipmentTypeCode.OnTrac).OnTrac.OnTracAccountID;
                return GetProfileAccount(ShipmentTypeCode.OnTrac, accountID);
            }
        }

        /// <summary>
        /// Saves the specified account.
        /// </summary>
        /// <param name="account">The account.</param>
        public override void Save(OnTracAccountEntity account)
        {
            OnTracAccountManager.SaveAccount(account);
        }
    }
}
