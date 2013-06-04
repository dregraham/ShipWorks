using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using System.IO;
using ShipWorks.Common.Threading;
using System.Threading;
using Interapptive.Shared.UI;
using System.Data.SqlTypes;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.Amazon
{
    /// <summary>
    /// Store-specific configuration settings for Amazon.  This does not include
    /// connectivity settings, those are in the AccountSettingsControl
    /// </summary>
    public partial class AmazonStoreSettingsControl : StoreSettingsControlBase
    {
        bool clearCookie = false;

        /// <summary>
        /// Simple UI wrapper for Amazon Weight Fields
        /// </summary>
        class FieldWrapper
        {
            // field this is wrapping
            public AmazonWeightField WeightField { get; set; }

            // get the display name
            public override string ToString()
            {
                return AmazonWeights.GetWeightFieldDisplay(WeightField);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonStoreSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load configuration from the store entity into the UI
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            AmazonStoreEntity amazonStore = store as AmazonStoreEntity;
            if (amazonStore == null)
            {
                throw new InvalidOperationException("A non Amazon store was passed to the Amazon store settings control.");
            }

            importInventoryControl.InitializeForStore(store.StoreID);

            // get all enum values
            List<AmazonWeightField> allFields = new List<AmazonWeightField>();
            allFields.AddRange(Enum.GetValues(typeof(AmazonWeightField)).Cast<AmazonWeightField>());

            // add all selected fields first
            List<AmazonWeightField> selectedFields = AmazonWeights.GetWeightsPriority(amazonStore);
            selectedFields.ForEach(f =>
                {
                    weightsCheckedList.Items.Add(new FieldWrapper() { WeightField = f }, true);
                });

            // now add those that aren't checked
            allFields.Except(selectedFields).ToList().ForEach(f =>
                {
                    weightsCheckedList.Items.Add(new FieldWrapper() { WeightField = f }, false);
                });
        }

        /// <summary>
        /// Save user-entered data back to the store entity
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            AmazonStoreEntity amazonStore = store as AmazonStoreEntity;
            if (amazonStore == null)
            {
                throw new InvalidOperationException("A non Amazon store was passed to the Amazon store settings control.");
            }

            // collect the checked items
            List<AmazonWeightField> selectedFields = new List<AmazonWeightField>();
            foreach (FieldWrapper wrapper in weightsCheckedList.CheckedItems)
            {
                selectedFields.Add(wrapper.WeightField);
            }

            AmazonWeights.SetWeightsPriority(amazonStore, selectedFields);

            if (clearCookie)
            {
                amazonStore.Cookie = "";
                amazonStore.CookieExpires = SqlDateTime.MinValue.Value;
                amazonStore.CookieWaitUntil = amazonStore.CookieExpires;
            }

            return true;
        }

        /// <summary>
        /// Moving an item up in the list
        /// </summary>
        private void OnUpClick(object sender, EventArgs e)
        {
            int selectedIndex = weightsCheckedList.SelectedIndex;
            if (selectedIndex >= 1)
            {
                FieldWrapper selectedItem = weightsCheckedList.SelectedItem as FieldWrapper;
                if (selectedItem == null)
                {
                    return;
                }

                // remove the item at the selected index, and add it at index - 1
                bool wasChecked = weightsCheckedList.GetItemChecked(selectedIndex);
                weightsCheckedList.Items.RemoveAt(selectedIndex);

                selectedIndex--;
                weightsCheckedList.Items.Insert(selectedIndex, selectedItem);

                // reselect it
                weightsCheckedList.SelectedIndex = selectedIndex;
                weightsCheckedList.SetItemChecked(selectedIndex, wasChecked);
            }
        }

        /// <summary>
        /// Moving an item down in the list
        /// </summary>
        private void OnDownClick(object sender, EventArgs e)
        {
            int selectedIndex = weightsCheckedList.SelectedIndex;
            if (selectedIndex < weightsCheckedList.Items.Count - 1)
            {
                FieldWrapper selectedItem = weightsCheckedList.SelectedItem as FieldWrapper;
                if (selectedItem == null)
                {
                    return;
                }

                // remove the item at the selected index, and add it at index - 1
                bool wasChecked = weightsCheckedList.GetItemChecked(selectedIndex);
                weightsCheckedList.Items.RemoveAt(selectedIndex);
                selectedIndex++;
                weightsCheckedList.Items.Insert(selectedIndex, selectedItem);

                // reselect it
                weightsCheckedList.SelectedIndex = selectedIndex;
                weightsCheckedList.SetItemChecked(selectedIndex, wasChecked);
            }
        }

        /// <summary>
        /// Clear the amazon download cookie
        /// </summary>
        private void OnClearCookie(object sender, EventArgs e)
        {
            clearCookie = true;
            clearCookieButton.Enabled = false;
        }
    }
}
