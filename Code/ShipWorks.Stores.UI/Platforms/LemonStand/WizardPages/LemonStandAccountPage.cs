using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.LemonStand.WizardPages
{
    public partial class LemonStandAccountPage : AddStoreWizardPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LemonStandAccountPage"/> class.
        /// </summary>
        public LemonStandAccountPage()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     User is moving to the next wizard page, perform any autoconfiguration or credentials saving
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            LemonStandStoreEntity store = GetStore<LemonStandStoreEntity>();

            store.StoreURL = storeURLTextbox.Text;
            store.Token = accessTokenTextbox.Text;

            if (storeURLTextbox.Text.Length == 0)
            {
                MessageHelper.ShowError(this, "Please enter your Store URL");
                e.NextPage = this;
                return;
            }

            if (accessTokenTextbox.Text.Length == 0)
            {
                MessageHelper.ShowError(this, "Please enter your Access Token");
                e.NextPage = this;
                return;
            }

            try
            {
                LemonStandWebClient client = new LemonStandWebClient(store);
                //Check to see if we have access to LemonStand with the new creds
                //Ask for some orders
                client.GetOrders(1);
            }
            catch (LemonStandException ex)
            {
                ShowConnectionException(ex);
                e.NextPage = this;
            }
        }

        /// <summary>
        ///     Hook to allow derivatives add custom error handling for connectivity testing failures.
        ///     Return true to indicate the error has been handled.
        /// </summary>
        protected virtual void ShowConnectionException(LemonStandException ex)
        {
            MessageHelper.ShowError(this, ex.Message);
        }
    }
}