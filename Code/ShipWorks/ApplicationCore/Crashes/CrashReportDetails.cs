using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ShipWorks.ApplicationCore.Crashes
{
    /// <summary>
    /// Window for displaying the details of a crash report that is going to be sent.
    /// </summary>
    public partial class CrashReportDetails : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CrashReportDetails(string details)
        {
            InitializeComponent();

            if (details == null)
            {
                throw new ArgumentNullException("details");
            }

            reportDetails.Text = details.Replace("\n", "\r\n");
        }

        /// <summary>
        /// Save the report to a file.
        /// </summary>
        private void OnSave(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, reportDetails.Text);
                }
                // This is one of the rare cases we catch general Exception.  We
                // do this because we have already crashed.  No need to make it worse
                // by crashing and not being able to report the crash.
                catch (Exception ex)
                {
                    // Use the standard MessageBox for safety
                    MessageBox.Show(this,
                        "An error occurred while saving the file.\n\n" +
                        "Details: " + ex.Message,
                        "ShipWorks",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }
    }
}
