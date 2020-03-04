using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;
using Autofac.Features.Indexed;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.UI.Settings.OneBalance
{
    /// <summary>
    /// View model for the OneBalanceEnableUpsBannerWpfControl
    /// </summary>
    [Component]
    public class OneBalanceEnableUpsBannerWpfViewModel : ViewModelBase, IOneBalanceEnableUpsBannerWpfViewModel
    {
        private readonly IIndex<ShipmentTypeCode, IShipmentTypeSetupWizard> setupWizardFactory;
        private readonly IWin32Window window;

        /// <summary>
        /// Constructor
        /// </summary>
        public OneBalanceEnableUpsBannerWpfViewModel(IIndex<ShipmentTypeCode, IShipmentTypeSetupWizard> setupWizardFactory, IWin32Window window)
        {
            this.setupWizardFactory = setupWizardFactory;
            this.window = window;
            ShowSetupDialogCommand = new RelayCommand(ShowSetupDialog);
        }

        /// <summary>
        /// RelayCommand for Showing the setup dialog
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand ShowSetupDialogCommand { get; }

        /// <summary>
        /// Shows the setup dialog to the user
        /// </summary>
        private void ShowSetupDialog()
        {
            var upsSetupWizard = setupWizardFactory[ShipmentTypeCode.UpsOnLineTools];
            upsSetupWizard.ShowDialog(window);
        }
    }
}