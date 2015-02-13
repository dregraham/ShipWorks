using System.Linq;
using ShipWorks.Data.Model.Custom.EntityClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal.WebTools.BestRate;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.BestRate
{
    /// <summary>
    /// Counter rate broker that converts shipments to Stamps
    /// </summary>
    public class StampsCounterRatesBroker : WebToolsCounterRatesBroker
    {
        private readonly ICarrierAccountRepository<UspsAccountEntity> stampsAccountRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="accountRepository">Repository that will be used to access Stamps accounts</param>
        public StampsCounterRatesBroker(ICarrierAccountRepository<UspsAccountEntity> accountRepository) :
            base(new StampsShipmentType())
        {
            stampsAccountRepository = accountRepository;
        }

        /// <summary>
        /// Updates the shipment account with the actual account
        /// </summary>
        /// <param name="postalShipmentEntity"></param>
        /// <param name="account"></param>
        protected override void UpdateChildAccountId(PostalShipmentEntity postalShipmentEntity, NullEntity account)
        {
            UspsAccountEntity uspsAccount = stampsAccountRepository.Accounts.FirstOrDefault();

            if (postalShipmentEntity.Usps == null)
            {
                postalShipmentEntity.Usps = new UspsShipmentEntity();
            }

            if (uspsAccount != null)
            {
                postalShipmentEntity.Usps.UspsAccountID = uspsAccount.UspsAccountID;
            }

            base.UpdateChildAccountId(postalShipmentEntity, account);
        }

        /// <summary>
        /// Configures the specified broker settings.
        /// </summary>
        public override void Configure(IBestRateBrokerSettings brokerSettings)
        {
            base.Configure(brokerSettings);

            ((StampsShipmentType)ShipmentType).ShouldRetrieveExpress1Rates = false;
        }
    }
}
