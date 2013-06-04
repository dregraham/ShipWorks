using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Stores.Management;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Newegg
{
    /// <summary>
    /// Store-specific configuration settings for Newegg. 
    /// </summary>
    public partial class NeweggDownloadStoreSettingsControl : StoreSettingsControlBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NeweggDownloadStoreSettingsControl" /> class.
        /// </summary>
        public NeweggDownloadStoreSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the data from the given store into the control
        /// </summary>
        /// <param name="store"></param>
        /// <exception cref="System.InvalidOperationException">A non-Newegg store was passed to the Newegg store settings control.</exception>
        public override void LoadStore(StoreEntity store)
        {
            NeweggStoreEntity neweggStore = store as NeweggStoreEntity;
            if (neweggStore == null)
            {
                throw new InvalidOperationException("A non-Newegg store was passed to the Newegg store settings control.");
            }

            excludeFulfilledByNewegg.Checked = neweggStore.ExcludeFulfilledByNewegg;
        }

        /// <summary>
        /// Save the data into the StoreEntity.  Nothing is saved to the database.
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">A non-Newegg store was passed to the Newegg store settings control.</exception>
        public override bool SaveToEntity(StoreEntity store)
        {
            NeweggStoreEntity neweggStore = store as NeweggStoreEntity;
            if (neweggStore == null)
            {
                throw new InvalidOperationException("A non-Newegg store was passed to the Newegg store settings control.");
            }

            neweggStore.ExcludeFulfilledByNewegg = excludeFulfilledByNewegg.Checked;

            return true;
        }
    }
}
