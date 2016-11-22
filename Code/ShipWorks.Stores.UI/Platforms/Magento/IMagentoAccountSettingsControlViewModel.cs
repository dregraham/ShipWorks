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
        /// <value>
        /// The magento version.
        /// </value>
        MagentoVersion MagentoVersion { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        SecureString Password { get; set; }

        /// <summary>
        /// Gets or sets the store code.
        /// </summary>
        /// <value>
        /// The store code.
        /// </value>
        string StoreCode { get; set; }

        /// <summary>
        /// Gets or sets the store URL.
        /// </summary>
        /// <value>
        /// The store URL.
        /// </value>
        string StoreUrl { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        string Username { get; set; }

        /// <summary>
        /// Saves the specified magento store entity.
        /// </summary>
        /// <param name="magentoStoreEntity">The magento store entity.</param>
        void Save(MagentoStoreEntity magentoStoreEntity);

        /// <summary>
        /// Loads the specified store.
        /// </summary>
        /// <param name="store">The store.</param>
        void Load(MagentoStoreEntity store);
    }
}