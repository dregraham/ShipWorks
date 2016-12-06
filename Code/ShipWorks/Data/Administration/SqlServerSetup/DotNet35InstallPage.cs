using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using ShipWorks.ApplicationCore.Interaction;
using System.Xml.Linq;
using Interapptive.Shared.UI;

namespace ShipWorks.Data.Administration.SqlServerSetup
{
    /// <summary>
    /// WizardPage for installing .net 3.5 SP1
    /// </summary>
    public partial class DotNet35InstallPage : WizardPage
    {
        bool dotNet35Installed = false;

        WizardPage pageAfterInstaller;
        RebootRequiredPage rebootPage = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public DotNet35InstallPage(WizardPage pageAfterInstaller, StartupAction startupAction, Func<XElement> startupArgument)
        {
            InitializeComponent();

            this.pageAfterInstaller = pageAfterInstaller;
            this.rebootPage = new RebootRequiredPage(".NET Framework SQL Components", startupAction, startupArgument);
        }

        /// <summary>
        /// Stepping into the page
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            // If its installed now, we are ok to move on.
            if (SqlServerInstaller.IsDotNet35Sp1Installed)
            {
                e.Skip = true;
                e.RaiseStepEventWhenSkipping = true;
            }
        }

        /// <summary>
        /// Install .NET 3.5 SP1
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            // If it installed and requires a reboot, show the reboot page
            if (dotNet35Installed && Wizard.Pages.Contains(rebootPage))
            {
                e.NextPage = rebootPage;
                return;
            }

            // If its installed now, we are ok to move on.
            if (SqlServerInstaller.IsDotNet35Sp1Installed || dotNet35Installed)
            {
                e.NextPage = pageAfterInstaller;
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            // Bring the installing message up and disable and the browsing buttons
            panelInstallingDotNet35.BringToFront();
            Wizard.NextEnabled = false;
            Wizard.BackEnabled = false;

            // Stay on this page
            e.NextPage = this;

            try
            {
                DotNet35Installer installer = new DotNet35Installer();
                installer.Exited += new EventHandler(OnInstallerExited);
                installer.InstallDotNet35();
            }
            catch (Win32Exception ex)
            {
                MessageHelper.ShowError(this, "An error occurred during installation:\n\n" + ex.Message);

                // Reset the gui
                panelInstallDotNet35.BringToFront();
                Wizard.NextEnabled = true;
                Wizard.BackEnabled = true;
            }
        }

        /// <summary>
        /// The Windows installer installation has completed.
        /// </summary>
        private void OnInstallerExited(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new EventHandler(OnInstallerExited), new object[] { sender, e });
                return;
            }

            DotNet35Installer installer = (DotNet35Installer) sender;

            // If it was successful, we should now be able to connect.
            if (SqlServerInstaller.IsDotNet35Sp1Installed || installer.LastExitCode == 0 || installer.LastExitCode == 3010 || installer.LastExitCode == 1614 )
            {
                dotNet35Installed = true;

                // Add the reboot page as the last page, so it has the finish button.
                if (installer.LastExitCode > 0)
                {
                    Wizard.Pages.Add(rebootPage);
                }

                Wizard.MoveNext();
            }
            else
            {
                MessageHelper.ShowError(this,
                    "The .NET Framework SQL Components were not installed.\n\n" + DotNet35Installer.FormatReturnCode(installer.LastExitCode));

                // Reset the gui
                panelInstallDotNet35.BringToFront();
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
                MessageHelper.ShowMessage(this, "Please wait for the installation to complete.");
                e.Cancel = true;
            }
        }
    }
}
