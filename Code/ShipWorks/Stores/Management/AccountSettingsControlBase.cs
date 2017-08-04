using System.ComponentModel;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Management
{
    /// <summary>
    /// Base class for all controls that expose the account settings of a store to be shown in the Store Settings window
    /// </summary>
    [ToolboxItem(false)]
    public partial class AccountSettingsControlBase : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AccountSettingsControlBase()
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
        protected virtual bool SaveToEntity(StoreEntity store) => true;
    }
}
