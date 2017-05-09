using Interapptive.Shared.UI;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Walmart;
using ShipWorks.Stores.UI.Platforms.Walmart.WizardPages;

namespace ShipWorks.Stores.UI.Platforms.Walmart
{
    /// <summary>
    /// Account settings page for Walmart
    /// </summary>
    [KeyedComponent(typeof(AccountSettingsControlBase), StoreTypeCode.Walmart)]
    public partial class WalmartAccountSettingsControlHost : AccountSettingsControlBase
    {
        private readonly IWalmartStoreSetupControlViewModel viewModel;
        private readonly IMessageHelper messageHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="WalmartAccountSettingsControlHost"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="messageHelper"></param>
        public WalmartAccountSettingsControlHost(IWalmartStoreSetupControlViewModel viewModel, IMessageHelper messageHelper)
        {
            this.viewModel = viewModel;
            this.messageHelper = messageHelper;
            InitializeComponent();
            storeSetupControl.DataContext = viewModel;
        }

        /// <summary>
        /// Load the data from the given store into the control
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            viewModel.Load(store as WalmartStoreEntity);
        }

        /// <summary>
        /// Save the data into the StoreEntity. Nothing is saved to the database.
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            bool success = false;
            try
            {
                viewModel.Save(store as WalmartStoreEntity);
                success = true;
            }
            catch (WalmartException ex)
            {
                messageHelper.ShowError(this, $"Error connecting to Walmart:\n{ex.Message}");
            }

            return success;
        }
    }
}
