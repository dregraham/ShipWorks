using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Shipping.Settings;
using System.Linq;
using ShipWorks.Shipping;
using Interapptive.Shared.UI;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    public partial class ChannelAdvisorSettingsControl : StoreSettingsControlBase
    {
        public ChannelAdvisorSettingsControl()
        {
            InitializeComponent();

            // Show Amazon control if the Amazon ctrl is configured.
            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            amazon.Visible = settings.ConfiguredTypes.Contains((int)ShipmentTypeCode.Amazon);
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
            }
            catch (ChannelAdvisorException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
                return false;
            }
            return true;
        }
        
        /// <summary>
        /// Called when Attributes Controle is resized
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnAttributesResize(object sender, EventArgs e)
        {
            Height = attributes.Height + consolidator.Height;

            // Adjust height if the Amazon control is visible
            if(amazon.Visible)
            {
                Height += amazon.Height;
            }
        }
    }
}
