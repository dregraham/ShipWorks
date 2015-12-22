using System.Collections.Generic;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// An abstract base class that implements the ICarrierAccountRepository interface with common method 
    /// implementations that could be used by other carrier account repositories.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CarrierAccountRepositoryBase<T> : ICarrierAccountRepository<T> where T : IEntity2
    {
        /// <summary>
        ///  Returns a list of accounts for the carrier.
        ///  </summary>
        public abstract IEnumerable<T> Accounts { get; }

        /// <summary>
        ///  Returns a carrier account for the provided accountID.
        ///  </summary><param name="accountID">The account ID for which to return an account.</param><returns>The matching account as IEntity2.</returns>
        public abstract T GetAccount(long accountID);

        /// <summary>
        ///  Returns the default account as defined by the primary profile
        ///  </summary>
        public abstract T DefaultProfileAccount { get; }

        /// <summary>
        /// Saves the specified account.
        /// </summary>
        /// <param name="account">The account.</param>
        public abstract void Save(T account);

        /// <summary>
        /// A helper method to gets the account associated withe the default profile. A null value is returned
        /// if there is not an account associated with the default profile.
        /// </summary>
        /// <param name="shipmentTypeCode">The shipment type code.</param>
        /// <param name="accountID">The account identifier.</param>
        /// <returns></returns>
        /// <exception cref="ShippingException">An account that no longer exists is associated with the default FedEx profile.</exception>
        protected T GetProfileAccount(ShipmentTypeCode shipmentTypeCode, long? accountID)
        {
            // Start with a null account
            T account = default(T);

            if (accountID.HasValue)
            {
                account = GetAccount(accountID.Value);
                if (object.Equals(account, default(T)))
                {
                    throw new ShippingException(string.Format("An account that no longer exists is associated with the default {0} profile.", EnumHelper.GetDescription(shipmentTypeCode)));
                }
            }

            return account;
        }
    }
}
