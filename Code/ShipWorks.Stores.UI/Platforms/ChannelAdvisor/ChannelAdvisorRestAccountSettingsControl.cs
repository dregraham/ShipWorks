using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.ChannelAdvisor;

namespace ShipWorks.Stores.UI.Platforms.ChannelAdvisor
{
    /// <summary>
    /// Control for showing the ChannelAdvisor Rest account settings
    /// </summary>
    [Component]
    public partial class ChannelAdvisorRestAccountSettingsControl : UserControl, IChannelAdvisorRestAccountSettingsControl
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
        /// Constructor
        /// </summary>
        public ChannelAdvisorRestAccountSettingsControl(IChannelAdvisorAccountSettingsViewModel viewModel) : this()
        {
            SetViewModel(viewModel);
        }

        /// <summary>
        /// Wire up the view model with the datacontext of the xaml
        /// </summary>
        private void SetViewModel(IChannelAdvisorAccountSettingsViewModel viewModel)
        {
            this.viewModel = viewModel;
            ((ChannelAdvisorRestAuthorizationControl) restAuthHost.Child).DataContext = viewModel;
        }

        /// <summary>
        /// Load the store into the view model
        /// </summary>
        public void LoadStore(StoreEntity store)
        {
            viewModel.Load(store as ChannelAdvisorStoreEntity);
        }

        /// <summary>
        /// Save the data into the store entity
        /// </summary>
        public bool SaveToEntity(StoreEntity store)
        {
            return viewModel.Save(store as ChannelAdvisorStoreEntity);
        }
    }
}
