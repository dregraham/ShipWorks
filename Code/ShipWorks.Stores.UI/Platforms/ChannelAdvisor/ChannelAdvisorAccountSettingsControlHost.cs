using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.UI.Platforms.ChannelAdvisor
{
    /// <summary>
    /// Account settings page for ChannelAdvisor
    /// </summary>
    /// <seealso cref="ShipWorks.Stores.Management.AccountSettingsControlBase" />
    [KeyedComponent(typeof(AccountSettingsControlBase), StoreTypeCode.ChannelAdvisor)]
    public partial class ChannelAdvisorAccountSettingsControlHost : AccountSettingsControlBase
    {
        private readonly IChannelAdvisorAccountSettingsViewModel viewModel;

        public ChannelAdvisorAccountSettingsControlHost(IChannelAdvisorAccountSettingsViewModel viewModel)
        {
            this.viewModel = viewModel;
            InitializeComponent();

            storeSetupControl.DataContext = viewModel;
        }

        public override bool SaveToEntity(StoreEntity store)
        {
            return viewModel.Save(store as ChannelAdvisorStoreEntity);
        }
    }
}
