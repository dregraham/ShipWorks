using System;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Interapptive.Shared.Data;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Connection;

namespace ShipWorks.ApplicationCore.Options.ResourceCleanup
{
    /// <summary>
    /// Form for selecting the cutoff date to use when purging old labels.
    /// </summary>
    public partial class ResourceCleanupDlg : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ResourceCleanupDlg()
        {
            InitializeComponent();

            deleteDate.Value = DateTime.Now.Subtract(TimeSpan.FromDays(60));
        }

        /// <summary>
        /// Loads a sql script
        /// </summary>
        private static string LoadScript(string resourceName)
        {
            string contents = "";

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    contents = reader.ReadToEnd();
                }
            }

            return contents;
        }

        #region Deletes

        /// <summary>
        /// Delete clicked
        /// </summary>
        private void OnDeleteClick(object sender, EventArgs e)
        {
            if (MessageHelper.ShowQuestion(this, String.Format("Are you sure you want to delete labels older than {0}?", deleteDate.Value.ToShortDateString())) == System.Windows.Forms.DialogResult.OK)
            {
                ProgressProvider progress = new ProgressProvider();
                progress.AddItem("Delete Labels");

                ProgressDlg progressDlg = new ProgressDlg(progress);
                progressDlg.Title = "Delete old labels";
                progressDlg.Description = String.Format("Deleting labels older than {0}.", deleteDate.Value.ToShortDateString());
                progressDlg.BehaveAsModal = true;

                progressDlg.Show(this);

                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += new DoWorkEventHandler(BackgroundPerformDelete);
                worker.RunWorkerAsync(progress);
                worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(DeleteComplete);
            }
        }

        /// <summary>
        /// Perform the deletion
        /// </summary>
        void BackgroundPerformDelete(object sender, DoWorkEventArgs e)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();

            IProgressProvider progress = e.Argument as IProgressProvider;
            IProgressReporter progressItem = progress.ProgressItems[0];

            progressItem.Starting();
            progressItem.Detail = "Loading script...";
            string script = LoadScript("ShipWorks.ApplicationCore.Options.ResourceCleanup.ResourceCleanup.sql");

            script = script.Replace("{CUTOFFDATE}", deleteDate.Value.ToShortDateString());
            script = script.Replace("{DATABASENAME}", SqlSession.Current.Configuration.DatabaseName);

            progressItem.Detail = "Connecting...";
            using (DbConnection con = SqlSession.Current.OpenConnection())
            {
                DbCommandProvider.ExecuteNonQuery(con, "SET XACT_ABORT ON");

                using (DbCommand cmd = DbCommandProvider.Create(con))
                {
                    cmd.CommandText = script;

                    // this does alot of thrashing, so give it more than enough time
                    progressItem.Detail = "Deleting labels...";
                    cmd.CommandTimeout = (int) TimeSpan.FromHours(1).TotalSeconds;

                    DbCommandProvider.ExecuteNonQuery(cmd);
                }
            }

            timer.Stop();
            e.Result = timer.Elapsed;

            progressItem.PercentComplete = 100;
            progressItem.Completed();
        }

        /// <summary>
        /// Background deletion is complete
        /// </summary>
        void DeleteComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (IsDisposed)
            {
                // form went away while we were working
                return;
            }

            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker) delegate { DeleteComplete(sender, e); });
                return;
            }

            if (e.Error != null)
            {
                MessageHelper.ShowError(this, "An error occurred while deleting: " + e.Error.Message);
                DialogResult = DialogResult.Cancel;
            }
            else
            {
                MessageBox.Show(this, "Labels were successfully deleted. Elapsed Time: " + ((TimeSpan) e.Result));

                DialogResult = DialogResult.OK;
            }

            // cleanup
            BackgroundWorker worker = sender as BackgroundWorker;
            worker.Dispose();
        }

        #endregion

        #region Estimate

        /// <summary>
        /// Update the status label with number of bytes estimated
        /// </summary>
        private void DisplayEstimatedBytesFreed(DateTime cutoffDate, long byteCount)
        {
            if (IsDisposed)
            {
                return;
            }

            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker) delegate { DisplayEstimatedBytesFreed(cutoffDate, byteCount); });
                return;
            }

            estimateLink.Enabled = true;
            statusPicture.Visible = false;
            okButton.Enabled = true;
            cancelButton.Enabled = true;

            string unit = "bytes";
            decimal result = byteCount;
            if (byteCount > Math.Pow(2, 30))
            {
                unit = "GB";
                result = (decimal) Math.Round(byteCount / Math.Pow(2, 30), 2);
            }
            else if (byteCount > Math.Pow(2, 20))
            {
                unit = "MB";
                result = (decimal) Math.Round(byteCount / Math.Pow(2, 20), 2);
            }
            else if (byteCount > Math.Pow(2, 10))
            {
                unit = "KB";
                result = (decimal) Math.Round(byteCount / Math.Pow(2, 10), 2);
            }

            statusText.Text = String.Format("Deleting older than {0} should free up {1} {2}.", cutoffDate.ToShortDateString(), result, unit);
        }

        /// <summary>
        /// Start the calculation
        /// </summary>
        void CalculateEstimate(object sender, DoWorkEventArgs e)
        {
            DateTime selectedDate = (DateTime) e.Argument;

            string estimateScript = LoadScript("ShipWorks.ApplicationCore.Options.ResourceCleanup.Estimate.sql");

            estimateScript = estimateScript.Replace("{CUTOFFDATE}", selectedDate.ToShortDateString());

            using (DbConnection con = SqlSession.Current.OpenConnection())
            {
                using (DbCommand cmd = DbCommandProvider.Create(con))
                {
                    cmd.CommandTimeout = (int) TimeSpan.FromMinutes(5).TotalSeconds;
                    cmd.CommandText = estimateScript;

                    object result = DbCommandProvider.ExecuteScalar(cmd);
                    long byteCount = 0;
                    if (result != null && result != DBNull.Value)
                    {
                        byteCount = (long) result;
                    }

                    e.Result = new object[] { byteCount, selectedDate };
                }
            }
        }

        /// <summary>
        /// Calculating the estimate is complete
        /// </summary>
        void CalculateEstimateComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            object[] result = e.Result as object[];

            DisplayEstimatedBytesFreed((DateTime) result[1], (long) result[0]);

            // cleanup
            BackgroundWorker worker = sender as BackgroundWorker;
            worker.Dispose();
        }

        /// <summary>
        /// Estimate link was clicked
        /// </summary>
        private void OnEstimate(object sender, MouseEventArgs e)
        {
            estimateLink.Enabled = false;
            statusPicture.Visible = true;
            statusText.Visible = true;
            okButton.Enabled = false;
            cancelButton.Enabled = false;

            StartEstimation();
        }

        /// <summary>
        /// Begin the calcuations
        /// </summary>
        private void StartEstimation()
        {
            BackgroundWorker calcWorker = new BackgroundWorker();
            calcWorker.WorkerReportsProgress = false;
            calcWorker.DoWork += new DoWorkEventHandler(CalculateEstimate);
            calcWorker.RunWorkerAsync(deleteDate.Value);
            calcWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(CalculateEstimateComplete);
        }

        #endregion
    }
}