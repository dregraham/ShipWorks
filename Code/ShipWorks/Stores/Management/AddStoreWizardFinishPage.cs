using System;
using System.Drawing;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.WebClientEnvironments;
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

            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                Control finishPageControl = CreateFinishPageControl(store.StoreTypeCode, scope).Create(store);

                addStoreWizardFinishPanel.Controls.Add(finishPageControl);
                addStoreWizardFinishPanel.Size = finishPageControl.Size;

                otherMessagingPanel.Location = new Point(addStoreWizardFinishPanel.Location.X,
                        addStoreWizardFinishPanel.Location.Y + addStoreWizardFinishPanel.Size.Height);

                panelHubInfo.Location = new Point(otherMessagingPanel.Location.X,
                    otherMessagingPanel.Location.Y + otherMessagingPanel.Size.Height);
                panelHubInfo.Visible = scope.Resolve<ILicenseService>().IsHub;
            }
        }

        /// <summary>
        /// Get the control to display at the top of the page
        /// </summary>
        private IStoreWizardFinishPageControlFactory CreateFinishPageControl(StoreTypeCode storeTypeCode, ILifetimeScope scope) =>
            scope.IsRegisteredWithKey<IStoreWizardFinishPageControlFactory>(storeTypeCode) ?
                    scope.ResolveKeyed<IStoreWizardFinishPageControlFactory>(storeTypeCode) :
                    scope.Resolve<IStoreWizardFinishPageControlFactory>();

        /// <summary>
        /// User clicked the link to open the getting started guide
        /// </summary>
        private void OnLinkGettingStartedGuide(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("https://support.shipworks.com/hc/en-us/articles/360050817192-Quick-Start-Welcome", this);
        }

        /// <summary>
        /// Open up the hub documentation
        /// </summary>
        private void OnLinkHubDoc(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("https://support.shipworks.com/hc/en-us/articles/360054590831-Introduction-to-The-Hub", this);
        }

        /// <summary>
        /// Launch the hub
        /// </summary>
        private void OnLinkLaunchHub(object sender, EventArgs e)
        {
            string warehouseUrl;
            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                warehouseUrl = scope.Resolve<WebClientEnvironmentFactory>().SelectedEnvironment.WarehouseUrl;
            }
            WebHelper.OpenUrl(warehouseUrl, this);
        }
    }
}
