using Interapptive.Shared.ComponentRegistration;
using System.Windows.Forms;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using System.Linq;
using ShipWorks.Shipping.Carriers.Postal;
using System;
using ShipWorks.Shipping.Carriers.UPS;

namespace ShipWorks.Shipping.UI.Settings.OneBalance
{
    /// <summary>
    /// WinForms host for the One Balance Settings Control
    /// </summary>
    [Component(RegistrationType.SpecificService, Service = typeof(IOneBalanceSettingsControlHost))]
    public partial class OneBalanceSettingsControlHost : UserControl, IOneBalanceSettingsControlHost
    {
        private OneBalanceSettingsControlViewModel settingsViewModel;
        private readonly IUspsAccountManager accountManager;
        private readonly Func<IPostageWebClient, IOneBalanceAddMoneyDialog> addMoneyDialogFactory;
        private readonly IOneBalanceEnableUpsBannerWpfViewModel bannerViewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public OneBalanceSettingsControlHost(IUspsAccountManager accountManager,
            Func<IPostageWebClient, IOneBalanceAddMoneyDialog> addMoneyDialogFactory, 
            IOneBalanceEnableUpsBannerWpfViewModel bannerViewModel)
        {
            InitializeComponent();
            this.accountManager = accountManager;
            this.addMoneyDialogFactory = addMoneyDialogFactory;
            this.bannerViewModel = bannerViewModel;
        }

        /// <summary>
        /// Initializes the hosted control
        /// </summary>
        public void Initialize()
        {
            settingsViewModel = new OneBalanceSettingsControlViewModel(accountManager, addMoneyDialogFactory, bannerViewModel);

            settingsControl.DataContext = settingsViewModel;
        }
    }
}
