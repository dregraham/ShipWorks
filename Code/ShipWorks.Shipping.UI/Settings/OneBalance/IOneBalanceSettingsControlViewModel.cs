using System.Windows.Input;

namespace ShipWorks.Shipping.UI.Settings.OneBalance
{
    public interface IOneBalanceSettingsControlViewModel
    {
        bool AddMoneyEnabled { get; set; }
        decimal Balance { get; set; }
        IOneBalanceEnableUpsBannerWpfViewModel BannerContext { get; }
        ICommand GetBalanceCommand { get; }
        bool Loading { get; set; }
        string Message { get; set; }
        ICommand ShowAddMoneyDialogCommand { get; }
        bool ShowBanner { get; set; }
        bool ShowDhlBanner { get; }
        bool ShowMessage { get; set; }
    }
}