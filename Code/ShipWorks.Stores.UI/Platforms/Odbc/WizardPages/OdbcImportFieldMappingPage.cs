using System;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
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
        private ILifetimeScope scope;
        private IOdbcImportFieldMappingDlgViewModel viewModel;
        private OdbcStoreEntity store;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcImportFieldMappingPage"/> class.
        /// </summary>
        public OdbcImportFieldMappingPage()
        {
            InitializeComponent();
            Load += OnLoad;
            StepNext += OnNext;
        }

        /// <summary>
        /// The position in which to show this wizard page
        /// </summary>
        public int Position => 1;

        /// <summary>
        /// Save the map to the ODBC Store
        /// </summary>
        private void OnNext(object sender, ShipWorks.UI.Wizard.WizardStepEventArgs e)
        {
            if (store == null)
            {
                store = GetStore<OdbcStoreEntity>();
            }

            if (viewModel.SelectedTable == null)
            {
                MessageHelperWrapper messageHelper = new MessageHelperWrapper(() => this);
                messageHelper.ShowError("Please setup your import map before continuing to the next page.");
                e.NextPage = this;
                return;
            }

            viewModel.Save(store);
        }

        /// <summary>
        /// Load the store into the view model
        /// </summary>
        private void OnLoad(object sender, EventArgs eventArgs)
        {
            store = GetStore<OdbcStoreEntity>();
            scope = IoC.BeginLifetimeScope();
            viewModel = scope.Resolve<IOdbcImportFieldMappingDlgViewModel>();

            viewModel.Load(store);
            odbcImportFieldMappingControl.DataContext = viewModel;
        }
    }
}
