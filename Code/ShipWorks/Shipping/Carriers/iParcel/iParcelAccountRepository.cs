using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.iParcel
{
    public class iParcelAccountRepository : CarrierAccountRepositoryBase<IParcelAccountEntity>, ICarrierAccountRepository<IParcelAccountEntity>
    {
        /// <summary>
        /// Gets the accounts for the carrier.
        /// </summary>
        public override IEnumerable<IParcelAccountEntity> Accounts
        {
            get
            {
                return iParcelAccountManager.Accounts;
            }
        }

        /// <summary>
        /// Returns a carrier account for the provided accountID.
        /// </summary>
        /// <param name="accountID">The account ID for which to return an account.</param>
        /// <returns>The matching account as IEntity2.</returns>
        public override IParcelAccountEntity GetAccount(long accountID)
        {
            return iParcelAccountManager.GetAccount(accountID);
        }

        /// <summary>
        /// Gets the default profile account.
        /// </summary>
        /// <value>
        /// The default profile account.
        /// </value>
        /// <exception cref="System.NotImplementedException"></exception>
        public override IParcelAccountEntity DefaultProfileAccount
        {
            get
            {
                long? accountID = ShipmentTypeManager.GetType(ShipmentTypeCode.iParcel).GetPrimaryProfile().IParcel.IParcelAccountID;

                return GetProfileAccount(ShipmentTypeCode.iParcel, accountID);
            }
        }

        /// <summary>
        /// Saves the specified account.
        /// </summary>
        /// <param name="account">The account.</param>
        public override void Save(IParcelAccountEntity account)
        {
            iParcelAccountManager.SaveAccount(account);
        }
    }
}
