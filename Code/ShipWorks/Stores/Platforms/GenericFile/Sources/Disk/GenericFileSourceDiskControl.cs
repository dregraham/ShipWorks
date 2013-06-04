using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.UI;
using System.IO;

namespace ShipWorks.Stores.Platforms.GenericFile.Sources.Disk
{
    /// <summary>
    /// Control for editing the settings of importing files from disk
    /// </summary>
    public partial class GenericFileSourceDiskControl : GenericFileSourceSettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GenericFileSourceDiskControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the settings from the given store into the control
        /// </summary>
        public override void LoadStore(GenericFileStoreEntity store)
        {
            actionsControl.Initialize(

                // Success options
                new object[]
                {
                     new { Value = GenericFileSuccessAction.Move, Display = "Move the file" },
                     new { Value = GenericFileSuccessAction.Delete, Display = "Delete the file" }
                },

                // Error options
                new object[]
                 {
                     new { Value = GenericFileErrorAction.Stop, Display = "Stop importing and display the error" },
                     new { Value = GenericFileErrorAction.Move, Display = "Move the file and continue importing" }
                 });

            folderPath.Text = store.DiskFolder;

            // The following settings are shared in the database, but only make sense to load if we are the source they are for - otherwise
            // we'll just let them start off as the defauts.
            if (store.FileSource == (int) GenericFileSourceTypeCode.Disk)
            {
                importMustMatch.Checked = store.NamePatternMatch != null;
                importMustMatchPattern.Text = store.NamePatternMatch ?? "";

                // Can't match
                importCantMatch.Checked = store.NamePatternSkip != null;
                importCantMatchPattern.Text = store.NamePatternSkip ?? "";

                // Actions
                actionsControl.LoadStore(store);
            }
        }

        /// <summary>
        /// Save the settings from the control into the store
        /// </summary>
        public override bool SaveToEntity(GenericFileStoreEntity store)
        {
            if (!actionsControl.CheckValidFolder(folderPath.Text, "import folder"))
            {
                return false;
            }

            store.FileSource = (int) GenericFileSourceTypeCode.Disk;
            store.DiskFolder = folderPath.Text;

            store.NamePatternMatch = importMustMatch.Checked ? importMustMatchPattern.Text : null;
            store.NamePatternSkip = importCantMatch.Checked ? importCantMatchPattern.Text : null;

            return actionsControl.SaveToEntity(store, store.DiskFolder);
        }

        /// <summary>
        /// Changing the must-match check box state
        /// </summary>
        private void OnCheckedChangedMustMatch(object sender, EventArgs e)
        {
            importMustMatchPattern.Enabled = importMustMatch.Checked;
        }

        /// <summary>
        /// Changing the cant-match check box state
        /// </summary>
        private void OnCheckedChangedCantMatch(object sender, EventArgs e)
        {
            importCantMatchPattern.Enabled = importCantMatch.Checked;
        }

        /// <summary>
        /// Browse for the folder to import from
        /// </summary>
        private void OnBrowseImportFolder(object sender, EventArgs e)
        {
            folderBrowser.SelectedPath = folderPath.Text;

            if (folderBrowser.ShowDialog(this) == DialogResult.OK)
            {
                folderPath.Text = folderBrowser.SelectedPath;
            }
        }

        /// <summary>
        /// Browse for the folder to move to after succesful processing
        /// </summary>
        private void OnBrowseForSuccessFolder(object sender, GenericFileSourceFolderBrowseEventArgs e)
        {
            folderBrowser.SelectedPath = e.Folder;

            if (folderBrowser.ShowDialog(this) == DialogResult.OK)
            {
                e.Folder = folderBrowser.SelectedPath;
            }
        }

        /// <summary>
        /// Browse for the folder to move to after an error during processing
        /// </summary>
        private void OnBrowseForErrorFolder(object sender, GenericFileSourceFolderBrowseEventArgs e)
        {
            folderBrowser.SelectedPath = e.Folder;

            if (folderBrowser.ShowDialog(this) == DialogResult.OK)
            {
                e.Folder = folderBrowser.SelectedPath;
            }
        }

        /// <summary>
        /// Adjust our own height to be big enough to hold the actions control size
        /// </summary>
        private void OnActionsSizeChanged(object sender, EventArgs e)
        {
            Height = actionsControl.Bottom;
        }
    }
}
