using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Templates.Controls.XslEditing;
using ShipWorks.Templates.Tokens;

namespace ShipWorks.Stores.Management
{
    /// <summary>
    /// Editor for editing status presets
    /// </summary>
    public partial class StatusPresetEditor : UserControl
    {
        // Loaded store and type
        StoreEntity store;
        StatusPresetTarget presetTarget;

        List<StatusPresetEntity> deleted = new List<StatusPresetEntity>();

        /// <summary>
        /// Constructor
        /// </summary>
        public StatusPresetEditor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the editor
        /// </summary>
        public void LoadPresets(StoreEntity store, StatusPresetTarget presetTarget)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            this.store = store;
            this.presetTarget = presetTarget;

            listStatusGlobal.Items.Clear();
            listStatusStore.Items.Clear();
            deleted.Clear();

            foreach (StatusPresetEntity preset in StatusPresetManager.GetGlobalPresets(presetTarget))
            {
                ListViewItem item = listStatusGlobal.Items.Add(preset.StatusText);
                item.Tag = preset;
            }

            foreach (StatusPresetEntity preset in StatusPresetManager.GetStorePresets(store, presetTarget))
            {
                ListViewItem item = listStatusStore.Items.Add(preset.StatusText);
                item.Tag = preset;
            }

            defaultStatus.Text = StatusPresetManager.GetStoreDefault(store, presetTarget).StatusText;

            FillDefaultCombo();

            OnGlobalSelectionChanged(null, EventArgs.Empty);
            OnStoreSelectionChanged(null, EventArgs.Empty);
        }

        /// <summary>
        /// Save all current changes
        /// </summary>
        public void Save(SqlAdapter adapter)
        {
            SavePresets(listStatusGlobal, null, adapter);
            SavePresets(listStatusStore, store, adapter);

            foreach (StatusPresetEntity preset in deleted)
            {
                adapter.DeleteEntity(preset);
            }

            StatusPresetEntity defaultPreset = StatusPresetManager.GetStoreDefault(store, presetTarget);
            defaultPreset.StatusText = defaultStatus.Text;
            adapter.SaveAndRefetch(defaultPreset);
        }

        /// <summary>
        /// Save the presets from the given list, to the specified store
        /// </summary>
        private void SavePresets(ListView statusList, StoreEntity store, SqlAdapter adapter)
        {
            foreach (ListViewItem item in statusList.Items)
            {
                StatusPresetEntity preset = item.Tag as StatusPresetEntity;
                if (preset == null)
                {
                    // Its new, we have to create it
                    preset = new StatusPresetEntity();
                    preset.StoreID = store != null ? store.StoreID : (long?) null;
                    preset.StatusTarget = (int) presetTarget;
                    preset.IsDefault = false;
                }

                preset.StatusText = item.Text;
                adapter.SaveAndRefetch(preset);
            }
        }

        /// <summary>
        /// The selection of global preset changed
        /// </summary>
        private void OnGlobalSelectionChanged(object sender, EventArgs e)
        {
            editStatusGlobal.Enabled = listStatusGlobal.SelectedItems.Count > 0;
            deleteStatusGlobal.Enabled = listStatusGlobal.SelectedItems.Count > 0;
        }

        /// <summary>
        /// Selection in the store specific list changed
        /// </summary>
        private void OnStoreSelectionChanged(object sender, EventArgs e)
        {
            editStatusStore.Enabled = listStatusStore.SelectedItems.Count > 0;
            deleteStatusStore.Enabled = listStatusStore.SelectedItems.Count > 0;
        }

        /// <summary>
        /// Create a new global preset
        /// </summary>
        private void OnNewPreset(object sender, EventArgs e)
        {
            // They are tokenized, so use the token editor
            using (TemplateTokenEditorDlg dlg = new TemplateTokenEditorDlg())
            {
                dlg.TokenText = "New Preset";

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    ListViewItem item = GetList(sender).Items.Add(dlg.TokenText);
                    item.Selected = true;
                }
            }
        }

        /// <summary>
        /// Edit the current selected item
        /// </summary>
        private void OnEditPreset(object sender, EventArgs e)
        {
            ListView listView = GetList(sender);

            string preset = listView.SelectedItems[0].Text;

            // They are tokenized, so use the token editor
            using (TemplateTokenEditorDlg dlg = new TemplateTokenEditorDlg())
            {
                dlg.TokenText = preset;

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    listView.SelectedItems[0].Text = dlg.TokenText;

                    listView.Sort();
                    FillDefaultCombo();
                }
            }
        }

        /// <summary>
        /// Deleting preset
        /// </summary>
        private void OnDeleteGlobalPreset(object sender, EventArgs e)
        {
            ListView listView = GetList(sender);
            ListViewItem item = listView.SelectedItems[0];

            StatusPresetEntity preset = item.Tag as StatusPresetEntity;
            if (preset != null)
            {
                deleted.Add(preset);
            }

            int index = item.Index;

            listView.Items.Remove(item);

            if (listView.Items.Count > index)
            {
                listView.Items[index].Selected = true;
            }
            else if (listView.Items.Count > 0)
            {
                listView.Items[index - 1].Selected = true;
            }

            FillDefaultCombo();
        }

        /// <summary>
        /// Fill the combo for choosing defaults with the available status
        /// </summary>
        private void FillDefaultCombo()
        {
            defaultStatus.Items.Clear();

            foreach (ListViewItem item in listStatusGlobal.Items)
            {
                defaultStatus.Items.Add(item.Text);
            }

            foreach (ListViewItem item in listStatusStore.Items)
            {
                defaultStatus.Items.Add(item.Text);
            }
        }

        /// <summary>
        /// Get the list associated with the specied event raiser
        /// </summary>
        private ListView GetList(object sender)
        {
            ListView list = sender as ListView;
            if (list != null)
            {
                return list;
            }

            if (sender == editStatusStore ||
                sender == newStatusStore ||
                sender == deleteStatusStore)
            {
                return listStatusStore;
            }
            else
            {
                return listStatusGlobal;
            }
        }
    }
}
