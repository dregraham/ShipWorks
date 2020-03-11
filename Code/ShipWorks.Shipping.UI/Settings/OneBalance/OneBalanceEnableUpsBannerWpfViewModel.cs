using System;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;
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
        private readonly IOneBalanceSetupWizard setupWizard;
        private readonly IWin32Window window;

        public event EventHandler SetupComplete;

        /// <summary>
        /// Constructor
        /// </summary>
        public OneBalanceEnableUpsBannerWpfViewModel(Func<ShipmentTypeCode, IOneBalanceSetupWizard> setupWizardFactory, IWin32Window window)
        {
            this.setupWizard = setupWizardFactory(ShipmentTypeCode.UpsOnLineTools);
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
            setupWizard.SetupOneBalanceAccount(window);
            RaiseSetupComplete();
        }

        /// <summary>
        /// Raises the SetupComplete event
        /// </summary>
        private void RaiseSetupComplete()
        {
            SetupComplete?.Invoke(this, EventArgs.Empty);
        }
    }
}