using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Management
{
    /// <summary>
    /// Base control for custom store settings that appear on in the Store Settings
    /// section of the Store Settings dialog
    /// </summary>
    [ToolboxItem(false)]
    public partial class StoreSettingsControlBase : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StoreSettingsControlBase()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the data from the given store into the control
        /// </summary>
        public virtual void LoadStore(StoreEntity store)
        {

        }

        /// <summary>
        /// Save the data into the StoreEntity.  Nothing is saved to the database.
        /// </summary>
        public virtual bool SaveToEntity(StoreEntity store)
        {
            return true;
        }

        /// <summary>
        /// Save the data to platform.
        /// </summary>
        public virtual Task<bool> SaveToPlatform(StoreEntity store)
        {
            return Task.FromResult(true);
        }
    }
}
