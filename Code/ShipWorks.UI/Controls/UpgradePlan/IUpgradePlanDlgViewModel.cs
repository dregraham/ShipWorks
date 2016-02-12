using System.Windows;
using GalaSoft.MvvmLight.Command;
using ShipWorks.ApplicationCore.Licensing;

namespace ShipWorks.UI.Controls.UpgradePlan
{
    public interface IUpgradePlanDlgViewModel
    {
        string Message { get; }

        RelayCommand<Window> UpgradePlanClickCommand { get; }

        void Load(string message, ICustomerLicense customerLicense);
    }
}