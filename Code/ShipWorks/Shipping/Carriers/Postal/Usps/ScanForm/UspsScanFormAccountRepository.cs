using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.ScanForms;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.ScanForm
{
    /// <summary>
    /// A USPS (Stamps.com Expedited) implementation of the IScanFormAccountRepository interface.
    /// </summary>
    public class UspsScanFormAccountRepository : IScanFormAccountRepository
    {
        /// <summary>
        /// Gets all of the accounts for a specific shipping carrier.
        /// </summary>
        /// <returns>A collection of the ScanFormCarrierAccount objects.</returns>
        public IEnumerable<IScanFormCarrierAccount> GetAccounts()
        {
            return AccountList.Select(CreateScanFormCarrierAccount).ToList();
        }

        /// <summary>
        /// Gets the accounts that should have scan forms created for them
        /// </summary>
        protected virtual IEnumerable<UspsAccountEntity> AccountList
        {
            get { return UspsAccountManager.UspsAccounts; }
        }

        /// <summary>
        /// Creates a ScanFormCarrierAccount from the account entity
        /// </summary>
        /// <param name="accountEntity">Account entity for which to create the scan form carrier account</param>
        /// <returns>A new instance of IScanFormCarrierAccount</returns>
        protected virtual IScanFormCarrierAccount CreateScanFormCarrierAccount(UspsAccountEntity accountEntity)
        {
            return new UspsScanFormCarrierAccount(new UspsScanFormRepository((UspsResellerType)accountEntity.UspsReseller), accountEntity);
        }
    }
}
