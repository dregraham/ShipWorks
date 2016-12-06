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
        private readonly IMessageHelper messageHelper;
        private readonly IMagentoAccountSettingsControlViewModel viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="MagentoAccountSettingsHost"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="messageHelper">The message helper.</param>
        public MagentoAccountSettingsHost(IMagentoAccountSettingsControlViewModel viewModel, IMessageHelper messageHelper)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            ((MagentoAccountSettingsControl) elementHost.Child).DataContext = viewModel;
            this.messageHelper = messageHelper;
        }

        /// <summary>
        /// Load the data from the given store into the control
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            viewModel.Load((MagentoStoreEntity) store);
        }

        /// <summary>
        /// Save the data into the StoreEntity.  Nothing is saved to the database.
        /// </summary>
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
