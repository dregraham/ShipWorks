using System;
using Autofac;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.UI.Platforms.Odbc.WizardPages
{
    /// <summary>
    /// Wizard page for choosing an import map to load or to start creating a new one
    /// </summary>
    public partial class OdbcImportFieldMappingPage : AddStoreWizardPage, IOdbcWizardPage
    {
        private ILifetimeScope scope;
        private IOdbcImportFieldMappingControlViewModel viewModel;
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
        private void OnNext(object sender, WizardStepEventArgs e)
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
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private void OnLoad(object sender, EventArgs eventArgs)
        {
            store = GetStore<OdbcStoreEntity>();
            scope = IoC.BeginLifetimeScope();
            viewModel = scope.Resolve<IOdbcImportFieldMappingControlViewModel>();

            viewModel.Load(store);
            odbcImportFieldMappingControl.DataContext = viewModel;
        }


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            scope?.Dispose();

            if (disposing)
            {
                components?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
