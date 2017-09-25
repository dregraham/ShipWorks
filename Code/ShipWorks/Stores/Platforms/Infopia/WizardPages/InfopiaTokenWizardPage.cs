using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.Infopia.WizardPages
{
    /// <summary>
    /// Setup Wizard page for inputting the Infopia User Token
    /// </summary>
    /// <remarks>
    /// THIS STORE IS DEAD
    /// This store is scheduled for removal as it no longer exists. Do not update this store when making
    /// all-platform changes.
    /// </remarks>
    public partial class InfopiaTokenWizardPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public InfopiaTokenWizardPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Stepping to the next page
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            InfopiaStoreEntity store = GetStore<InfopiaStoreEntity>();

            if (!accountSettings.SaveToEntity(store))
            {
                e.NextPage = this;
                return;
            }

            store.StoreName = "Infopia Store";
        }
    }
}
