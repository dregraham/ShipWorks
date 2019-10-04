using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Stores.Management;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.GenericFile.WizardPages
{
    public partial class GenericStoreHubPage : AddStoreWizardPage, IGenericStoreHubPage
    {
        private GenericStoreHubViewModel viewModel;
        private GenericFileStoreEntity store;
        private readonly Func<GenericStoreHubViewModel> viewModelFactory;
        private readonly IMessageHelper messageHelper;
        private readonly ILicenseService licenseService;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericStoreHubPage(Func<GenericStoreHubViewModel> viewModelFactory, IMessageHelper messageHelper, ILicenseService licenseService)
        {
            this.viewModelFactory = viewModelFactory;
            this.messageHelper = messageHelper;
            this.licenseService = licenseService;
            InitializeComponent();
            SteppingIntoAsync = OnSteppingInto;
            StepNextAsync = OnNext;
        }

        /// <summary>
        /// When stepping into, load and set the controls view model
        /// </summary>
        private async Task OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            EditionRestrictionLevel restrictionLevel = licenseService.CheckRestriction(EditionFeature.Warehouse, null);

            if (restrictionLevel == EditionRestrictionLevel.None)
            {
                store = GetStore<GenericFileStoreEntity>();

                viewModel = viewModelFactory();
                genericStoreHubControl.DataContext = viewModel;

                await viewModel.LoadStoresFromHub().ConfigureAwait(true);

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
