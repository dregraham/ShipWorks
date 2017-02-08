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
using Autofac;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Data.Administration.SqlServerSetup
{
    /// <summary>
    /// Wizard page for downloading .NET 3.5 SP1
    /// </summary>
    public partial class DotNet35DownloadPage : WizardPage
    {
        WizardDownloadHelper downloader;
        DotNet35InstallPage installPage;

        /// <summary>
        /// Constructor
        /// </summary>
        public DotNet35DownloadPage(WizardPage pageAfterInstaller, StartupAction startupAction, Func<XElement> startupArgument)
        {
            InitializeComponent();

            installPage = new DotNet35InstallPage(pageAfterInstaller, startupAction, startupArgument);
        }

        /// <summary>
        /// Stepping into the page
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            if (downloader == null)
            {
                downloader = new WizardDownloadHelper(Wizard, DotNet35Installer.RedistributableLocalPath, DotNet35Installer.RedistributableDownloadUri);
                Wizard.Pages.Insert(Wizard.Pages.IndexOf(this) + 1, installPage);
            }

            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                SqlServerInstaller sqlServerInstaller = lifetimeScope.Resolve<SqlServerInstaller>();

                // If the installer is already available, just skip over the download.
                if (sqlServerInstaller.IsDotNet35Sp1Installed || DotNet35Installer.IsRedistributableAvailable)
                {
                    e.Skip = true;

                    // Has to be set, b\c Wizard didn't know about it at the time this function was entered.
                    if (e.StepReason == WizardStepReason.StepForward)
                    {
                        e.SkipToPage = installPage;
                    }
                }
            }

        }

        /// <summary>
        /// Page is being shown
        /// </summary>
        private void OnShown(object sender, WizardPageShownEventArgs e)
        {
            download.Enabled = true;
            progress.Value = 0;
            bytes.Text = "";

            // Cant click next, have to download
            Wizard.NextEnabled = false;
        }

        /// <summary>
        /// Download the redistributable from the interapptive server
        /// </summary>
        private void OnDownload(object sender, EventArgs e)
        {
            downloader.Download(download, progress, bytes);
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
