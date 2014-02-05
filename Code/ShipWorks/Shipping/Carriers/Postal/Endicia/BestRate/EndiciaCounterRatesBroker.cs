using System.Linq;
using ShipWorks.Data.Model.Custom.EntityClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.WebTools.BestRate;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.BestRate
{
    /// <summary>
    /// Counter rate broker that converts shipments to Endicia
    /// </summary>
    public class EndiciaCounterRatesBroker : WebToolsCounterRatesBroker
    {
        private readonly ICarrierAccountRepository<EndiciaAccountEntity> endiciaAccountRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="accountRepository">Repository that will be used to access Endicia accounts</param>
        public EndiciaCounterRatesBroker(ICarrierAccountRepository<EndiciaAccountEntity> accountRepository) :
            base(new EndiciaShipmentType())
        {
            endiciaAccountRepository = accountRepository;
        }

        /// <summary>
        /// Updates the shipment account with the actual account
        /// </summary>
        /// <param name="postalShipmentEntity"></param>
        /// <param name="account"></param>
        protected override void UpdateChildAccountId(PostalShipmentEntity postalShipmentEntity, NullEntity account)
        {
            EndiciaAccountEntity endiciaAccount = endiciaAccountRepository.Accounts.FirstOrDefault();

            if (postalShipmentEntity.Endicia != null && endiciaAccount != null)
            {
                postalShipmentEntity.Endicia.EndiciaAccountID = endiciaAccount.EndiciaAccountID;
            }

            base.UpdateChildAccountId(postalShipmentEntity, account);
        }
    }
}
