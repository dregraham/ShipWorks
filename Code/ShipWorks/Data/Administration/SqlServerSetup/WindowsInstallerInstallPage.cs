using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using ShipWorks.UI;
using ShipWorks.ApplicationCore.Interaction;
using System.Xml.Linq;
using Interapptive.Shared.UI;

namespace ShipWorks.Data.Administration.SqlServerSetup
{
    /// <summary>
    /// Wizard page for installing windows installer.  This page should not be added directly to the wizard.  It will be added automatically
    /// by the WindowsInstallerDownloadPage.
    /// </summary>
    public partial class WindowsInstallerInstallPage : WizardPage
    {
        bool windowsInstallerInstalled = false;

        WizardPage pageAfterInstaller;
        RebootRequiredPage rebootPage = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public WindowsInstallerInstallPage(WizardPage pageAfterInstaller, StartupAction startupAction, Func<XElement> startupArgument)
        {
            InitializeComponent();

            this.pageAfterInstaller = pageAfterInstaller;
            this.rebootPage = new RebootRequiredPage("Windows Installer 4.5", startupAction, startupArgument);
        }

        /// <summary>
        /// Stepping into the page
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            // If its installed now, we are ok to move on.
            if (!SqlServerInstaller.IsWindowsInstallerRequired)
            {
                e.Skip = true;
                e.RaiseStepEventWhenSkipping = true;
            }
        }

        /// <summary>
        /// Install Windows Installer
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            if (windowsInstallerInstalled)
            {
                e.NextPage = rebootPage;
                return;
            }

            // If its installed now, we are ok to move on.
            if (!SqlServerInstaller.IsWindowsInstallerRequired)
            {
                e.NextPage = pageAfterInstaller;
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            // Bring the installing message up and dislabe and the browsing buttons
            panelInstallingWindowsInstaller.BringToFront();
            Wizard.NextEnabled = false;
            Wizard.BackEnabled = false;

            // Stay on this page
            e.NextPage = this;

            try
            {
                WindowsInstallerInstaller installer = new WindowsInstallerInstaller();
                installer.Exited += new EventHandler(OnWindowsInstallerExited);
                installer.InstallWindowsInstaller();
            }
            catch (Win32Exception ex)
            {
                MessageHelper.ShowError(this, "An error occurred while installing Windows Installer:\n\n" + ex.Message);

                // Reset the gui
                panelInstallWindowsInstaller.BringToFront();
                Wizard.NextEnabled = true;
                Wizard.BackEnabled = true;
            }
        }

        /// <summary>
        /// The Windows installer installation has completed.
        /// </summary>
        private void OnWindowsInstallerExited(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new EventHandler(OnWindowsInstallerExited), new object[] { sender, e });
                return;
            }

            WindowsInstallerInstaller installer = (WindowsInstallerInstaller) sender;

            // If it was successful, we should now be able to connect.
            if (!SqlServerInstaller.IsWindowsInstallerRequired || installer.LastExitCode == 0 || installer.LastExitCode == 3010)
            {
                windowsInstallerInstalled = true;

                // Add the reboot page as the last page, so it has the finish button.
                Wizard.Pages.Add(rebootPage);

                Wizard.MoveNext();
            }
            else
            {
                MessageHelper.ShowError(this,
                    "Windows Installer 4.5 was not installed.\n\n" + WindowsInstallerInstaller.FormatReturnCode(installer.LastExitCode));

                // Reset the gui
                panelInstallWindowsInstaller.BringToFront();
                Wizard.NextEnabled = true;
                Wizard.BackEnabled = true;
            }
        }

        /// <summary>
        /// Cancell the installation of windows installer
        /// </summary>
        private void OnCancel(object sender, CancelEventArgs e)
        {
            if (!Wizard.NextEnabled)
            {
                MessageHelper.ShowMessage(this, "Please wait for the installation of Windows Installer to complete.");
                e.Cancel = true;
            }
        }
    }
}
