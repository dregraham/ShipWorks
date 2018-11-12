using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.ScanForms;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Express1
{
    public class Express1EndiciaScanFormAccountRepository : IScanFormAccountRepository
    {
        /// <summary>
        /// Gets all of the accounts for a specific shipping carrier.
        /// </summary>
        /// <returns>A collection of the ScanFormCarrierAccount objects.</returns>
        public IEnumerable<IScanFormCarrierAccount> GetAccounts()
        {
            List<IScanFormCarrierAccount> carrierAccounts = new List<IScanFormCarrierAccount>();

            List<EndiciaAccountEntity> accountEntities = EndiciaAccountManager.Express1Accounts;
            foreach (EndiciaAccountEntity accountEntity in accountEntities)
            {
                // The Express 1 carrier account uses the same repository as Endicia
                Express1EndiciaScanFormCarrierAccount carrierAccount = new Express1EndiciaScanFormCarrierAccount(new EndiciaScanFormRepository(false), accountEntity);
                carrierAccounts.Add(carrierAccount);
            }

            return carrierAccounts;
        }

        /// <summary>
        /// Does the repository have any accounts
        /// </summary>
        public bool HasAccounts => EndiciaAccountManager.Express1AccountsReadOnly.Any();
    }
}
