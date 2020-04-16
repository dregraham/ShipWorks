using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// Exception thrown when there are insufficient funds for processing in a label server account
    /// </summary>
    public class EndiciaInsufficientFundsException : EndiciaApiException, IInsufficientFunds
    {
        readonly EndiciaAccountEntity account;

        /// <summary>
        /// Constructor
        /// </summary>
        public EndiciaInsufficientFundsException(EndiciaAccountEntity account, int code, string message)
            : base(code, message)
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
                return account.EndiciaReseller == (int)EndiciaReseller.None ? "Endicia" : "Express1";
            }
        }

        /// <summary>
        /// Identifier of the account
        /// </summary>
        public string AccountIdentifier
        {
            get
            {
                return account.AccountNumber;
            }
        }

        /// <summary>
        /// Create a dialog that will allow a customer to purchase more postage
        /// </summary>
        public IForm CreatePostageDialog(ILifetimeScope lifetimeScope)
        {
            EndiciaBuyPostageDlg dialog = lifetimeScope.Resolve<EndiciaBuyPostageDlg>();
            dialog.LoadAccount(account);
            return dialog;
        }
    }
}
