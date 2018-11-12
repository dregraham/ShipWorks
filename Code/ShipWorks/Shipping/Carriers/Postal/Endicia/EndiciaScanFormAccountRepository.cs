using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.ScanForms;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// An Endicia implementation of the IScanFormAccountRepository interface.
    /// </summary>
    public class EndiciaScanFormAccountRepository : IScanFormAccountRepository
    {
        /// <summary>
        /// Gets all of the accounts for a specific shipping carrier.
        /// </summary>
        /// <returns>A collection of the ScanFormCarrierAccount objects.</returns>
        public IEnumerable<IScanFormCarrierAccount> GetAccounts()
        {
            List<IScanFormCarrierAccount> carrierAccounts = new List<IScanFormCarrierAccount>();

            List<EndiciaAccountEntity> accountEntities = EndiciaAccountManager.EndiciaAccounts;
            foreach (EndiciaAccountEntity accountEntity in accountEntities)
            {
                EndiciaScanFormCarrierAccount carrierAccount = new EndiciaScanFormCarrierAccount(new EndiciaScanFormRepository(true), accountEntity);
                carrierAccounts.Add(carrierAccount);
            }

            return carrierAccounts;
        }

        /// <summary>
        /// Does the repository have any accounts
        /// </summary>
        public bool HasAccounts => EndiciaAccountManager.EndiciaAccountsReadOnly.Any();
    }
}
