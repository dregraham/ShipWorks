using System.Collections.Generic;
using ShipWorks.Shipping.ScanForms;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Express1
{
    public class Express1ScanFormAccountRepository : IScanFormAccountRepository
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
                Express1ScanFormCarrierAccount carrierAccount = new Express1ScanFormCarrierAccount(new EndiciaScanFormRepository(false), accountEntity);
                carrierAccounts.Add(carrierAccount);
            }

            return carrierAccounts;
        }
    }
}
