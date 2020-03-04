using System.Windows.Input;

namespace ShipWorks.Shipping.UI.Settings.OneBalance
{
    /// <summary>
    /// Interface for the OneBalanceEnableUpsBannerWpfViewModel
    /// </summary>
    public interface IOneBalanceEnableUpsBannerWpfViewModel
    {
        /// <summary>
        /// RelayCommand for Showing the setup dialog
        /// </summary>
        ICommand ShowSetupDialogCommand { get; }
    }
}