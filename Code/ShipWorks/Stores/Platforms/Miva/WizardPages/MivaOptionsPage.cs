using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.Miva.WizardPages
{
    /// <summary>
    /// Page for configuring sebenza integration
    /// </summary>
    public partial class MivaOptionsPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MivaOptionsPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Stepping into the page
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            MivaStoreEntity mivaStore = GetStore<MivaStoreEntity>();

            orderStatusControl.LoadStore(mivaStore);
        }

        /// <summary>
        /// Stepping next from the sebenza page
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            MivaStoreEntity mivaStore = GetStore<MivaStoreEntity>();

            mivaStore.LiveManualOrderNumbers = livaManualOrderNumbers.Checked;

            if (!orderStatusControl.SaveToEntity(mivaStore))
            {
                e.NextPage = this;
            }
        }
    }
}
