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
    public class OneBalanceSettingsControlViewModel
    {
        public decimal Balance { get; set; }
        public string Message { get; set; }

        public bool ShowBalance { get; set; } = true;
        public bool ShowMessage { get; set; } = false;

        private readonly IUspsAccountManager accountManager;
        private readonly IPostageWebClient webClient;

        /// <summary>
        /// Initialize the control to display information for the given account
        /// </summary>
        public OneBalanceSettingsControlViewModel(IUspsAccountManager accountManager, IPostageWebClient webClient)
        {
            this.accountManager = accountManager;
            this.webClient = webClient;

            GetAccountBalance();
        }

        private bool HasAccount() => accountManager.GetAccounts(UspsResellerType.None)
            .Where(a => a.ShortAccountDescription == webClient.AccountIdentifier)
            .FirstOrDefault() != null;
        

        private void GetAccountBalance()
        {
            if (HasAccount())
            {
                GetBalance();
            }

        }

        /// <summary>
        /// Update the postage balance of the account
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
                    ShowBalance = false;

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
