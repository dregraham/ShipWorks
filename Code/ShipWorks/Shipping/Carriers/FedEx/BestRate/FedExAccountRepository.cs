using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;

namespace ShipWorks.Shipping.Carriers.FedEx.BestRate
{
    public class FedExAccountRepository : ICarrierAccountRepository<FedExAccountEntity>
    {
        /// <summary>
        /// Gets the accounts for the carrier.
        /// </summary>
        public IEnumerable<FedExAccountEntity> Accounts
        {
            get
            {
                return FedExAccountManager.Accounts;
            }
        }

        /// <summary>
        /// Returns a carrier account for the provided accountID.
        /// </summary>
        /// <param name="accountID">The account ID for which to return an account.</param>
        /// <returns>The matching account as IEntity2.</returns>
        public FedExAccountEntity GetAccount(long accountID)
        {
            return FedExAccountManager.GetAccount(accountID);
        }

        /// <summary>
        /// Gets the account associated withe the default profile. A null value is returned
        /// if there is not an account associated with the default profile.
        /// </summary>
        /// <exception cref="ShippingException">An account that no longer exists is associated with the default FedEx profile.</exception>
        public FedExAccountEntity DefaultProfileAccount 
        {
            get
            {
                FedExAccountEntity account = null;
                long? accountID = new FedExShipmentType().GetPrimaryProfile().FedEx.FedExAccountID;
                
                if (accountID.HasValue)
                {
                    account = GetAccount(accountID.Value);
                    if (account == null)
                    {
                        throw new ShippingException("An account that no longer exists is associated with the default FedEx profile.");
                    }
                }

                return account;
            }
        }
    }
}