using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using Interapptive.Shared.UI;
using System.IO;
using System.Reflection;
using Interapptive.Shared.Utility;
using System.Diagnostics;
using ShipWorks.ApplicationCore.Interaction;
using System.Xml.Linq;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Configuration
{
    /// <summary>
    /// Wizard for migrating configuration from ShipWorks 2
    /// </summary>
    public partial class ConfigurationMigrationWizard : WizardForm
    {
        ConfigurationMigrationSource migrationSource;
        ConfigurationMigrationAction action;

        /// <summary>
        /// Constructor
        /// </summary>
        public ConfigurationMigrationWizard(ConfigurationMigrationSource source)
        {
            InitializeComponent();

            this.migrationSource = source;

            if (migrationSource == ConfigurationMigrationSource.InPlace)
            {
                Pages.Remove(wizardPageSelectFolder);

                // We know now already (that if successful) it will be InPlace
                action = ConfigurationMigrationAction.InPlace;
            }
            else
            {
                Pages.Remove(wizardPageInPlace);

                installPath.Text = ShipWorks2xConfigurationMigrator.GetLastShipWorks2xInstallPath();
            }
        }

        /// <summary>
        /// Indicates what migration action the wizard produced.  None is returned if the user cancelled.
        /// </summary>
        public ConfigurationMigrationAction MigrationAction
        {
            get { return DialogResult == DialogResult.OK ? action : ConfigurationMigrationAction.None; }
        }

        /// <summary>
        /// Stepping next from the "In Place" upgrade page
        /// </summary>
        private void OnStepNextInPlace(object sender, WizardStepEventArgs e)
        {
            e.NextPage = wizardPageCommonCleanup;
        }

        /// <summary>
        /// Changing the choice to select a folder or not update at all
        /// </summary>
        private void OnChangeSelectFolderChoice(object sender, EventArgs e)
        {
            // This hack is to ensure no radio's are selected the first time we step into the page
            if (radioSelectFolder.Tag == null)
            {
                radioSelectFolder.Checked = false;
                radioSelectFolder.Tag = new object();
            }

            panelSelectFolderLocation.Enabled = radioSelectFolder.Checked;
            NextEnabled = radioDontUpgrade.Checked || radioSelectFolder.Checked;
        }

        /// <summary>
        /// Stepping next from the Select Folder page
        /// </summary>
        private void OnStepNextSelectFolder(object sender, WizardStepEventArgs e)
        {
            if (!radioSelectFolder.Checked && !radioDontUpgrade.Checked)
            {
                MessageHelper.ShowInformation(this, "Please make a selection to continue.");
                e.NextPage = CurrentPage;
                return;
            }

            if (radioDontUpgrade.Checked)
            {
                action = ConfigurationMigrationAction.SideBySide;
                StartupController.SetStartupAction(StartupAction.OpenDatabaseSetup, new XElement("Upgrade2x", true));

                e.NextPage = wizardPageFinish;
            }
            else
            {

                if (!ValidateShipWorks2(installPath.Text, this))
                {
                    e.NextPage = CurrentPage;
                    return;
                }

                action = ConfigurationMigrationAction.SelectedFolder;
            }
        }

        /// <summary>
        /// Browse for a ShipWorks2 installation
        /// </summary>
        private void OnBrowseShipWorks(object sender, EventArgs e)
        {
            openShipWorksPathDlg.InitialDirectory = installPath.Text;

            // Show the user the browser
            if (openShipWorksPathDlg.ShowDialog(this) == DialogResult.OK)
            {
                installPath.Text = Path.GetDirectoryName(openShipWorksPathDlg.FileName);
            }
        }

        /// <summary>
        /// Validate the shipworks path before the file dialog closes
        /// </summary>
        private void OnBrowseValidateShipWorksPath(object sender, CancelEventArgs e)
        {
            OpenFileDialog dlg = (OpenFileDialog) sender;

            if (!ValidateShipWorks2(Path.GetDirectoryName(dlg.FileName), this))
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Validate the given filename is a valid ShipWorks2 executable
        /// </summary>
        private static bool ValidateShipWorks2(string shipworksPath, IWin32Window owner)
        {
            try
            {
                if (shipworksPath.StartsWith(@"\\"))
                {
                    MessageHelper.ShowError(owner, "You cannot upgrade from an instance of ShipWorks installed on another computer.");
                    return false;
                }

                FileVersionInfo fileInfo = FileVersionInfo.GetVersionInfo(Path.Combine(shipworksPath, "shipworks.exe"));

                // At least makes sure description contains "ShipWorks", whcih ShipWorks always will
                if (string.IsNullOrEmpty(fileInfo.FileDescription) || !fileInfo.FileDescription.Contains("ShipWorks"))
                {
                    MessageHelper.ShowError(owner,
                        "The selected application is not ShipWorks.");

                    return false;
                }

                if (fileInfo.FileMajorPart == 2)
                {
                    return true;
                }

                // If this is the path we are running in, its possible we just installed on top of sw2
                if (PathUtility.IsSamePath(shipworksPath, Application.StartupPath))
                {
                    // It may be that sw3 just got installed on top of 2 on this path, and its upgradable
                    ShipWorks2xDataPaths paths = new ShipWorks2xDataPaths(shipworksPath);
                    if (File.Exists(paths.Post24SqlSessionFile) || File.Exists(paths.Pre24SqlSessionFile))
                    {
                        return true;
                    }
                }

                MessageHelper.ShowError(owner,
                    "The selected version of ShipWorks is not a version of ShipWorks 2.");

                return false;
            }
            catch (FileNotFoundException ex)
            {
                MessageHelper.ShowError(owner,
                    "The selected file is not ShipWorks 2:\n\nDetail: " + ex.Message);

                return false;
            }
        }

        /// <summary>
        /// Stepping into the Upgrade Type page
        /// </summary>
        private void OnSteppingIntoUpgradeType(object sender, WizardSteppingIntoEventArgs e)
        {
            upgrade2xPath.Text = installPath.Text;
        }

        /// <summary>
        /// Stepping next from the upgrade type page
        /// </summary>
        private void OnStepNextUpgradeType(object sender, WizardStepEventArgs e)
        {
            if (!radioUpgradeRemove.Checked && !radioUpgradeLeave.Checked)
            {
                MessageHelper.ShowInformation(this, "Please make a selection to continue.");
                e.NextPage = CurrentPage;
                return;
            }
        }

        /// <summary>
        /// Stepping into the cleanup page
        /// </summary>
        private void OnSteppingIntoCleanup(object sender, WizardSteppingIntoEventArgs e)
        {
            // If they are choosing the folder to upgrade from - but chose to leave it alone, don't ask them to do the common stuff either
            if (radioSelectFolder.Checked && radioUpgradeLeave.Checked)
            {
                e.Skip = true;
                e.RaiseStepEventWhenSkipping = true;
            }
        }

        /// <summary>
        /// Stepping next from the cleanup page - which actually launches the work
        /// </summary>
        private void OnStepNextCleanup(object sender, WizardStepEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                bool remove2xInstance = (migrationSource == ConfigurationMigrationSource.InPlace) ? true : radioUpgradeRemove.Checked;

                ShipWorks2xConfigurationMigrator.LaunchMigration(
                    migrationSource == ConfigurationMigrationSource.InPlace ? Application.StartupPath : installPath.Text,
                    remove2xInstance,
                    remove2xInstance && cleanupBox.Checked);
            }
            catch (ConfigurationMigrationException ex)
            {
                MessageHelper.ShowError(this, "An error occurred while updating your configuration:\n\n" + ex.Message);
                e.NextPage = CurrentPage;
            }
        }

    }
}
