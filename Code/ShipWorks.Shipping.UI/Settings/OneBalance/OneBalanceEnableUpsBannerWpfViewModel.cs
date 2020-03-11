using System;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Shipping.Carriers.UPS;

namespace ShipWorks.Shipping.UI.Settings.OneBalance
{
    /// <summary>
    /// View model for the OneBalanceEnableUpsBannerWpfControl
    /// </summary>
    [Component]
    public class OneBalanceEnableUpsBannerWpfViewModel : ViewModelBase, IOneBalanceEnableUpsBannerWpfViewModel
    {
        private readonly UpsSetupWizard setupWizard;
        private readonly IWin32Window window;

        public event EventHandler SetupComplete;

        /// <summary>
        /// Constructor
        /// </summary>
        public OneBalanceEnableUpsBannerWpfViewModel(UpsSetupWizard setupWizard, IWin32Window window)
        {
            this.setupWizard = setupWizard;
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