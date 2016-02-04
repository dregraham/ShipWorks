using GalaSoft.MvvmLight.Command;

namespace ShipWorks.UI.Controls.UpgradePlan
{
    public interface IUpgradePlanDlgViewModel
    {
        string Message { get; }
        RelayCommand UpgradePlanClickCommand { get; }
    }
}