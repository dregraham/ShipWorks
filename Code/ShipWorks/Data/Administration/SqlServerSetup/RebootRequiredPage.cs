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
using Microsoft.Win32;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Win32;

namespace ShipWorks.Data.Administration.SqlServerSetup
{
    /// <summary>
    /// Wizard page for rebooting after an install.  Does not need to be manually added to the Wizard.  Will be done
    /// by the pages before it.
    /// </summary>
    public partial class RebootRequiredPage : WizardPage
    {
        StartupAction startupAction;
        Func<XElement> startupArgument;

        /// <summary>
        /// Constructor
        /// </summary>
        public RebootRequiredPage(string productName, StartupAction startupAction, Func<XElement> startupArgument)
        {
            InitializeComponent();

            this.startupAction = startupAction;
            this.startupArgument = startupArgument;

            labelSuccess.Text = string.Format(labelSuccess.Text, productName);
            Title = string.Format(Title, productName);
            Description = string.Format(Description, productName);
        }

        /// <summary>
        /// Stepping into the page
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            if (Wizard.Pages.IndexOf(this) != Wizard.Pages.Count - 1)
            {
                throw new InvalidOperationException("The Reboot page needs to be last.");
            }
        }

        /// <summary>
        /// Stepping next
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            // Regardless of what they choose, we'll need rebooted before running again
            StartupController.RequireReboot();
            StartupController.SetStartupAction(startupAction, startupArgument());

            // OK tells the caller of the wizard that setup was fine... it's not fine yet, still pending, so treat as a cancel
            Wizard.DialogResult = DialogResult.Cancel;

            if (radioRebootNow.Checked)
            {
                if (openAfterRestart.Checked)
                {
                    using (RegistryKey runOnce = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\RunOnce"))
                    {
                        runOnce.SetValue("ShipWorks", Application.ExecutablePath);
                    }
                }

                WindowsUtility.ExitWindows(WindowsExitType.Restart, true);
            }

            Application.Exit();
        }

        /// <summary>
        /// The restart now option is changing
        /// </summary>
        private void OnChangeRestartNow(object sender, EventArgs e)
        {
            openAfterRestart.Enabled = radioRebootNow.Checked;
        }
    }
}
