using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.UI.Platforms.ChannelAdvisor;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    public partial class ChannelAdvisorRestAccountSettingsControl : UserControl, IChannelAdvisorRestAccountSettingsControl
    {
        private IChannelAdvisorAccountSettingsViewModel viewModel;

        public ChannelAdvisorRestAccountSettingsControl()
        {
            InitializeComponent();
        }

        public ChannelAdvisorRestAccountSettingsControl(IChannelAdvisorAccountSettingsViewModel viewModel) : this()
        {
            SetViewModel(viewModel);
        }

        private void SetViewModel(IChannelAdvisorAccountSettingsViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public void LoadStore(StoreEntity store)
        {
            throw new NotImplementedException();
        }

        public bool SaveToEntity(StoreEntity store)
        {
            throw new NotImplementedException();
        }
    }
}
