using System;
using System.Linq;
using ShipWorks.Stores.Management;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping;
using ShipWorks.Stores.Platforms.Amazon;
using ShipWorks.Shipping.Carriers.Amazon;
using Interapptive.Shared.UI;

namespace ShipWorks.Stores.UI.Platforms.GenericModule
{
    /// <summary>
    /// Settings for Generic Module store
    /// </summary>
    [KeyedComponent(typeof(StoreSettingsControlBase), StoreTypeCode.GenericModule)]
    public partial class GenericModuleStoreSettingsControl : StoreSettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GenericModuleStoreSettingsControl()
        {
            InitializeComponent();

            // Show Amazon control if the Amazon ctrl is configured.
            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            amazon.Visible = settings.ConfiguredTypes.Contains(ShipmentTypeCode.Amazon);
        }

        /// <summary>
        /// Load the data from the given store into the control
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            IAmazonCredentials amazonCredentials = store as IAmazonCredentials;

            if (amazonCredentials == null)
            {
                throw new InvalidOperationException("A non Generic Module store was passed to the Channel Advisor store settings control.");
            }

            amazon.LoadStore(amazonCredentials);
        }

        /// <summary>
        /// Save the data into the StoreEntity.  Nothing is saved to the database.
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            IAmazonCredentials amazonCredentials = store as IAmazonCredentials;

            if (amazonCredentials == null)
            {
                throw new InvalidOperationException("A non Generic Module store was passed to the Channel Advisor store settings control.");
            }

            try
            {
                amazon.SaveToEntity(amazonCredentials);
            }
            catch (AmazonShippingException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
                return false;
            }
            return true;
        }
    }
}
