using System;
using System.ComponentModel;
using System.Threading.Tasks;
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
        /// Should the save operation use the async version
        /// </summary>
        public virtual bool IsSaveAsync => false;

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
            if (IsSaveAsync)
            {
                throw new InvalidOperationException("Sync version of SaveToEntity called when control requires Async");
            }

            return true;
        }

        /// <summary>
        /// Save the data into the StoreEntity.  Nothing is saved to the database.
        /// </summary>
        public virtual Task<bool> SaveToEntityAsync(StoreEntity store)
        {
            if (!IsSaveAsync)
            {
                throw new InvalidOperationException("Async version of SaveToEntity called when control requires Sync");
            }

            return Task.FromResult(true);
        }
    }
}
