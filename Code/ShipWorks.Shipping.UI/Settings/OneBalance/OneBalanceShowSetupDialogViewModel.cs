using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;
using Autofac.Features.Indexed;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.UI.Settings.OneBalance
{
    /// <summary>
    /// View model for the OneBalanceEnableUpsBannerWpfControl
    /// </summary>
    [KeyedComponent(typeof(IOneBalanceShowSetupDialogViewModel), ShipmentTypeCode.UpsOnLineTools)]
    public class OneBalanceShowSetupDialogViewModel : ViewModelBase, IOneBalanceShowSetupDialogViewModel
    {
        private readonly IIndex<ShipmentTypeCode, IOneBalanceSetupWizard> setupWizardFactory;
        private readonly IWin32Window window;
        private readonly IUspsAccountManager accountManager;
        private readonly IMessageHelper messageHelper;
        private IUspsWebClient webClient;

        public event EventHandler SetupComplete;

        /// <summary>
        /// Constructor
        /// </summary>
        public OneBalanceShowSetupDialogViewModel(IIndex<ShipmentTypeCode, IOneBalanceSetupWizard> setupWizardFactory, IWin32Window window)
        {
            this.setupWizardFactory = setupWizardFactory;
            this.window = window;
            this.accountManager = accountManager;
            ShowSetupWizardCommand = new RelayCommand<ShipmentTypeCode>((shipmentTypeCode)=>ShowSetupWizard(shipmentTypeCode));
        }

        /// <summary>
        /// RelayCommand for Showing the setup dialog
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand ShowSetupWizardCommand { get; }

        /// <summary>
        /// Shows the setup dialog to the user
        /// </summary>
        protected virtual void ShowSetupWizard(ShipmentTypeCode shipmentTypeCode)
        {
            var setupWizard = setupWizardFactory[shipmentTypeCode];
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