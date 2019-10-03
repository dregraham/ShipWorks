using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Data.Administration.VersionSpecificUpdates
{
    /// <summary>
    /// Update excluded best rate account table
    /// </summary>
    public class V_07_04_00_00 : IVersionSpecificUpdate
    {
        private readonly ICarrierAccountRepositoryFactory accountRepositoryFactory;
        private readonly IShipmentTypeManager shipmentTypeManager;
        private readonly IBestRateExcludedAccountRepository excludedAccountRepository;
        
        public V_07_04_00_00(ICarrierAccountRepositoryFactory accountRepositoryFactory,
            IShipmentTypeManager shipmentTypeManager,
                IBestRateExcludedAccountRepository excludedAccountRepository)
        {
            this.accountRepositoryFactory = accountRepositoryFactory;
            this.shipmentTypeManager = shipmentTypeManager;
            this.excludedAccountRepository = excludedAccountRepository;
        }

        /// <summary>
        /// 7.4.0.0
        /// </summary>
        public Version AppliesTo => new Version(7, 4, 0, 0);

        /// <summary>
        /// Run Once
        /// </summary>
        public bool AlwaysRun => false;

        /// <summary>
        /// Add all non-default accounts to excluded. Also, don't add an account if it is the only account for that carrier
        /// </summary>
        public void Update()
        {
            List<long> accountsToExclude = new List<long>();

            foreach (ShipmentTypeCode shipmentTypeCode in shipmentTypeManager.ConfiguredShipmentTypeCodes)
            {
                var accountRepository = accountRepositoryFactory.Get(shipmentTypeCode);
                if (accountRepository.AccountsReadOnly.Count() > 1)
                {
                    long defaultAccountId = accountRepository.DefaultProfileAccount.AccountId;

                    var carrierAccountsToExclude = accountRepository.AccountsReadOnly
                        .Select(a => a.AccountId)
                        .Where(accountId => accountId != defaultAccountId);

                    accountsToExclude.AddRange(carrierAccountsToExclude);
                }
            }

            excludedAccountRepository.Save(accountsToExclude);
        }
    }
}
