using System.Linq;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.Custom.EntityClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal.WebTools.BestRate;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.BestRate
{
    /// <summary>
    /// Counter rate broker that converts shipments to USPS
    /// </summary>
    public class UspsCounterRatesBroker : WebToolsCounterRatesBroker
    {
        private readonly ICarrierAccountRepository<UspsAccountEntity> uspsAccountRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="accountRepository">Repository that will be used to access USPS accounts</param>
        public UspsCounterRatesBroker(ICarrierAccountRepository<UspsAccountEntity> accountRepository) :
            base(new UspsShipmentType())
        {
            uspsAccountRepository = accountRepository;
        }

        /// <summary>
        /// Updates the shipment account with the actual account
        /// </summary>
        /// <param name="postalShipmentEntity"></param>
        /// <param name="account"></param>
        protected override void UpdateChildAccountId(PostalShipmentEntity postalShipmentEntity, NullCarrierAccount account)
        {
            UspsAccountEntity uspsAccount = uspsAccountRepository.Accounts.FirstOrDefault();

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
    }
}
