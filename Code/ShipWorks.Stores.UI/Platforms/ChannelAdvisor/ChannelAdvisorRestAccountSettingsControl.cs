using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.ChannelAdvisor;

namespace ShipWorks.Stores.UI.Platforms.ChannelAdvisor
{
    /// <summary>
    /// Control for showing the ChannelAdvisor Rest account settings
    /// </summary>
    public partial class ChannelAdvisorRestAccountSettingsControl : UserControl
    {
        private IChannelAdvisorAccountSettingsViewModel viewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelAdvisorRestAccountSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the store into the view model
        /// </summary>
        public void LoadStore(StoreEntity store, IChannelAdvisorAccountSettingsViewModel restViewModel)
        {
            viewModel = restViewModel;
            ((ChannelAdvisorRestAuthorizationControl)restAuthHost.Child).DataContext = viewModel;

            viewModel.Load(store as ChannelAdvisorStoreEntity);
        }

        /// <summary>
        /// Save the data into the store entity
        /// </summary>
        public bool SaveToEntity(StoreEntity store)
        {
            return viewModel.Save(store as ChannelAdvisorStoreEntity, true);
        }
    }
}
