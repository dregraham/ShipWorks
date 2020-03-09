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
        /// <summary>
        /// Constructor
        /// </summary>
        public OneBalanceSettingsControlHost(IOneBalanceSettingsControlViewModel settingsModel)
        {
            InitializeComponent();
            this.settingsControl.DataContext = settingsModel;
        }
    }
}
