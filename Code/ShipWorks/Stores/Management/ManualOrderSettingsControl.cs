using System;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Management
{
    /// <summary>
    /// UserControl for editing manual order settings for a store
    /// </summary>
    public partial class ManualOrderSettingsControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ManualOrderSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the manual order settings from the given store
        /// </summary>
        public virtual void LoadStore(StoreEntity store)
        {
            prefix.Text = store.ManualOrderPrefix;
            postfix.Text = store.ManualOrderPostfix;

            UpdateExample();
        }

        /// <summary>
        /// Save the data from the control into the given entity
        /// </summary>
        public virtual void SaveToEntity(StoreEntity store)
        {
            store.ManualOrderPrefix = prefix.Text.Trim();
            store.ManualOrderPostfix = postfix.Text.Trim();
        }

        /// <summary>
        /// User has changed what's typed into the boxes
        /// </summary>
        private void OnChangePrefixPostfix(object sender, EventArgs e)
        {
            UpdateExample();
        }

        /// <summary>
        /// Update the example text using the specified prefix\postfix
        /// </summary>
        private void UpdateExample()
        {
            example.Text = string.Format("{0}1045{1}", prefix.Text, postfix.Text);
        }
    }
}
