using System;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    /// <summary>
    /// Wizard page for choosing an import map to load or to start creating a new one
    /// </summary>
    public partial class OdbcImportFieldMappingPage : AddStoreWizardPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcImportFieldMappingPage"/> class.
        /// </summary>
        public OdbcImportFieldMappingPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Called when [click new map button].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnClickNewMapButton(object sender, EventArgs e)
        {
            using (var scope = IoC.BeginLifetimeScope())
            {
                OdbcImportFieldMappingDlgFactory factory = scope.Resolve<OdbcImportFieldMappingDlgFactory>();
                IDialog dlg = factory.GetOdbcImportFieldMappingDlg(this);
                dlg.ShowDialog();
            }
        }
    }
}
