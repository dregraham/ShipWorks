using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.BestRate;

namespace ShipWorks.Shipping.UI.Settings
{
    /// <summary>
    /// View model for the BestRateAccountSettingsControl
    /// </summary>
    [Component(RegistrationType.Self)]
    public class BestRateAccountSettingsViewModel
    {
        private readonly ICarrierAccountRetrieverFactory accountRetrieverFactory;
        private readonly IBestRateExcludedAccountRepository excludedAccountRepository;
        private readonly IShipmentTypeManager shipmentTypeManager;

        private static readonly List<ShipmentTypeCode> excludedShipmentTypes = new List<ShipmentTypeCode>
        {
            ShipmentTypeCode.None,
            ShipmentTypeCode.BestRate,
            ShipmentTypeCode.Other,
            ShipmentTypeCode.PostalWebTools,
            ShipmentTypeCode.Express1Endicia,
            ShipmentTypeCode.Express1Usps,
            ShipmentTypeCode.UpsWorldShip,
            ShipmentTypeCode.AmazonSFP,
            ShipmentTypeCode.AmazonSWA,
            ShipmentTypeCode.iParcel
        };

        /// <summary>
        /// Constructor
        /// </summary>
        public BestRateAccountSettingsViewModel(ICarrierAccountRetrieverFactory accountRetrieverFactory, IBestRateExcludedAccountRepository excludedAccountRepository, IShipmentTypeManager shipmentTypeManager)
        {
            this.accountRetrieverFactory = accountRetrieverFactory;
            this.excludedAccountRepository = excludedAccountRepository;
            this.shipmentTypeManager = shipmentTypeManager;

            Carriers = new ObservableCollection<BestRateCarrier>();
        }

        /// <summary>
        /// The carriers (with their accounts) allowed for best rate
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ObservableCollection<BestRateCarrier> Carriers { get; set; }

        /// <summary>
        /// Load the users shipping accounts for each carrier allowed for best rate
        /// </summary>
        public void Load()
        {
            // Get the shipment types allowed for best rate
            IEnumerable<ShipmentTypeCode> bestRateShipmentTypes = shipmentTypeManager.ShipmentTypeCodes.Except(excludedShipmentTypes);

            foreach (ShipmentTypeCode shipmentType in bestRateShipmentTypes)
            {
                Carriers.Add(new BestRateCarrier
                {
                    Name = EnumHelper.GetDescription(shipmentType),
                    Accounts = GetAccountsForCarrier(shipmentType)
                });
            }
        }

        /// <summary>
        /// Save the excluded accounts
        /// </summary>
        public void Save()
        {
            List<long> excludedAccounts = new List<long>();

            foreach (BestRateCarrier carrier in Carriers)
            {
                excludedAccounts.AddRange(carrier.Accounts.Where(a => !a.IsActive).Select(a => a.AccountID));
            }

            excludedAccountRepository.Save(excludedAccounts);
        }

        /// <summary>
        /// Get the accounts for the given carrier
        /// </summary>
        private ObservableCollection<BestRateAccount> GetAccountsForCarrier(ShipmentTypeCode shipmentType)
        {
            ObservableCollection<BestRateAccount> accounts = new ObservableCollection<BestRateAccount>();

            IEnumerable<ICarrierAccount> carrierAccounts = accountRetrieverFactory.Create(shipmentType).AccountsReadOnly;

            if (carrierAccounts.Any())
            {
                List<long> excludedAccountIDs = excludedAccountRepository.GetAll().ToList();

                // for each account, see if its in the list of excluded accounts
                foreach (ICarrierAccount carrierAccount in carrierAccounts)
                {
                    BestRateAccount account = new BestRateAccount
                    {
                        AccountID = carrierAccount.AccountId,
                        AccountDescription = carrierAccount.AccountDescription
                    };

                    // if the account was successfully removed from the list, it should be excluded
                    account.IsActive = !excludedAccountIDs.Remove(carrierAccount.AccountId);

                    accounts.Add(account);
                }
            }

            return accounts;
        }
    }
}
