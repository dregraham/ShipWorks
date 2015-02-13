using System;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps
{
    /// <summary>
    /// Exception thrown when there are insufficient funds for processing
    /// </summary>
    public class StampsInsufficientFundsException : StampsException, IInsufficientFunds
    {
        private readonly UspsAccountEntity account;

        /// <summary>
        /// Constructor
        /// </summary>
        public StampsInsufficientFundsException(UspsAccountEntity account, string message) :
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
                switch ((StampsResellerType)account.UspsReseller)
                {
                    case StampsResellerType.None:
                        return "Stamps.com";
                    case StampsResellerType.Express1:
                        return "Express1";
                    case StampsResellerType.StampsExpedited:
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
            return new StampsPurchasePostageDlg(account);
        }
    }
}
