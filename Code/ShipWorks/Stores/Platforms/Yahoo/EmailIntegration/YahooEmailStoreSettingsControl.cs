using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.Yahoo.EmailIntegration
{
    /// <summary>
    /// Settings control for Yahoo store settings
    /// </summary>
    public partial class YahooEmailStoreSettingsControl : StoreSettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public YahooEmailStoreSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the store into the control
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            importProductsControl.InitializeForStore(store.StoreID);
        }
    }
}
