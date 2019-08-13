using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
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
        private readonly ILicenseService licenseService;

        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcHubPage(Func<OdbcHubViewModel> viewModelFactory, IMessageHelper messageHelper, ILicenseService licenseService)
        {
            this.viewModelFactory = viewModelFactory;
            this.messageHelper = messageHelper;
            this.licenseService = licenseService;
            InitializeComponent();
            SteppingIntoAsync += OnSteppingInto;
            StepNextAsync += OnNext;
        }

        /// <summary>
        /// The position in which to show this wizard page
        /// </summary>
        public int Position => -1;

        /// <summary>
        /// When stepping into, load and set the controls view model
        /// </summary>
        private async Task OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            EditionRestrictionLevel restrictionLevel = licenseService.CheckRestriction(EditionFeature.Warehouse, null);

            if (restrictionLevel == EditionRestrictionLevel.None)
            {
                store = GetStore<OdbcStoreEntity>();

                viewModel = viewModelFactory();
                odbcHubControl.DataContext = viewModel;

                await viewModel.LoadStoresFromHub();

                if (!viewModel.Stores.Any())
                {
                    e.Skip = true;
                    e.RaiseStepEventWhenSkipping = false;
                }
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
