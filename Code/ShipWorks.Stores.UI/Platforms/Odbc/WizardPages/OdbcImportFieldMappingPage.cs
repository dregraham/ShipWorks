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
            Load += OnLoad;
        }

        private void OnLoad(object sender, EventArgs eventArgs)
        {
            using (var scope = IoC.BeginLifetimeScope())
            {
                OdbcStoreEntity store = GetStore<OdbcStoreEntity>();
                OdbcDataSource dataSource = scope.Resolve<OdbcDataSource>();
                dataSource.Restore(store.ConnectionString);

                IOdbcSchema schema = scope.Resolve<IOdbcSchema>();
                schema.Load(dataSource);

                IOdbcImportFieldMappingDlgViewModel viewModel = scope.Resolve<IOdbcImportFieldMappingDlgViewModel>();
                viewModel.Load(schema.Tables);
                odbcImportFieldMappingControl.DataContext = viewModel;
            }
        }

        /// <summary>
        /// The position in which to show this wizard page
        /// </summary>
        public int Position => 1;
    }
}
