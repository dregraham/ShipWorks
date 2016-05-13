using System;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Odbc;

namespace ShipWorks.Stores.UI.Platforms.Odbc.WizardPages
{
    /// <summary>
    /// Wizard page for choosing an import map to load or to start creating a new one
    /// </summary>
    public partial class OdbcImportFieldMappingPage : AddStoreWizardPage, IOdbcWizardPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcImportFieldMappingPage"/> class.
        /// </summary>
        public OdbcImportFieldMappingPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The position in which to show this wizard page
        /// </summary>
        public int Position => 1;

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
                OdbcStoreEntity store = GetStore<OdbcStoreEntity>();
                OdbcDataSource dataSource = scope.Resolve<OdbcDataSource>();
                dataSource.Restore(store.ConnectionString);

                IOdbcSchema schema = scope.Resolve<IOdbcSchema>();

                schema.Load(dataSource);
                IDialog dlg = factory.CreateOdbcImportFieldMappingDlg(Parent.Parent, schema.Tables);
                dlg.ShowDialog();
            }
        }
    }
}
