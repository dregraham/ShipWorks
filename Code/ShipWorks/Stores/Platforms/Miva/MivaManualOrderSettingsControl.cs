using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.Miva
{
    /// <summary>
    /// UserControl for configuring miva manual order options
    /// </summary>
    public partial class MivaManualOrderSettingsControl : ManualOrderSettingsControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MivaManualOrderSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Changed which option to use
        /// </summary>
        private void OnOptionChanged(object sender, EventArgs e)
        {
            prefix.Enabled = radioLocal.Checked;
            postfix.Enabled = radioLocal.Checked;
            example.Enabled = radioLocal.Checked;
        }

        /// <summary>
        /// Load the settings from the store
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            base.LoadStore(store);

            MivaStoreEntity miva = (MivaStoreEntity) store;
            radioLive.Checked = miva.LiveManualOrderNumbers;
            radioLocal.Checked = !miva.LiveManualOrderNumbers;
        }

        /// <summary>
        /// Save the settings to the store
        /// </summary>
        public override void SaveToEntity(StoreEntity store)
        {
            base.SaveToEntity(store);

            MivaStoreEntity miva = (MivaStoreEntity) store;
            miva.LiveManualOrderNumbers = radioLive.Checked;
        }
    }
}
