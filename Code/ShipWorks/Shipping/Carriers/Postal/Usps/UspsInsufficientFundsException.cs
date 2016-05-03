using System;
using System.Windows.Forms;
using Autofac;
using ShipWorks.Data.Model.EntityClasses;

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
                        return "USPS";
                    case UspsResellerType.Express1:
                        return "Express1";
                    default:
                        throw new ArgumentOutOfRangeException("UspsReseller");
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
        public Form CreatePostageDialog(ILifetimeScope lifetimeScope)
        {
            return new UspsPurchasePostageDlg(account);
        }
    }
}
