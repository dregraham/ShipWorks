using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Autofac;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Administration;
using ShipWorks.Filters.Management;
using ShipWorks.Shipping.Settings;
using ShipWorks.Stores.Management;
using ShipWorks.Templates.Management;

namespace ShipWorks.UI.Dialogs.QuickStart
{
    /// <summary>
    /// UI logic for QuickStart
    /// </summary>
    public partial class QuickStart : IQuickStart
    {
        /// <summary>
        /// ctor
        /// </summary>
        public QuickStart(IWin32Window owner) : base(owner, null, false)
        {
            InitializeComponent();
        }

        /// <summary>
        /// Open the add store wizard
        /// </summary>
        private void OnClickAddStore(object sender, RoutedEventArgs e)
        {
            AddStoreWizard.RunWizard(this, OpenedFromSource.QuickStart);
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

        /// <summary>
        /// Close handler
        /// </summary>
        private void Close(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
