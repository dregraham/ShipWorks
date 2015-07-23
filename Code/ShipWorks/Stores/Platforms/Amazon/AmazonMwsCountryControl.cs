using System;
using System.ComponentModel;
using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.Amazon
{
    /// <summary>
    /// Allow the user to select the country that their Amazon store is in
    /// </summary>
    [ToolboxItem(true)]
    public partial class AmazonMwsCountryControl : AccountSettingsControlBase
    {
        AmazonStoreEntity amazonStore;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonMwsCountryControl()
        {
            InitializeComponent();

            countries.Items.Add("Canada");
            countries.Items.Add("France");
            countries.Items.Add("Germany");
            countries.Items.Add("Italy");
            countries.Items.Add("Mexico");
            countries.Items.Add("Spain");
            countries.Items.Add("United Kingdom");
            countries.Items.Add("United States");

            countries.SelectedItem = "United States";
        }
        
        /// <summary>
        /// Load settings from the store entity
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            // validate we get an AmazonStoreEntity
            this.amazonStore = store as AmazonStoreEntity;
            if (amazonStore == null)
            {
                throw new ArgumentException("AmazonStoreEntity expected.", "store");
            }

            countries.SelectedItem = Geography.GetCountryName(amazonStore.AmazonApiRegion);

            base.LoadStore(store);
        }

        /// <summary>
        /// Save user-entered values back to the entity
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            AmazonStoreEntity saveStore = store as AmazonStoreEntity;
            if (saveStore == null)
            {
                throw new ArgumentException("AmazonStoreEntity expected.", "store");
            }

            saveStore.AmazonApiRegion = Geography.GetCountryCode((string)countries.SelectedItem);

            return true;
        }
    }
}
