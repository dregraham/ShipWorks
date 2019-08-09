using System;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
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
        private readonly IMessageHelper messageHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcHubPage(Func<OdbcHubViewModel> viewModelFactory, IMessageHelper messageHelper)
        {
            this.viewModelFactory = viewModelFactory;
            this.messageHelper = messageHelper;
            InitializeComponent();
            SteppingInto += OnSteppingInto;
            StepNextAsync += OnNext;
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
            odbcHubControl.DataContext = viewModel;
        }

        /// <summary>
        /// When stepping next, save the store
        /// </summary>
        private async Task OnNext(object sender, WizardStepEventArgs e)
        {
            Result result = await viewModel.Save(store).ConfigureAwait(true);

            if (result.Failure)
            {
                messageHelper.ShowError(result.Message);
                e.NextPage = this;
            }
        }
    }
}
