using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.Amazon.WizardPages
{
    /// <summary>
    /// Page for entering Amazon user credentials
    /// </summary>
    public partial class AmazonCredentialsPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonCredentialsPage()
        {
            InitializeComponent();

            StepNextAsync = OnStepNextAsync;
        }

        /// <summary>
        /// User is navigating to the next wizard page
        /// </summary>
        private async Task OnStepNextAsync(object sender, WizardStepEventArgs e)
        {
            AmazonStoreEntity store = GetStore<AmazonStoreEntity>();

            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                IMessageHelper messageHelper = lifetimeScope.Resolve<IMessageHelper>();

                using (messageHelper.SetCursor(Cursors.WaitCursor))
                {
                    if (!await accountSettings.SaveToEntityAsync(store).ConfigureAwait(true))
                    {
                        // there was an error, stay on this page
                        e.NextPage = this;
                        return;
                    }

                    // set the store name here
                    store.StoreName = store.MerchantName;

                    // this store is using the old APIs
                    store.AmazonApi = (int) AmazonApi.LegacySoap;
                }
            }
        }
    }
}
