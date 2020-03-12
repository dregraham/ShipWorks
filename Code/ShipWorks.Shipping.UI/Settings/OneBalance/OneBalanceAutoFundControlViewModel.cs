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

        private decimal minimumBalance;
        private decimal autoFundAmount;
        private bool isAutoFund;
        private string autoFundError;

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
            set => Set(ref isAutoFund, value);
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
