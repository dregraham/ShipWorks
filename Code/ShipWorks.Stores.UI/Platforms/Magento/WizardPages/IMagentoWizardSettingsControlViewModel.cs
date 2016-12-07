using System.Security;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.UI.Platforms.Magento.WizardPages
{
    /// <summary>
    /// ViewModel to support MagentoWizardSettingsControl
    /// </summary>
    public interface IMagentoWizardSettingsControlViewModel
    {
        /// <summary>
        /// True if Magento1, False if Magento2
        /// </summary>
        bool IsMagento1 { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        SecureString Password { get; set; }

        /// <summary>
        /// StoreCode
        /// </summary>
        string StoreCode { get; set; }

        /// <summary>
        /// StoreUrl
        /// </summary>
        string StoreUrl { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        string Username { get; set; }

        /// <summary>
        /// Save and validate settings
        /// </summary>
        GenericResult<MagentoStoreEntity> Save(MagentoStoreEntity store);
    }
}