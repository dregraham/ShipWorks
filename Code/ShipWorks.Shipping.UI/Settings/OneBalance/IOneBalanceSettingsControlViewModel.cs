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
        IOneBalanceEnableUpsBannerWpfViewModel BannerContext { get; }

        /// <summary>
        /// RelayCommand for getting the account balance
        /// </summary>
        ICommand GetBalanceCommand { get; }

        /// <summary>
        /// A flag to indicate if we are still trying to load the balance
        /// </summary>
        bool Loading { get; set; }

        /// <summary>
        /// The message to be displayed in place of the account balance if needed
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// Relay command for showing the add money dialog
        /// </summary>
        ICommand ShowAddMoneyDialogCommand { get; }

        /// <summary>
        /// A flag to indicate if we are in a state where we should show the banner
        /// </summary>
        bool ShowBanner { get; set; }

        /// <summary>
        /// A flag to indcate if we should show the dhl setup banner
        /// </summary>
        bool ShowDhlBanner { get; }

        /// <summary>
        /// A flag to indicate if we should show the message
        /// </summary>
        bool ShowMessage { get; set; }
    }
}