using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Express1;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Registration.Promotion;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Contracts;

namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// Factory that creates registration promotion classes based on which types of accounts a user has
    /// </summary>
    public class RegistrationPromotionFactory
    {
        private readonly ICarrierAccountRepository<UspsAccountEntity> uspsAccountRepository;
        private readonly bool stampsAccountsExist;
        private readonly bool endiciaAccountsExist;
        private readonly bool express1AccountsExist;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationPromotionFactory"/> class.
        /// </summary>
        public RegistrationPromotionFactory() : 
            this(new UspsAccountRepository(), new Express1StampsAccountRepository(), 
                new EndiciaAccountRepository(), new Express1EndiciaAccountRepository())
        {
        }

        /// <summary>
        /// Constructor that allows easier testing of the factory
        /// </summary>
        public RegistrationPromotionFactory(ICarrierAccountRepository<UspsAccountEntity> uspsAccountRepository,
            ICarrierAccountRepository<UspsAccountEntity> stampsExpress1AccountRepository,
            ICarrierAccountRepository<EndiciaAccountEntity> endiciaAccountRepository,
            ICarrierAccountRepository<EndiciaAccountEntity> endiciaExpress1AccountRepository)
        {
            this.uspsAccountRepository = uspsAccountRepository;

            stampsAccountsExist = uspsAccountRepository.Accounts.Any();
            endiciaAccountsExist = endiciaAccountRepository.Accounts.Any();
            express1AccountsExist = stampsExpress1AccountRepository.Accounts.Any() || endiciaExpress1AccountRepository.Accounts.Any();
        }

        /// <summary>
        /// Creates the registration promotion based on the Postal accounts in ShipWorks.
        /// </summary>
        /// <returns>An instance of IRegistrationPromotion.</returns>
        public IRegistrationPromotion CreateRegistrationPromotion()
        {
            if (!PostalAccountsExist())
            {
                return new NewPostalCustomerRegistrationPromotion();
            }

            if (OnlyExpress1AccountsExist())
            {
                return new Express1OnlyRegistrationPromotion();
            }

            if (endiciaAccountsExist)
            {
                return AnyUspsResellerAccountsExist() ? 
                    (IRegistrationPromotion) new EndiciaCbpRegistrationPromotion() : 
                    new EndiciaIntuishipRegistrationPromotion();
            }

            return AnyUspsResellerAccountsExist() ?
                (IRegistrationPromotion) new StampsCbpRegistrationPromotion() :
                new StampsIntuishipRegistrationPromotion();
        }

        /// <summary>
        /// Are there any postal accounts in the system?
        /// </summary>
        private bool PostalAccountsExist()
        {
            return (endiciaAccountsExist || stampsAccountsExist || express1AccountsExist);
        }

        /// <summary>
        /// Are there only Express1 accounts in the system?
        /// </summary>
        private bool OnlyExpress1AccountsExist()
        {
            return express1AccountsExist && !stampsAccountsExist && !endiciaAccountsExist;
        }

        /// <summary>
        /// Is any Usps or Stamps account a reseller account?
        /// </summary>
        /// <returns></returns>
        private bool AnyUspsResellerAccountsExist()
        {
            return uspsAccountRepository.Accounts.Any(x => x.ContractType == (int)UspsAccountContractType.Reseller);
        }
    }
}
