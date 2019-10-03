using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        public ObservableCollection<BestRateCarrier> Carriers { get; set; }

        /// <summary>
        /// Load the users shipping accounts for each carrier allowed for best rate
        /// </summary>
        public void Load()
        {
            IEnumerable<ShipmentTypeCode> bestRateShipmentTypes = shipmentTypeManager.ShipmentTypeCodes.Except(excludedShipmentTypes);

            foreach (ShipmentTypeCode shipmentType in bestRateShipmentTypes)
            {
                ICarrierAccountRetriever carrierAccountRetriever = accountRetrieverFactory.Create(shipmentType);

                IEnumerable<ICarrierAccount> carrierAccounts = carrierAccountRetriever.AccountsReadOnly;

                if (carrierAccounts.Any())
                {
                    List<long> excludedAccountIDs = excludedAccountRepository.GetAll().ToList();

                    BestRateCarrier carrier = new BestRateCarrier()
                    {
                        Name = EnumHelper.GetDescription(shipmentType),
                        ShipmentType = shipmentType
                    };

                    foreach (ICarrierAccount carrierAccount in carrierAccounts)
                    {
                        BestRateAccount account = new BestRateAccount()
                        {
                            AccountID = carrierAccount.AccountId,
                            AccountDescription = carrierAccount.AccountDescription
                        };

                        if (excludedAccountIDs.Contains(carrierAccount.AccountId))
                        {
                            excludedAccountIDs.Remove(carrierAccount.AccountId);
                            account.IsActive = false;
                        }
                        else
                        {
                            account.IsActive = true;
                        }

                        carrier.Accounts.Add(account);
                    }

                    Carriers.Add(carrier);
                }
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
    }
}
