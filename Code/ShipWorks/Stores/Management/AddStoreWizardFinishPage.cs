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

            StoreEntity store = GetStore<StoreEntity>();
            StoreType storeType = StoreTypeManager.GetType(store);
            UserControl finishPageControl = storeType.CreateWizardFinishPageControl();
            addStoreWizardFinishPanel.Controls.Add(finishPageControl);
            addStoreWizardFinishPanel.Size = finishPageControl.Size;
            otherMessagingPanel.Location =
                new Point(addStoreWizardFinishPanel.Location.X + addStoreWizardFinishPanel.Size.Width,
                    addStoreWizardFinishPanel.Location.Y + addStoreWizardFinishPanel.Size.Height);
        }
        
        /// <summary>
        /// Loads the given control into the download section of the page 
        /// </summary>
        /// <param name="downloadControl"></param>
        public void SetDownloadSection(UserControl downloadControl)
        {
            downloadPanel.Controls.Clear();
            downloadPanel.Controls.Add(downloadControl);
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
