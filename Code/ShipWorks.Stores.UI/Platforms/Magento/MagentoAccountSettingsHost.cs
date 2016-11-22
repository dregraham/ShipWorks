using System.Windows.Forms;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using Interapptive.Shared.UI;
using System;
using ShipWorks.Stores.Platforms.Magento;

namespace ShipWorks.Stores.UI.Platforms.Magento
{
    /// <summary>
    /// Host for the WPF MagentoAccountSettingsControl
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    [KeyedComponent(typeof(AccountSettingsControlBase), StoreTypeCode.Magento)]
    public partial class MagentoAccountSettingsHost : AccountSettingsControlBase
    {
        private IMessageHelper messageHelper;
        private IMagentoAccountSettingsControlViewModel viewModel;

        public MagentoAccountSettingsHost(IMagentoAccountSettingsControlViewModel viewModel, IMessageHelper messageHelper)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            ((MagentoAccountSettingsControl) elementHost.Child).DataContext = viewModel;
            this.messageHelper = messageHelper;
        }

        public override void LoadStore(StoreEntity store)
        {
            viewModel.Load((MagentoStoreEntity) store);
        }

        public override bool SaveToEntity(StoreEntity store)
        {
            try
            {
                viewModel.Save((MagentoStoreEntity) store);
                return true;
            }
            catch (MagentoException ex)
            {
                messageHelper.ShowError(ex.Message);
                return false;
            }
        }
    }
}
