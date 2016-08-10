using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.UI.Platforms.Odbc.ViewModels.Import;
using ShipWorks.UI.Wizard;
using System;
using System.Windows.Interop;

namespace ShipWorks.Stores.UI.Platforms.Odbc.WizardPages.Import
{
    /// <summary>
    /// Wizard page for choosing an import map to load or to start creating a new one
    /// </summary>
    public partial class OdbcImportMappingPage : AddStoreWizardPage, IOdbcWizardPage, IWin32Window
    {
        private IOdbcImportMappingControlViewModel viewModel;
        private OdbcStoreEntity store;
        private readonly Func<IOdbcImportMappingControlViewModel> viewModelFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcImportMappingPage"/> class.
        /// </summary>
        public OdbcImportMappingPage(Func<IOdbcImportMappingControlViewModel> viewModelFactory)
        {
            this.viewModelFactory = viewModelFactory;
            InitializeComponent();
            SteppingInto += OnSteppingInto;
            StepNext += OnNext;
            StepBack += OnBack;
        }

        /// <summary>
        /// The position in which to show this wizard page
        /// </summary>
        public int Position => 2;

        /// <summary>
        /// Load saved map from store
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            store = GetStore<OdbcStoreEntity>();

            viewModel = viewModelFactory();
            viewModel.Load(store);

            mappingControl.DataContext = viewModel;
        }

        /// <summary>
        /// Save the map to the ODBC Store
        /// </summary>
        private void OnNext(object sender, WizardStepEventArgs e)
        {
            if (!viewModel.ValidateRequiredMappingFields())
            {
                e.NextPage = this;
                return;
            }

            viewModel.Save(store);
        }

        /// <summary>
        /// Save the map without validation. Validation will happen when they come back to this page and click next.
        /// </summary>
        private void OnBack(object sender, WizardStepEventArgs e)
        {
            viewModel.Save(GetStore<OdbcStoreEntity>());
        }
    }
}
