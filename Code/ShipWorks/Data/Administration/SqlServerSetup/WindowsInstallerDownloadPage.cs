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

namespace ShipWorks.Data.Administration.SqlServerSetup
{
    /// <summary>
    /// Wizard page for downloading windows installer
    /// </summary>
    public partial class WindowsInstallerDownloadPage : WizardPage
    {
        WizardDownloadHelper downloader;
        WindowsInstallerInstallPage installPage;

        /// <summary>
        /// Constructor
        /// </summary>
        public WindowsInstallerDownloadPage(WizardPage pageAfterInstaller, StartupAction startupAction, Func<XElement> startupArgument)
        {
            InitializeComponent();

            installPage = new WindowsInstallerInstallPage(pageAfterInstaller, startupAction, startupArgument);
        }

        /// <summary>
        /// Stepping into the page
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            if (downloader == null)
            {
                downloader = new WizardDownloadHelper(Wizard, WindowsInstallerInstaller.RedistributableLocalPath, WindowsInstallerInstaller.RedistributableDownloadUri);
                Wizard.Pages.Insert(Wizard.Pages.IndexOf(this) + 1, installPage);
            }

            // If the installer is already available, just skip over the download.
            if (!SqlServerInstaller.IsWindowsInstallerRequired || WindowsInstallerInstaller.IsRedistributableAvailable)
            {
                e.Skip = true;

                // Has to be set, b\c Wizard didn't know about it at the time this function was entered.
                if (e.StepReason == WizardStepReason.StepForward)
                {
                    e.SkipToPage = installPage;
                }
            }
        }

        /// <summary>
        /// Page is being shown
        /// </summary>
        private void OnShown(object sender, WizardPageShownEventArgs e)
        {
            downloadWindowsInstaller.Enabled = true;
            progressWindowsInstaller.Value = 0;
            bytesWindowsInstaller.Text = "";

            // Cant click next, have to download
            Wizard.NextEnabled = false;
        }

        /// <summary>
        /// Download the windows installer redistributable from the interapptive server
        /// </summary>
        private void OnDownload(object sender, EventArgs e)
        {
            downloader.Download(downloadWindowsInstaller, progressWindowsInstaller, bytesWindowsInstaller);
        }

        /// <summary>
        /// Cancel  has been clicked
        /// </summary>
        private void OnCancelWindows(object sender, CancelEventArgs e)
        {
            downloader.OnCancelDownload(sender, e);
        }
    }
}
