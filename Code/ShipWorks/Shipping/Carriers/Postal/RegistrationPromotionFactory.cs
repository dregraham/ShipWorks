using System.Linq;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Registration.Promotion;

namespace ShipWorks.Shipping.Carriers.Postal
{
    public class RegistrationPromotionFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationPromotionFactory"/> class.
        /// </summary>
        public RegistrationPromotionFactory()
        {
            StampsAccountsExist = StampsAccountManager.StampsAccounts.Any() || StampsAccountManager.StampsExpeditedAccounts.Any();
            EndiciaAccountsExist = EndiciaAccountManager.EndiciaAccounts.Any();
            Express1AccountsExist = StampsAccountManager.Express1Accounts.Any() || EndiciaAccountManager.Express1Accounts.Any();
        }

        /// <summary>
        /// Gets or sets a value indicating whether [stamps accounts exist].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [stamps accounts exist]; otherwise, <c>false</c>.
        /// </value>
        private bool StampsAccountsExist { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [endicia accounts exist].
        /// </summary>
        /// <value>
        /// <c>true</c> if [endicia accounts exist]; otherwise, <c>false</c>.
        /// </value>
        private bool EndiciaAccountsExist { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [Express1 accounts exist].
        /// </summary>
        /// <value>
        /// <c>true</c> if [express1 accounts exist]; otherwise, <c>false</c>.
        /// </value>
        private bool Express1AccountsExist { get; set; }


        /// <summary>
        /// Creates the registration promotion based on the Postal accounts in ShipWorks.
        /// </summary>
        /// <returns>An instance of IRegistrationPromotion.</returns>
        public IRegistrationPromotion CreateRegistrationPromotion()
        {
            IRegistrationPromotion promotion = null;
            
            if (PostalAccountsExist())
            {
                if (Express1AccountsExist && !StampsAccountsExist && !EndiciaAccountsExist)
                {
                    // Only an Express1 account exists
                    promotion = new Express1OnlyRegistrationPromotion();
                }
                else if (EndiciaAccountsExist)
                {
                    // Case for an Endicia account exists
                    promotion = new EndiciaRegistrationPromotion();
                }
                else
                {
                    // Case for an existing customer with a postal account only through Stamps.com
                    promotion = new StampsIntuishipRegistrationPromotion();
                }
            }
            else
            {
                // There aren't any postal accounts in ShipWorks; this is considered to be a brand new customer, 
                // so use a promo code to indicate such to allow Stamps.com to track this information
                promotion = new NewPostalCustomerRegistrationPromotion();
            }
            
            return promotion;
        }

        /// <summary>
        /// Postals the accounts exist.
        /// </summary>
        /// <returns></returns>
        private bool PostalAccountsExist()
        {
            return (EndiciaAccountsExist || StampsAccountsExist || Express1AccountsExist);
        }
    }
}
