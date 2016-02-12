using System.Windows;
using GalaSoft.MvvmLight.Command;

namespace ShipWorks.UI.Controls.UpgradePlan
{
    public interface IUpgradePlanDlgViewModel
    {
        string Message { get; }

        RelayCommand<Window> UpgradePlanClickCommand { get; }

        void Load(string message);
    }
}