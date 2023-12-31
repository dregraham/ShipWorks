﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Interapptive.Shared;
using Interapptive.Shared.Data;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Common.Threading;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Users.Audit;

namespace ShipWorks.ApplicationCore.Settings.PrintResultCleanup
{
    /// <summary>
    /// Form for selecting the cutoff date to use when purging old labels.
    /// </summary>
    public partial class PrintResultCleanupDlg : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PrintResultCleanupDlg()
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
            if (MessageHelper.ShowQuestion(this, String.Format("Are you sure you want to delete print results older than {0}?", deleteDate.Value.ToShortDateString())) == System.Windows.Forms.DialogResult.OK)
            {
                ProgressProvider progress = new ProgressProvider();
                progress.AddItem("Delete Print Output");
                progress.AddItem("Cleanup Unused Resources");
                progress.AddItem("Reindex and Shrink Database");

                ProgressDlg progressDlg = new ProgressDlg(progress);
                progressDlg.Title = "Delete print output";
                progressDlg.Description = String.Format("Deleting output older than {0}.", deleteDate.Value.ToShortDateString());
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
        [NDependIgnoreLongMethod]
        private void BackgroundPerformDelete(object sender, DoWorkEventArgs e)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();

            IProgressProvider progress = e.Argument as IProgressProvider;
            IProgressReporter progressItem = progress.ProgressItems[0];
            IProgressReporter deOrphanProgressItem = progress.ProgressItems[1];
            IProgressReporter shrinkProgress = progress.ProgressItems[2];

            progressItem.Starting();
            progressItem.Detail = "Finding items to delete ...";

            PrintResultCollection jobsToDelete = FindPrintResultsToDelete(deleteDate.Value);

            int maxCount = jobsToDelete.Count;
            int current = 0;

            // dont' audit any of this, we are tryign to save space
            using (AuditBehaviorScope scope = new AuditBehaviorScope(AuditBehaviorUser.SuperUser, new AuditReason(AuditReasonType.Default), AuditState.Disabled))
            {
                foreach (PrintResultEntity printResult in jobsToDelete)
                {
                    current++;

                    progressItem.Detail = String.Format("Removing reference {0} of {1}...", current, maxCount);
                    // Put the SuperUser in scope, and don't audit
                    using (SqlAdapter adapter = new SqlAdapter(true))
                    {
                        // find all resources used by this print job
                        List<long> references = DataResourceManager.GetConsumerResourceReferenceIDs(printResult.PrintResultID);

                        // unreference them
                        references.ForEach(referenceID => DataResourceManager.ReleaseResourceReference(referenceID));

                        // delete the print result
                        adapter.DeleteEntity(printResult);

                        // OK, commit this row
                        adapter.Commit();
                    }

                    // update progress
                    progressItem.PercentComplete = (100 * current) / maxCount;
                }

                // done deleting references
                progressItem.Completed();

                // start up the orphan deletion
                deOrphanProgressItem.Detail = "Removing orphaned Resources...";
                deOrphanProgressItem.Starting();

                DataResourceManager.DeleteAbandonedResourceData();

                // done
                deOrphanProgressItem.PercentComplete = 100;
                deOrphanProgressItem.Completed();

                shrinkProgress.Starting();

                using (DbConnection con = SqlSession.Current.OpenConnection())
                {
                    using (DbCommand cmd = DbCommandProvider.Create(con))
                    {
                        // this can really take a while
                        cmd.CommandTimeout = (int) TimeSpan.FromMinutes(60).TotalSeconds;

                        shrinkProgress.Detail = "Rebuilding indexes...";
                        cmd.CommandText = String.Format("DBCC DBREINDEX([Resource])");
                        cmd.ExecuteNonQuery();

                        shrinkProgress.PercentComplete = 50;

                        shrinkProgress.Detail = "Shrinking database...";
                        cmd.CommandText = String.Format("DBCC SHRINKDATABASE({0})", SqlSession.Current.Configuration.DatabaseName);
                        cmd.ExecuteNonQuery();
                    }
                }

                shrinkProgress.PercentComplete = 100;
                shrinkProgress.Completed();

                // stop the clock
                timer.Stop();
                e.Result = timer.Elapsed;
            }
        }

        private PrintResultCollection FindPrintResultsToDelete(DateTime deleteDate)
        {
            // just selecting all printresult ids
            ExcludeIncludeFieldsList includefields = new ExcludeIncludeFieldsList(false);
            includefields.Add(PrintResultFields.PrintResultID);

            PrintResultCollection results = new PrintResultCollection();
            using (SqlAdapter adapter = SqlAdapter.Default)
            {
                adapter.FetchEntityCollection(results, new RelationPredicateBucket(PrintResultFields.PrintDate < deleteDate), int.MaxValue, new SortExpression(PrintResultFields.PrintResultID | SortOperator.Ascending));

                return results;
            }
        }

        /// <summary>
        /// Background deletion is complete
        /// </summary>
        private void DeleteComplete(object sender, RunWorkerCompletedEventArgs e)
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
                MessageBox.Show(this, "Print Jobs were successfully deleted. Elapsed Time: " + ((TimeSpan) e.Result));

                DialogResult = DialogResult.OK;
            }

            // cleanup
            BackgroundWorker worker = sender as BackgroundWorker;
            worker?.Dispose();
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
        private void CalculateEstimate(object sender, DoWorkEventArgs e)
        {
            DateTime selectedDate = (DateTime) e.Argument;

            string estimateScript = LoadScript("ShipWorks.ApplicationCore.Settings.PrintResultCleanup.Estimate.sql");

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
        private void CalculateEstimateComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            object[] result = e.Result as object[];

            if (result != null)
            {
                DisplayEstimatedBytesFreed((DateTime) result[1], (long) result[0]);
            }

            // cleanup
            BackgroundWorker worker = sender as BackgroundWorker;
            worker?.Dispose();
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