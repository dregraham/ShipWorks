using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps
{
    /// <summary>
    /// Exception thrown when there are insufficient funds for processing
    /// </summary>
    public class StampsInsufficientFundsException : StampsException, IInsufficientFundsException
    {
        private readonly StampsAccountEntity account;

        /// <summary>
        /// Constructor
        /// </summary>
        public StampsInsufficientFundsException(StampsAccountEntity account, string message) :
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
                return account.IsExpress1 ? "Express1" : "Stamps.com";
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
            return new StampsPurchasePostageDlg(account, null);
        }
    }
}
