using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Interapptive.Shared;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Licensing.Activation;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.CarrierSetup;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Warehouse.Configuration.DTO.ShippingSettings;

namespace ShipWorks.Shipping.Carriers.CarrierSetup
{
    /// <summary>
    /// Setup the DHL carrier configuration downloaded from the hub
    /// </summary>
    [KeyedComponent(typeof(ICarrierSetup), ShipmentTypeCode.DhlExpress)]
    public class DhlCarrierSetup : BaseCarrierSetup<DhlExpressAccountEntity, IDhlExpressAccountEntity>, ICarrierSetup
    {
        private readonly ICarrierAccountRepository<DhlExpressAccountEntity, IDhlExpressAccountEntity> accountRepository;
        private readonly IShipEngineWebClient shipEngineWebClient;
        private readonly IUspsWebClient uspsWebClient;
        private readonly ICarrierAccountDescription accountDescription;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreTooManyParams]
        public DhlCarrierSetup(IShipmentTypeSetupActivity shipmentTypeSetupActivity,
            IShippingSettings shippingSettings,
            IShipmentPrintHelper printHelper,
            ICarrierAccountRepository<DhlExpressAccountEntity, IDhlExpressAccountEntity> accountRepository,
            IIndex<ShipmentTypeCode, ICarrierAccountDescription> accountDescriptionFactory,
            IShipEngineWebClient shipEngineWebClient,
            IUspsWebClient uspsWebClient,
            Func<Type,ILog> createLog)
            : base(shipmentTypeSetupActivity, shippingSettings, printHelper, accountRepository)
        {
            this.accountRepository = accountRepository;
            this.shipEngineWebClient = shipEngineWebClient;
            this.uspsWebClient = uspsWebClient;
            accountDescription = accountDescriptionFactory[ShipmentTypeCode.DhlExpress];
            log = createLog(typeof(DhlCarrierSetup));
        }

        /// <summary>
        /// Setup a DHL account from data imported from the hub
        /// </summary>
        public async Task Setup(CarrierConfiguration config, UspsAccountEntity oneBalanceUspsAccount)
        {
            // skip if we already know about this account
            if (accountRepository.AccountsReadOnly.Any(x =>
                x.HubCarrierId == config.HubCarrierID && x.HubVersion >= config.HubVersion))
            {
                return;
            }

            // skip if there is already a one balance account and this config is for onebalance
            if (config.IsOneBalance &&
                accountRepository.AccountsReadOnly.Any(x => x.UspsAccountId.HasValue))
            {
                return;
            }

            var additionalAccountInfo = config.AdditionalData["dhl"].ToObject<DhlAccountConfiguration>();
            var dhlAccount = GetOrCreateAccountEntity(config.HubCarrierID);

            if (!config.IsOneBalance)
            {
                GetAddress(config.Address).CopyTo(dhlAccount, string.Empty);
            }

            dhlAccount.HubVersion = config.HubVersion;

            if (dhlAccount.IsNew)
            {
                if (config.IsOneBalance)
                {
                    dhlAccount.UspsAccountId = oneBalanceUspsAccount.UspsAccountID;
                    dhlAccount.AccountNumber = oneBalanceUspsAccount.UspsAccountID;
                    dhlAccount.FirstName = oneBalanceUspsAccount.FirstName;
                    dhlAccount.MiddleName = oneBalanceUspsAccount.MiddleName;
                    dhlAccount.LastName = oneBalanceUspsAccount.LastName;
                    dhlAccount.Company = oneBalanceUspsAccount.Company;
                    dhlAccount.Street1 = oneBalanceUspsAccount.Street1;
                    dhlAccount.City = oneBalanceUspsAccount.City;
                    dhlAccount.StateProvCode = Geography.GetStateProvCode(oneBalanceUspsAccount.StateProvCode);
                    dhlAccount.PostalCode = oneBalanceUspsAccount.PostalCode;
                    dhlAccount.CountryCode = Geography.GetCountryCode(oneBalanceUspsAccount.CountryCode);
                    dhlAccount.Email = oneBalanceUspsAccount.Email;
                    dhlAccount.Phone = oneBalanceUspsAccount.Phone;
                    dhlAccount.Description = accountDescription.GetDefaultAccountDescription(dhlAccount);
                }
                else
                {
                    GenericResult<string> connectAccountResult = await shipEngineWebClient.ConnectDhlAccount(additionalAccountInfo.AccountNumber);
                    if (connectAccountResult.Success)
                    {
                        dhlAccount.AccountNumber = long.Parse(additionalAccountInfo.AccountNumber);
                        dhlAccount.ShipEngineCarrierId = connectAccountResult.Value;
                        dhlAccount.Description = accountDescription.GetDefaultAccountDescription(dhlAccount);
                    }
                    else
                    {
                        throw connectAccountResult.Exception;
                    }
                }
            }

            accountRepository.Save(dhlAccount);
            SetupDefaultsIfNeeded(ShipmentTypeCode.DhlExpress, config.RequestedLabelFormat);
        }

        /// <summary>
        /// Get or Create a DHL account
        /// </summary>
        private DhlExpressAccountEntity GetOrCreateAccountEntity(Guid carrierId) =>
            accountRepository.Accounts.FirstOrDefault(x => x.HubCarrierId == carrierId)
            ?? new DhlExpressAccountEntity { HubCarrierId = carrierId };
    }
}
