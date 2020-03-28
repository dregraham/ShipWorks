using System.Windows.Input;

namespace ShipWorks.Shipping.UI.Settings.OneBalance
{
    /// <summary>
    /// Interface for the OneBalanceSettingsControlViewModel
    /// </summary>
    public interface IOneBalanceSettingsControlViewModel
    {
        /// <summary>
        /// A flag to indicate if we are in a state that the user should be allowed to add money
        /// </summary>
        bool AddMoneyEnabled { get; set; }

        /// <summary>
        /// The current balance of the one balance account
        /// </summary>
        decimal Balance { get; set; }

        /// <summary>
        /// The data context for the enable ups banner
        /// </summary>
        IOneBalanceShowSetupWizardViewModel BannerContext { get; }

        /// <summary>
        /// RelayCommand for getting initial values to populate fields with
        /// </summary>
        ICommand GetInitialValuesCommand { get; }

        /// <summary>
        /// A flag to indicate if we are still trying to load the balance
        /// </summary>
        bool Loading { get; set; }

        /// <summary>
        /// The message to be displayed in place of the account balance if needed
        /// </summary>
        string GetBalanceError { get; set; }

        /// <summary>
        /// Relay command for showing the add money dialog
        /// </summary>
        ICommand ShowAddMoneyDialogCommand { get; }

        /// <summary>
        /// A flag to indicate if we are in a state where we should show the banner
        /// </summary>
        bool ShowBanner { get; set; }

        /// <summary>
        /// A flag to indicate if we should show the message
        /// </summary>
        bool ShowGetBalanceError { get; set; }

        /// <summary>
        /// Send the auto fund settings to Stamps
        /// </summary>
        void SaveAutoFundSettings();
    }
}