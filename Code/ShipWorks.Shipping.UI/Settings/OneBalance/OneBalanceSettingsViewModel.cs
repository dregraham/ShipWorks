using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;
using ShipWorks.Shipping.Carriers.UPS;

namespace ShipWorks.Shipping.UI.Settings.OneBalance
{
    /// <summary>
    /// View model for the OneBalanceSettingsControl
    /// </summary>
    [Component]
    public class OneBalanceSettingsControlViewModel : ViewModelBase, IOneBalanceSettingsControlViewModel
    {
        private IPostageWebClient postageWebClient;
        private IUspsWebClient webClient;
        private readonly IUspsAccountManager accountManager;
        private UpsAccountEntity upsAccount;
        private readonly Func<IPostageWebClient, IOneBalanceAddMoneyDialog> addMoneyDialogFactory;
        private AutoBuySettings autoBuySettings;

        private decimal balance;
        private decimal minimumBalance;
        private decimal autoFundAmount;
        private string getBalanceError;
        private string autoFundError;
        private bool showGetBalanceError = false;
        private bool showBanner;
        private bool loading = true;
        private bool addMoneyEnabled = true;
        private bool isAutoFund;

        /// <summary>
        /// Initialize the control to display information for the given account
        /// </summary>
        public OneBalanceSettingsControlViewModel(IUspsAccountManager accountManager,
            Func<IPostageWebClient, IOneBalanceAddMoneyDialog> addMoneyDialogFactory,
            IOneBalanceEnableUpsBannerWpfViewModel bannerViewModel)
        {
            this.accountManager = accountManager;
            SetupWebClients();
            upsAccount = GetUpsAccount();

            this.addMoneyDialogFactory = addMoneyDialogFactory;
            ShowBanner = upsAccount == null;

            BannerContext = bannerViewModel;

            BannerContext.SetupComplete += OnOneBalanceSetupComplete;

            GetInitialValuesCommand = new RelayCommand(GetInitialValues);
            ShowAddMoneyDialogCommand = new RelayCommand(ShowAddMoneyDialog);
        }

        /// <summary>
        /// The current balance of the one balance account
        /// </summary>
        [Obfuscation(Exclude = true)]
        public decimal Balance
        {
            get => balance;
            set => Set(ref balance, value);
        }

        /// <summary>
        /// The message to be displayed in place of the account balance if needed
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string GetBalanceError
        {
            get => getBalanceError;
            set => Set(ref getBalanceError, value);
        }

        /// <summary>
        /// A flag to indicate if we should show the message
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ShowGetBalanceError
        {
            get => showGetBalanceError;
            set => Set(ref showGetBalanceError, value);
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
        /// A flag to indicate if we are in a state that the user should be allowed to add money
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool AddMoneyEnabled
        {
            get => addMoneyEnabled;
            set => Set(ref addMoneyEnabled, value);
        }

        /// <summary>
        /// A flag to indicate if we are in a state where we should show the banner
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ShowBanner
        {
            get => showBanner;
            set => Set(ref showBanner, value);
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
        /// A flag to indcate if we should show the dhl setup banner
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ShowDhlBanner
        {
            get => webClient != null;
        }

        /// <summary>
        /// The data context for the enable ups banner
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IOneBalanceEnableUpsBannerWpfViewModel BannerContext { get; }

        /// <summary>
        /// RelayCommand for getting initial values to populate fields with
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand GetInitialValuesCommand { get; }

        /// <summary>
        /// Relay command for showing the add money dialog
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand ShowAddMoneyDialogCommand { get; }

        /// <summary>
        /// Retrieve the accounts balance if we have a One Balance account
        /// </summary>
        private void GetAccountBalance()
        {
            Loading = true;
            ShowBanner = upsAccount == null;

            if (webClient != null)
            {
                Task.Run(() =>
                {
                    GetBalance();
                    AddMoneyEnabled = webClient != null && !showGetBalanceError;
                    Loading = false;
                });
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
                    Balance = new ShipWorks.Shipping.Carriers.Postal.PostageBalance(postageWebClient).Value;

                    break;
                }
                catch (UspsException ex)
                {
                    bool keepTrying = false;

                    // This message means we created a new account, but it wasn't ready to go yet
                    if (ex.Message.Contains("Registration timed out while authenticating."))
                    {
                        GetBalanceError = $"Your One Balance account is not ready yet.";

                        keepTrying = true;
                    }
                    else
                    {
                        GetBalanceError = "There was an error retrieving your account balance";
                    }

                    ShowGetBalanceError = true;

                    if (keepTrying)
                    {
                        // Sleep for a few seconds to allow the registration time to go through
                        Thread.Sleep(3000);
                    }
                }
            }
        }

        /// <summary>
        /// Show the add money dialog
        /// </summary>
        private void ShowAddMoneyDialog()
        {
            var addMoneyDialog = addMoneyDialogFactory(postageWebClient);

            var dlgResult = addMoneyDialog.ShowDialog();
            if (dlgResult == true)
            {
                GetAccountBalance();
            }
        }

        /// <summary>
        /// Sets up the web client
        /// </summary>
        private void SetupWebClients()
        {
            UspsShipmentType shipmentType = ShipmentTypeManager.GetType(ShipmentTypeCode.Usps) as UspsShipmentType;
            webClient = shipmentType.CreateWebClient();

            var account = accountManager.UspsAccounts.FirstOrDefault(a => a.ShipEngineCarrierId != null);
            postageWebClient = account == null ? null : new UspsPostageWebClient(account);
        }

        /// <summary>
        /// Get the Ups account
        /// </summary>
        private UpsAccountEntity GetUpsAccount() => UpsAccountManager.Accounts.FirstOrDefault(e => e.ShipEngineCarrierId != null);

        /// <summary>
        /// Event handler for the banners SetupComplete event
        /// </summary>
        private void OnOneBalanceSetupComplete(object sender, EventArgs e)
        {
            SetupWebClients();
            upsAccount = GetUpsAccount();
            GetAccountBalance();
        }

        /// <summary>
        /// Get initial values for fields that need to be populated
        /// </summary>
        private void GetInitialValues()
        {
            GetAccountBalance();
            InitiateGetAutoFundSettings();
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
            catch (Exception ex)
            {
                // We don't need to log the exception because the webclient takes care of that
                AutoFundError = "There was a problem retrieving your automatic funding settings.";
            }
        }

        /// <summary>
        /// Send the auto fund settings to Stamps
        /// </summary>
        public void SaveAutoFundSettings()
        {
            // Only set if something changed
            if (autoBuySettings.AutoBuyEnabled != IsAutoFund ||
                autoBuySettings.PurchaseAmount != AutoFundAmount ||
                autoBuySettings.TriggerAmount != MinimumBalance)
            {
                var account = accountManager.UspsAccounts.FirstOrDefault(a => a.ShipEngineCarrierId != null);

                var newAutoBuySettings = new AutoBuySettings()
                {
                    AutoBuyEnabled = IsAutoFund,
                    PurchaseAmount = AutoFundAmount,
                    TriggerAmount = MinimumBalance
                };

                Task.Run(() =>
                {
                    try
                    {
                        webClient.SetAutoBuy(account, newAutoBuySettings);
                    }
                    catch (Exception ex)
                    {
                        // If there's an exception it's already been logged, so we don't need to do anything here
                        // because the user has already left the One Balance settings screen
                    }
                });
            }
        }
    }
}
