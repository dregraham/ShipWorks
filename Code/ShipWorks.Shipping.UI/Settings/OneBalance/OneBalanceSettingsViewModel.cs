using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;

namespace ShipWorks.Shipping.UI.Settings.OneBalance
{
    /// <summary>
    /// View model for the OneBalanceSettingsControl
    /// </summary>
    public class OneBalanceSettingsControlViewModel
    {
        private readonly IPostageWebClient webClient;

        /// <summary>
        /// The current balance of the one balance account
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// The message to be displayed in place of the account balance if needed
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// A flag to indicate if we should show the message
        /// </summary>
        public bool ShowMessage { get; set; } = false;

        /// <summary>
        /// Initialize the control to display information for the given account
        /// </summary>
        public OneBalanceSettingsControlViewModel(IPostageWebClient webClient)
        {
            this.webClient = webClient;

            GetAccountBalance();
        }

        /// <summary>
        /// Retrieve the accounts balance if we have a One Balance account
        /// </summary>
        private void GetAccountBalance()
        {
            if (!string.IsNullOrEmpty(webClient.AccountIdentifier))
            {
                GetBalance();
            }
        }

        /// <summary>
        /// Retrieve the accounts balance
        /// </summary>
        private void GetBalance()
        {
            int tries = 5;

            while (tries-- > 0)
            {
                try
                {
                    Balance = new PostageBalance(webClient).Value;

                    break;
                }
                catch (UspsException ex)
                {
                    bool keepTrying = false;
                    string message = ex.Message;

                    // This message means we created a new account, but it wasn't ready to go yet
                    if (ex.Message.Contains("Registration timed out while authenticating."))
                    {
                        message = $"Your One Balance account is not ready yet.";

                        keepTrying = true;
                    }

                    Message = message;

                    ShowMessage = true;

                    if (keepTrying)
                    {
                        // Sleep for a few seconds to allow the registration time to go through
                        Thread.Sleep(3000);
                    }
                }
            }
        }
    }
}
