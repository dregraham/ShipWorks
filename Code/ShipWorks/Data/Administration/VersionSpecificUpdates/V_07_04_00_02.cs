using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.iParcel;
using ShipWorks.Shipping.Carriers.OnTrac;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Data.Administration.VersionSpecificUpdates
{
    /// <summary>
    /// Update excluded best rate account table
    /// </summary>
    public class V_07_04_00_02 : IVersionSpecificUpdate
    {
        private readonly IIndex<ShipmentTypeCode, ICarrierAccountRetriever> carrierAccountRetrieverFactory;
        private readonly IShipmentTypeManager shipmentTypeManager;
        private readonly IBestRateExcludedAccountRepository excludedAccountRepository;
        private readonly IShippingSettings shippingSettings;

        public V_07_04_00_02(IIndex<ShipmentTypeCode, ICarrierAccountRetriever> carrierAccountRetrieverFactory,
            IShipmentTypeManager shipmentTypeManager,
            IBestRateExcludedAccountRepository excludedAccountRepository,
            IShippingSettings shippingSettings)
        {
            this.carrierAccountRetrieverFactory = carrierAccountRetrieverFactory;
            this.shipmentTypeManager = shipmentTypeManager;
            this.excludedAccountRepository = excludedAccountRepository;
            this.shippingSettings = shippingSettings;
        }

        /// <summary>
        /// 7.4.0.0
        /// </summary>
        public Version AppliesTo => new Version(7, 4, 0, 1);

        /// <summary>
        /// Run Once
        /// </summary>
        public bool AlwaysRun => false;

        /// <summary>
        /// Add all non-default accounts to excluded. Also, don't add an account if it is the only account for that carrier
        /// </summary>
        public void Update()
        {
            UspsAccountManager.InitializeForCurrentSession();
            EndiciaAccountManager.InitializeForCurrentSession();
            FedExAccountManager.InitializeForCurrentSession();
            UpsAccountManager.InitializeForCurrentSession();
            OnTracAccountManager.InitializeForCurrentSession();
            iParcelAccountManager.InitializeForCurrentSession();

            List<long> accountsToExclude = new List<long>();

            var shippingSettingsEntity = shippingSettings.FetchReadOnly();

            foreach (ShipmentTypeCode shipmentTypeCode in shipmentTypeManager.ConfiguredShipmentTypeCodes.Except(shippingSettingsEntity.BestRateExcludedTypes.Union(new[] { ShipmentTypeCode.BestRate })))
            {
                var accountRepository = carrierAccountRetrieverFactory[shipmentTypeCode];
                if (accountRepository.AccountsReadOnly.Count() > 1)
                {
                    long defaultAccountId = accountRepository.DefaultProfileAccount.AccountId;

                    var carrierAccountsToExclude = accountRepository.AccountsReadOnly
                        .Select(a => a.AccountId)
                        .Where(accountId => accountId != defaultAccountId);

                    accountsToExclude.AddRange(carrierAccountsToExclude);
                }
            }

            if (accountsToExclude.Any())
            {
                excludedAccountRepository.InitializeForCurrentSession();
                excludedAccountRepository.Save(accountsToExclude);
            }
        }
    }
}
