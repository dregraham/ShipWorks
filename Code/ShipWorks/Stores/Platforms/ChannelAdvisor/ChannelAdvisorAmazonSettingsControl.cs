using System.ComponentModel;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    /// <summary>
    /// CA-specific store settings
    /// </summary>
    [ToolboxItem(true)]
    public partial class ChannelAdvisorAmazonSettingsControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelAdvisorAmazonSettingsControl()
        {
            InitializeComponent();

            countries.Items.Add("Germany");
            countries.Items.Add("United Kingdom");
            countries.Items.Add("United States");

            countries.SelectedItem = "United States";
        }

        /// <summary>
        /// Populate the UI with values from the store entity
        /// </summary>
        public void LoadStore(ChannelAdvisorStoreEntity caStore)
        {
            MethodConditions.EnsureArgumentIsNotNull(caStore, nameof(caStore));

            countries.SelectedItem = Geography.GetCountryName(caStore.AmazonApiRegion);
            merchantID.Text = caStore.AmazonMerchantID;
            authToken.Text = caStore.AmazonAuthToken;
        }

        /// <summary>
        /// Save the UI settings to the store entity
        /// </summary>
        public void SaveToEntity(ChannelAdvisorStoreEntity caStore)
        {
            MethodConditions.EnsureArgumentIsNotNull(caStore, nameof(caStore));

            caStore.AmazonApiRegion = Geography.GetCountryCode((string)countries.SelectedItem);
            caStore.AmazonMerchantID = merchantID.Text;
            caStore.AmazonAuthToken = authToken.Text;
        }
    }
}
