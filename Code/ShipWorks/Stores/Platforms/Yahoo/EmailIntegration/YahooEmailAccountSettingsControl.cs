using Interapptive.Shared.Security;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.Yahoo.EmailIntegration
{
    /// <summary>
    /// Settings control for yahoo account settings
    /// </summary>
    public partial class YahooEmailAccountSettingsControl : AccountSettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public YahooEmailAccountSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the settings from the store into the UI
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            YahooStoreEntity yahoo = (YahooStoreEntity) store;

            emailAccountControl.InitializeForStore(yahoo);

            if (yahoo.TrackingUpdatePassword.Length > 0)
            {
                trackingPassword.Text = SecureText.Decrypt(yahoo.TrackingUpdatePassword, "yahoo");
            }
        }

        /// <summary>
        /// Save the settings from the store into the UI
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            YahooStoreEntity yahoo = (YahooStoreEntity) store;

            if (trackingPassword.Text.Length > 0)
            {
                yahoo.TrackingUpdatePassword = SecureText.Encrypt(trackingPassword.Text, "yahoo");
            }
            else
            {
                yahoo.TrackingUpdatePassword = "";
            }

            return base.SaveToEntity(store);
        }
    }
}
