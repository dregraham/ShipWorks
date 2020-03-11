using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;

namespace ShipWorks.Shipping.UI.Settings.OneBalance
{
    /// <summary>
    /// View model for the OneBalanceAutoFundControl
    /// </summary>
    [Component]
    public class OneBalanceAutoFundControlViewModel : ViewModelBase, IOneBalanceAutoFundControlViewModel
    {
        private readonly IUspsAccountManager accountManager;
        private IUspsWebClient webClient;
        private AutoBuySettings autoBuySettings;

        private decimal balance;
        private decimal minimumBalance;
        private decimal autoFundAmount;
        private bool isAutoFund;
        private string autoFundError;
        private bool loading = true;

        /// <summary>
        /// Constuctor
        /// </summary>
        public OneBalanceAutoFundControlViewModel(IUspsAccountManager accountManager)
        {
            this.accountManager = accountManager;
            SetupWebClient();
            InitiateGetAutoFundSettings();
        }

        /// <summary>
        /// The current balance of the one balance account
        /// </summary>
        [Obfuscation(Exclude = true)]
        public decimal Balance
        {
            get => balance;
            set
            {
                Set(ref balance, value);
                Loading = false;
            }
        }

        /// <summary>
        /// The account balance that triggers the auto fund
        /// </summary>
        [Obfuscation(Exclude = true)]
        public decimal MinimumBalance
        {
            get => minimumBalance;
            set => Set(ref minimumBalance, value);
        }

        /// <summary>
        /// The amount to add to the account balance when auto funding
        /// </summary>
        [Obfuscation(Exclude = true)]
        public decimal AutoFundAmount
        {
            get => autoFundAmount;
            set => Set(ref autoFundAmount, value);
        }

        /// <summary>
        /// A value indicating if auto funding is turned on
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsAutoFund
        {
            get => isAutoFund;
            set
            {
                Set(ref isAutoFund, value);
                if (!value)
                {
                    MinimumBalance = 0;
                    AutoFundAmount = 0;
                }
            }
        }

        /// <summary>
        /// The message to be displayed if we couldn't get auto fund settings
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string AutoFundError
        {
            get => autoFundError;
            set => Set(ref autoFundError, value);
        }

        /// <summary>
        /// A flag to indicate if we are still trying to load the balance
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool Loading
        {
            get => loading;
            set => Set(ref loading, value);
        }

        /// <summary>
        /// Make an API call to stamps tp save the auto fund settings
        /// </summary>
        public void SaveSettings()
        {
            // Only set if something changed
            if (autoBuySettings == null ||
                autoBuySettings.AutoBuyEnabled != IsAutoFund ||
                autoBuySettings.PurchaseAmount != AutoFundAmount ||
                autoBuySettings.TriggerAmount != MinimumBalance)
            {
                var account = accountManager.UspsAccounts.FirstOrDefault(a => a.ShipEngineCarrierId != null);

                if (account == null)
                {
                    return;
                }

                var newAutoBuySettings = new AutoBuySettings()
                {
                    AutoBuyEnabled = IsAutoFund,
                    PurchaseAmount = AutoFundAmount,
                    TriggerAmount = MinimumBalance
                };

                webClient.SetAutoBuy(account, newAutoBuySettings);
            }
        }

        /// <summary>
        /// Sets up the web client
        /// </summary>
        private void SetupWebClient()
        {
            UspsShipmentType shipmentType = ShipmentTypeManager.GetType(ShipmentTypeCode.Usps) as UspsShipmentType;
            webClient = shipmentType.CreateWebClient();
        }

        /// <summary>
        /// Fire and forget the retrieval of auto fund settings
        /// </summary>
        private void InitiateGetAutoFundSettings()
        {
            Task.Run(() => GetAutoFundSettings());
        }

        /// <summary>
        /// Retrieve the accounts balance if we have a One Balance account
        /// </summary>
        private void GetAutoFundSettings()
        {
            var account = accountManager.UspsAccounts.FirstOrDefault(a => a.ShipEngineCarrierId != null);

            try
            {
                var accountInfo = (AccountInfoV41) webClient.GetAccountInfo(account);

                IsAutoFund = accountInfo.AutoBuySettings.AutoBuyEnabled;
                MinimumBalance = accountInfo.AutoBuySettings.TriggerAmount;
                AutoFundAmount = accountInfo.AutoBuySettings.PurchaseAmount;

                autoBuySettings = accountInfo.AutoBuySettings;
            }
            catch (Exception)
            {
                // We don't need to log the exception because the webclient takes care of that
                AutoFundError = "There was a problem retrieving your automatic funding settings.";
            }
        }
    }
}
