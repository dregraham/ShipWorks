using System.Windows.Forms;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.UI.Platforms.Magento
{
    /// <summary>
    /// Host for the WPF MagentoAccountSettingsControl
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    [KeyedComponent(typeof(AccountSettingsControlBase), StoreTypeCode.Magento)]
    public partial class MagentoAccountSettingsHost : AccountSettingsControlBase
    {
        public MagentoAccountSettingsHost()
        {
            InitializeComponent();
        }
    }
}
