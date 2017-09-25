using ShipWorks.Actions.Tasks;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor.CoreExtensions.Actions
{
    /// <summary>
    /// Task editor for MarketplaceAdvisor promote orders and parcels tasks
    /// </summary>
    /// <remarks>
    /// THIS STORE IS DEAD
    /// This store is scheduled for removal as it no longer exists. Do not update this store when making
    /// all-platform changes.
    /// </remarks>
    public partial class MarketplaceAdvisorPromoteTaskEditor : ActionTaskEditor
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MarketplaceAdvisorPromoteTaskEditor(string orderOrParcel)
        {
            InitializeComponent();

            labelInfo.Text = string.Format(labelInfo.Text, orderOrParcel);
        }
    }
}
