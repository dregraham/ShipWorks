using System;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Stamps;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Exception thrown when there are insufficient funds for processing
    /// </summary>
    public class UspsInsufficientFundsException : UspsException, IInsufficientFunds
    {
        private readonly UspsAccountEntity account;

        /// <summary>
        /// Constructor
        /// </summary>
        public UspsInsufficientFundsException(UspsAccountEntity account, string message) :
            base(message)
        {
            this.account = account;
        }

        /// <summary>
        /// Name of the provider associated with the exception
        /// </summary>
        public string Provider
        {
            get
            {
                switch ((UspsResellerType)account.UspsReseller)
                {
                    case UspsResellerType.None:
                        return "Stamps.com";
                    case UspsResellerType.Express1:
                        return "Express1";
                    case UspsResellerType.StampsExpedited:
                        return "USPS";
                    default:
                        throw new ArgumentOutOfRangeException("StampsReseller");
                }
            }
        }

        /// <summary>
        /// Identifier of the account
        /// </summary>
        public string AccountIdentifier
        {
            get
            {
                return account.Username;
            }
        }

        /// <summary>
        /// Create a dialog that will allow a customer to purchase more postage
        /// </summary>
        public Form CreatePostageDialog()
        {
            return new UspsPurchasePostageDlg(account);
        }
    }
}
