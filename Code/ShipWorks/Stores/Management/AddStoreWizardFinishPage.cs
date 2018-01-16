using System;
using System.Drawing;
using System.Windows.Forms;
using Interapptive.Shared.Net;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Management
{
    /// <summary>
    /// Finish page for the add store wizard
    /// </summary>
    public partial class AddStoreWizardFinishPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AddStoreWizardFinishPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load messaging based on store
        /// </summary>
        public void LoadDownloadMessage()
        {
            StoreEntity store = GetStore<StoreEntity>();
            StoreType storeType = StoreTypeManager.GetType(store);
            UserControl finishPageControl = storeType.CreateWizardFinishPageControl();
            addStoreWizardFinishPanel.Controls.Add(finishPageControl);
            addStoreWizardFinishPanel.Size = finishPageControl.Size;
            otherMessagingPanel.Location =
                new Point(addStoreWizardFinishPanel.Location.X,
                    addStoreWizardFinishPanel.Location.Y + addStoreWizardFinishPanel.Size.Height);
        }

        /// <summary>
        /// User clicked the link to open the getting started guide
        /// </summary>
        private void OnLinkGettingStartedGuide(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("http://support.shipworks.com/support/solutions/articles/104800-getting-started-with-shipworks", this);
        }
    }
}
