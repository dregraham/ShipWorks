﻿using System;
using System.Windows.Forms;
using Divelements.SandGrid;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Editing
{
    /// <summary>
    /// Window for managing dimensions profiles
    /// </summary>
    [Component(RegistrationType.Self)]
    public partial class DimensionsManagerDlg : Form
    {
        long initialProfileID = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        public DimensionsManagerDlg()
        {
            InitializeComponent();

            this.Closing += OnClosing;
        }

        /// <summary>
        /// Handles the closing event.
        /// </summary>
        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Send a message that dimensions profiles have changed.
            Messenger.Current.Send(new DimensionsProfilesChangedMessage(null));
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            LoadProfiles();
            UpdateButtonState();
        }

        /// <summary>
        /// Load all the profiles into the grid
        /// </summary>
        private void LoadProfiles()
        {
            sandGrid.Rows.Clear();

            foreach (DimensionsProfileEntity profile in DimensionsManager.Profiles)
            {
                GridRow row = new GridRow(new string[]
                    {
                        profile.Name,
                        string.Format("{0} x {1} x {2} in", profile.Length, profile.Width, profile.Height),
                        WeightConverter.Current.FormatWeight(profile.Weight)
                    });

                sandGrid.Rows.Add(row);
                row.Tag = profile;

                if (profile.DimensionsProfileID == initialProfileID)
                {
                    row.Selected = true;
                }
            }

            if (sandGrid.SelectedElements.Count == 0 && sandGrid.Rows.Count > 0)
            {
                sandGrid.Rows[0].Selected = true;
            }
        }

        /// <summary>
        /// Update the UI state of the buttons
        /// </summary>
        private void UpdateButtonState()
        {
            bool enabled = sandGrid.SelectedElements.Count > 0;

            edit.Enabled = enabled;
            delete.Enabled = enabled;
        }

        /// <summary>
        /// Selected profile is changing
        /// </summary>
        private void OnChangeSelectedProfile(object sender, Divelements.SandGrid.SelectionChangedEventArgs e)
        {
            UpdateButtonState();
        }

        /// <summary>
        /// Edit the selected profile
        /// </summary>
        private void OnEdit(object sender, EventArgs e)
        {
            DimensionsProfileEntity profile = (DimensionsProfileEntity) sandGrid.SelectedElements[0].Tag;
            initialProfileID = profile.DimensionsProfileID;

            using (DimensionsProfileDlg dlg = new DimensionsProfileDlg(profile))
            {
                dlg.ShowDialog(this);

                DimensionsManager.CheckForChangesNeeded();
                LoadProfiles();
            }
        }

        /// <summary>
        /// Double-clicked a row (do Edit)
        /// </summary>
        private void OnActivate(object sender, GridRowEventArgs e)
        {
            OnEdit(sender, EventArgs.Empty);
        }

        /// <summary>
        /// Add a new custom sheet
        /// </summary>
        private void OnAdd(object sender, EventArgs e)
        {
            DimensionsProfileEntity profile = new DimensionsProfileEntity();
            profile.Name = "Profile 1";
            profile.Length = 0;
            profile.Width = 0;
            profile.Height = 0;
            profile.Weight = 0;

            using (DimensionsProfileDlg dlg = new DimensionsProfileDlg(profile))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    initialProfileID = profile.DimensionsProfileID;

                    LoadProfiles();
                }
            }
        }

        /// <summary>
        /// Delete the selected profile
        /// </summary>
        private void OnDelete(object sender, EventArgs e)
        {
            GridRow row = (GridRow) sandGrid.SelectedElements[0];
            DimensionsProfileEntity profile = (DimensionsProfileEntity) row.Tag;

            // By default we are just asking if they want to delete
            string question = string.Format("Delete the profile '{0}'?", profile.Name);

            DialogResult result = MessageHelper.ShowQuestion(this, question);

            if (result == DialogResult.OK)
            {
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.DeleteEntity(profile);
                }

                DimensionsManager.CheckForChangesNeeded();
                LoadProfiles();
            }
        }
    }
}
