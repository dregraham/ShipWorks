using System;
using System.Reflection;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityInterfaces;
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
        private readonly IUspsAccountEntity account;
        private IUspsWebClient webClient;
        private AutoBuySettings autoBuySettings;

        private decimal minimumBalance;
        private decimal autoFundAmount;
        private bool isAutoFund;
        private string autoFundError;

        /// <summary>
        /// Constructor
        /// </summary>
        public OneBalanceAutoFundControlViewModel(IUspsAccountEntity account)
        {
            this.account = account;
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
        /// Make an API call to stamps to save the auto fund settings
        /// </summary>
        public void SaveSettings()
        {
            if (account == null)
            {
                return;
            }

            // Only set if something changed
            // If we couldn't get the account settings initially don't try to save because
            // chances are we won't be able to and the user could get stuck on this page.
            if (autoBuySettings != null &&
                (autoBuySettings.AutoBuyEnabled != IsAutoFund ||
                autoBuySettings.PurchaseAmount != AutoFundAmount ||
                autoBuySettings.TriggerAmount != MinimumBalance))
            {
                if (IsAutoFund && AutoFundAmount > 500)
                {
                    throw new UspsException("The amount to automatically fund cannot be greater than $500");
                }

                if (IsAutoFund && MinimumBalance < 0.01M)
                {
                    throw new UspsException("Minimum balance amount must be greater than $0");
                }

                if (IsAutoFund && AutoFundAmount < 10)
                {
                    throw new UspsException("Amount to be funded must be at least $10");
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
            AutoFundError = null;

            if (account == null)
            {
                return;
            }

            try
            {
                var accountInfo = (AccountInfoV65) webClient.GetAccountInfo(account);

                IsAutoFund = accountInfo.AutoBuySettings.AutoBuyEnabled;
                MinimumBalance = accountInfo.AutoBuySettings.TriggerAmount;
                AutoFundAmount = accountInfo.AutoBuySettings.PurchaseAmount;

                autoBuySettings = accountInfo.AutoBuySettings;
            }
            catch (Exception)
            {
                // We don't need to log the exception because the web client takes care of that
                AutoFundError = "There was a problem retrieving your automatic funding settings. Your changes will not be saved.";
            }
        }
    }
}
