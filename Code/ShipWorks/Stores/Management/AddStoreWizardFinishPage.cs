using System;
using System.Drawing;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore;
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
        public void LoadDownloadControl()
        {
            StoreEntity store = GetStore<StoreEntity>();

            Control finishPageControl = CreateFinishPageControl(store.StoreTypeCode).Create(store);
            addStoreWizardFinishPanel.Controls.Add(finishPageControl);
            addStoreWizardFinishPanel.Size = finishPageControl.Size;

            otherMessagingPanel.Location = new Point(addStoreWizardFinishPanel.Location.X,
                    addStoreWizardFinishPanel.Location.Y + addStoreWizardFinishPanel.Size.Height);
        }

        /// <summary>
        /// Get the control to display at the top of the page
        /// </summary>
        private IStoreWizardFinishPageControlFactory CreateFinishPageControl(StoreTypeCode storeTypeCode)
        {
            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                return scope.IsRegisteredWithKey<IStoreWizardFinishPageControlFactory>(storeTypeCode) ?
                    scope.ResolveKeyed<IStoreWizardFinishPageControlFactory>(storeTypeCode) :
                    scope.Resolve<IStoreWizardFinishPageControlFactory>();
            }
        }

        /// <summary>
        /// User clicked the link to open the getting started guide
        /// </summary>
        private void OnLinkGettingStartedGuide(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("https://shipworks.zendesk.com/hc/en-us/articles/360022464752-ShipWorks-User-Manual", this);
        }
    }
}
