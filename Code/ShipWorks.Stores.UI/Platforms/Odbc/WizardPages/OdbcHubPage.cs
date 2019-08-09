using System;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.UI.Platforms.Odbc.ViewModels;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.UI.Platforms.Odbc.WizardPages
{
    public partial class OdbcHubPage : AddStoreWizardPage, IOdbcWizardPage
    {
        private OdbcHubViewModel viewModel;
        private OdbcStoreEntity store;
        private readonly Func<OdbcHubViewModel> viewModelFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcHubPage(Func<OdbcHubViewModel> viewModelFactory)
        {
            this.viewModelFactory = viewModelFactory;
            InitializeComponent();
            SteppingInto += OnSteppingInto;
            StepNext += OnNext;
        }

        /// <summary>
        /// The position in which to show this wizard page
        /// </summary>
        public int Position => -1;

        /// <summary>
        /// When stepping into, load and set the controls view model
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            store = GetStore<OdbcStoreEntity>();

            viewModel = viewModelFactory();
            viewModel.NewStoreAction = () => { };
            viewModel.Load();

            if (viewModel.Stores.Any())
            {
                odbcHubControl.DataContext = viewModel;
            }
            else
            {
                e.Skip = true;
                e.RaiseStepEventWhenSkipping = false;
            }
        }

        /// <summary>
        /// When stepping next, save the store
        /// </summary>
        private void OnNext(object sender, WizardStepEventArgs e)
        {
            if (viewModel.SelectedStore != null)
            {
                viewModel.Save(store);
            }
            else
            {
                e.NextPage = this;
            }
        }
    }
}
