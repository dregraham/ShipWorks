using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.UPS.Promo.API;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.UPS.Promo.RateFootnotes
{
    /// <summary>
    /// An IRateFootnoteFactory for creating USPS promotion footnotes.
    /// </summary>
    public class UpsPromoFootnoteFactory : IRateFootnoteFactory
    {
        private readonly UpsAccountEntity upsAccount;
        private readonly ICarrierSettingsRepository settingsRepository;
        private readonly ICarrierAccountRepository<UpsAccountEntity> accountRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsPromoFootnoteFactory(UpsAccountEntity upsAccount, ICarrierSettingsRepository settingsRepository, ICarrierAccountRepository<UpsAccountEntity> accountRepository)
        {
            this.settingsRepository = settingsRepository;
            this.accountRepository = accountRepository;
            this.upsAccount = upsAccount;
        }

        /// <summary>
        /// Creates a UpsPromoFootnote
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public RateFootnoteControl CreateFootnote(FootnoteParameters parameters)
        {
            return new UpsPromoFootnote(CreatePromo());
        }

        /// <summary>
        /// Creates the UpsPromo
        /// </summary>
        /// <returns></returns>
        private UpsPromo CreatePromo()
        {
            return new UpsPromo(upsAccount, settingsRepository, accountRepository, new UpsPromoWebClientFactory());
        }

        /// <summary>
        /// Not for Best Rate
        /// </summary>
        public bool AllowedForBestRate => false;
        
        /// <summary>
        /// Unused but required by Interface
        /// </summary>
        public ShipmentType ShipmentType { get; }
    }
}
