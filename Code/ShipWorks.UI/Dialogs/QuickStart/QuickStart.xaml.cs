using System.Windows;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Administration;
using ShipWorks.Filters.Management;
using ShipWorks.Shipping.Settings;
using ShipWorks.Stores.Management;
using ShipWorks.Templates.Management;
using ShipWorks.UI.Dialogs.DefaultPrinters;

namespace ShipWorks.UI.Dialogs.QuickStart
{
    /// <summary>
    /// UI logic for QuickStart
    /// </summary>
    public partial class QuickStart : IQuickStart
    {
        private readonly IWin32Window owner;

        /// <summary>
        /// ctor
        /// </summary>
        public QuickStart(IWin32Window owner) : base(owner, null, false)
        {
            this.owner = owner;
            InitializeComponent();
        }

        /// <summary>
        /// Open the add store wizard
        /// </summary>
        private void OnClickAddStore(object sender, RoutedEventArgs e)
        {
            AddStoreWizard.RunWizard(owner, OpenedFromSource.QuickStart);
        }

        /// <summary>
        /// Open click setup printers
        /// </summary>
        private void OnClickSetupPrinters(object sender, RoutedEventArgs e)
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                IDefaultPrinters defaultPrintersDialog = lifetimeScope.Resolve<IDefaultPrinters>();
                defaultPrintersDialog.ShowDialog();
            }
        }

        /// <summary>
        /// Open the shipping settings dialog
        /// </summary>
        private void OnClickShippingRules(object sender, RoutedEventArgs e)
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                using (ShippingSettingsDlg dlg = new ShippingSettingsDlg(lifetimeScope))
                {
                    dlg.ShowDialog(this);
                }
            }
        }

        /// <summary>
        /// Open the add filter wizard
        /// </summary>
        private void OnClickFilters(object sender, RoutedEventArgs e)
        {
            FilterEditingService.NewFilter(
                false,
                null,
                null,
                this);
        }

        /// <summary>
        /// Open the template manager
        /// </summary>
        private void OnClickTemplates(object sender, RoutedEventArgs e)
        {
            using (TemplateManagerDlg dlg = new TemplateManagerDlg())
            {
                dlg.ShowDialog(this);
            }
        }

        /// <summary>
        /// Navigate to the products page in the hub in the users browser
        /// </summary>
        private void OnClickInventory(object sender, RoutedEventArgs e)
        {
            WebHelper.OpenUrl("https://hub.shipworks.com/products", this);
        }
    }
}
