using System;
using System.Linq;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Settings;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    /// <summary>
    /// Settings control for Channel Advisor stores
    /// </summary>
    public partial class ChannelAdvisorSettingsControl : StoreSettingsControlBase
    {
        private readonly bool showAmazonSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelAdvisorSettingsControl()
        {
            InitializeComponent();

            // Show Amazon control if the Amazon carrier is configured. We need to save this to a variable
            // because we need it later on and we can't rely on whether the Amazon control is visible because
            // it won't be until the layout is finished.
            IShippingSettingsEntity settings = ShippingSettings.FetchReadOnly();
            showAmazonSettings = settings.ConfiguredTypes.Contains(ShipmentTypeCode.Amazon);
            amazon.Visible = showAmazonSettings;
            daysBack.MaxDaysBack = 7;
        }

        /// <summary>
        /// Load the data from the given store into the control
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            ChannelAdvisorStoreEntity caStore = store as ChannelAdvisorStoreEntity;
            if (caStore == null)
            {
                throw new InvalidOperationException("A non Channel Advisor store was passed to the Channel Advisor store settings control.");
            }

            attributes.LoadStore(caStore);
            consolidator.LoadStore(caStore);
            amazon.LoadStore(caStore);
            daysBack.LoadStore(caStore);
        }

        /// <summary>
        /// Save the data into the StoreEntity.  Nothing is saved to the database.
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            ChannelAdvisorStoreEntity caStore = store as ChannelAdvisorStoreEntity;
            if (caStore == null)
            {
                throw new InvalidOperationException("A non Channel Advisor store was passed to the Channel Advisor store settings control.");
            }

            try
            {
                attributes.SaveToEntity(caStore);
                consolidator.SaveToEntity(caStore);
                amazon.SaveToEntity(caStore);
                daysBack.SaveToEntity(caStore);
            }
            catch (Exception ex) when (ex.GetType() == typeof(ChannelAdvisorException) || ex.GetType() == typeof(AmazonShippingException))
            {
                MessageHelper.ShowError(this, ex.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Called when Attributes Control is resized
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnAttributesResize(object sender, EventArgs e)
        {
            Height = attributes.Height + consolidator.Height + daysBack.Height + 30;

            // Adjust height if the Amazon control is visible
            if (showAmazonSettings)
            {
                Height += amazon.Height;
            }
        }
    }
}
