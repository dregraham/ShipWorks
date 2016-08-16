using Interapptive.Shared.Security;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Miva
{
    /// <summary>
    /// User control for configuring the settings for a miva account
    /// </summary>
    public partial class MivaAccountSettingsControl : GenericStoreAccountSettingsControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MivaAccountSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the settings from the UI
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            base.LoadStore(store);

            MivaStoreEntity miva = (MivaStoreEntity) store;

            encryptionPassphrase.Text = SecureText.Decrypt(miva.EncryptionPassphrase, miva.ModuleUsername);
            storeCode.Text = miva.ModuleOnlineStoreCode;
        }

        /// <summary>
        /// Save the settings from the UI to the store
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            MivaStoreEntity miva = (MivaStoreEntity) store;

            miva.EncryptionPassphrase = SecureText.Encrypt(encryptionPassphrase.Text, miva.ModuleUsername);
            miva.ModuleOnlineStoreCode = storeCode.Text.Trim();

            return base.SaveToEntity(store);
        }
    }
}
