using System.ComponentModel;
using ShipWorks.Stores.Platforms.Magento.Enums;
using ShipWorks.Data.Model.EntityClasses;
using System.Security;

namespace ShipWorks.Stores.UI.Platforms.Magento
{
    /// <summary>
    /// Interface for ViewModel for MagentoAccountSettingsControl
    /// </summary>
    public interface IMagentoAccountSettingsControlViewModel
    {
        /// <summary>
        /// Gets or sets the magento version.
        /// </summary>
        MagentoVersion MagentoVersion { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        SecureString Password { get; set; }

        /// <summary>
        /// Gets or sets the store code.
        /// </summary>
        string StoreCode { get; set; }

        /// <summary>
        /// Gets or sets the store URL.
        /// </summary>
        string StoreUrl { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        string Username { get; set; }

        /// <summary>
        /// Saves the specified magento store entity.
        /// </summary>
        void Save(MagentoStoreEntity magentoStoreEntity);

        /// <summary>
        /// Loads the specified store.
        /// </summary>
        void Load(MagentoStoreEntity store);
    }
}