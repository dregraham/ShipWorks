using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;

namespace ShipWorks.Actions.UI
{
    /// <summary>
    /// A user control for displaying a list of check boxes for all the stores in the system. 
    /// </summary>
    public partial class StoreCheckBoxPanel : UserControl
    {
        public event EventHandler StoreSelectionChanged;

        // By default we only show enabled stores, but if someone tries to say a disabled one is checked, we add it in
        List<long> storesForcedDisplayed = new List<long>();

        /// <summary>
        /// Initializes a new instance of the <see cref="StoreCheckBoxPanel"/> class.
        /// </summary>
        public StoreCheckBoxPanel()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Load the panel of store check boxes
        /// </summary>
        public void LoadStores()
        {
            // Detach all event handlers prior to clearing the list
            List<CheckBox> checkBoxes = panelStores.Controls.OfType<CheckBox>().ToList();
            checkBoxes.ForEach(c => c.CheckedChanged -= OnCheckedChanged);

            panelStores.Controls.Clear();

            Point location = new Point(0, 0);

            // Go through all the stores
            foreach (StoreEntity store in StoreManager.GetAllStores().Where(s => s.Enabled || storesForcedDisplayed.Contains(s.StoreID)))
            {
                CheckBox checkBox = new CheckBox();
                checkBox.AutoSize = true;
                checkBox.Location = location;
                checkBox.Text = store.StoreName;
                checkBox.Location = location;
                checkBox.Parent = panelStores;
                checkBox.Tag = store.StoreID;
                
                location.Y = checkBox.Bottom + 4;
                checkBox.CheckedChanged += OnCheckedChanged;
            }

            panelStores.Height = location.Y + 4;
            Height = panelStores.Height + 4;
        }

        /// <summary>
        /// Called when a store check box is checked/unchecked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnCheckedChanged(object sender, EventArgs e)
        {
            if (StoreSelectionChanged != null)
            {
                // Notify interested parties that the list of selected stores has changed
                StoreSelectionChanged(this, e);
            }
        }
        
        /// <summary>
        /// Gets or sets the selected store IDs.
        /// </summary>
        /// <value>The selected stores.</value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IEnumerable<long> SelectedStoreIDs
        {
            get
            {
                List<long> selectedStoreIDs = new List<long>();
                foreach (CheckBox checkBox in panelStores.Controls.OfType<CheckBox>().Where(c => c.Checked))
                {
                    // It's possible that a store could have been deleted since the panel was initialized
                    long storeID = (long)checkBox.Tag;
                    StoreEntity store = StoreManager.GetStore(storeID);

                    if (store != null)
                    {
                        selectedStoreIDs.Add(storeID);
                    }
                }

                return selectedStoreIDs;
            }
            set
            {
                if (value == null)
                {
                    value = new long[0];
                }

                var displayedStores = panelStores.Controls.OfType<CheckBox>().Select(c => (long) c.Tag);
                var missingStores = value.Except(displayedStores);

                // If we don't have checkboxes for some of them, they may just be disabled.  Force them to be displayed anyway.
                if (missingStores.Any())
                {
                    storesForcedDisplayed.AddRange(missingStores);
                    LoadStores();
                }

                // Check all the ones that need checked
                foreach (CheckBox checkBox in panelStores.Controls.OfType<CheckBox>())
                {
                    checkBox.Checked = value.Contains((long)checkBox.Tag);
                }
            }
        }
    }
}
