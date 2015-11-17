using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using ShipWorks.Common.Threading;
using Interapptive.Shared.Data;
using System.Data.SqlClient;
using log4net;
using Interapptive.Shared.UI;
using System.Threading;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Administration.UpdateFrom2x.LegacyCode;
using System.Transactions;
using Interapptive.Shared;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.PostMigration
{
    /// <summary>
    /// Post migration step that encompasses many "final" tasks that don't have UI.
    /// </summary>
    public partial class FinalTasksWizardPage : WizardPage
    {
        static readonly ILog log = LogManager.GetLogger(typeof(FinalTasksWizardPage));

        /// <summary>
        /// Constructor
        /// </summary>
        public FinalTasksWizardPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Stepping in
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            // If it's not in progress, we already must have ran the final cleanup stuff
            if (!MigrationController.IsMigrationInProgress())
            {
                e.Skip = true;
            }
        }

        /// <summary>
        /// Stepping next
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            // Already done
            if (!MigrationController.IsMigrationInProgress())
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            // Bring the upgrading message up and dislabe and the browsing buttons
            panelUpdating.Visible = true;
            panelUpdating.BringToFront();
            Wizard.NextEnabled = false;
            Wizard.BackEnabled = false;
            Wizard.CanCancel = false;

            // Stay on this page
            e.NextPage = this;

            // Create the progress provider and window
            ProgressProvider progressProvider = new ProgressProvider();
            ProgressDlg progressDlg = new ProgressDlg(progressProvider);
            progressDlg.Title = "Finalizing Upgrade";
            progressDlg.Description = "ShipWorks is performing some final steps.";
            progressDlg.Show(this);

            // Used for async invoke
            MethodInvoker<ProgressProvider> invoker = new MethodInvoker<ProgressProvider>(AsyncPerformSteps);

            // Pass along user state
            Dictionary<string, object> userState = new Dictionary<string, object>();
            userState["invoker"] = invoker;
            userState["progressDlg"] = progressDlg;

            // Kick off the async upgrade process
            invoker.BeginInvoke(progressDlg.ProgressProvider, new AsyncCallback(OnAsyncUpdateComplete), userState);
        }

        /// <summary>
        /// Method meant to be called from an asycn invoker to update the database in the background
        /// </summary>
        private void AsyncPerformSteps(ProgressProvider progressProvider)
        {
            var progressEmail = progressProvider.ProgressItems.Add("Import Email History");
            var progressActions = progressProvider.ProgressItems.Add("Prepare Actions");
            var progressFilters = progressProvider.ProgressItems.Add("Prepare Filters");
            var progressDeleteArchives = progressProvider.ProgressItems.Add("Remove Archives");
            var progressCleanup = progressProvider.ProgressItems.Add("Finalize Database");

            EmailLogImporter.ImportEmailHistory(progressEmail);

            ActionConverter.ConvertActions(progressActions);
            FilterConverter.ConvertFilters(progressFilters);

            RemoveArchives(progressDeleteArchives);
            CleanupDatabase(progressCleanup);
        }

        /// <summary>
        /// Drops the linked Archive Databases
        /// </summary>
        private static void RemoveArchives(ProgressItem progress)
        {
            progress.Starting();

            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                List<string> dropCommands = GenerateDropArchivesSqlCommands(con);

                int count = 0;
                foreach (string sqlCommand in dropCommands)
                {
                    progress.Detail = sqlCommand;

                    using (SqlCommand cmd = SqlCommandProvider.Create(con))
                    {
                        cmd.CommandText = sqlCommand;
                        cmd.ExecuteNonQuery();
                    }

                    progress.PercentComplete = (100 * ++count) / dropCommands.Count;
                }
            }

            progress.Detail = "Done";
            progress.PercentComplete = 100;
            progress.Completed();
        }

        /// <summary>
        /// Cleanup all data related to migration to leave the database in a perfectly clean 3x state
        /// </summary>
        private static void CleanupDatabase(ProgressItem progress)
        {
            progress.Starting();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, SqlCommandProvider.DefaultTimeout))
            {
                using (SqlConnection con = SqlSession.Current.OpenConnection())
                {
                    List<string> sqlCommands = GenerateCleanupSqlCommands(con);

                    int count = 0;
                    foreach (string sqlCommand in sqlCommands)
                    {
                        progress.Detail = sqlCommand;

                        using (SqlCommand cmd = SqlCommandProvider.Create(con))
                        {
                            cmd.CommandText = sqlCommand;
                            cmd.ExecuteNonQuery();
                        }

                        progress.PercentComplete = (100 * ++count) / sqlCommands.Count;
                    }
                }

                scope.Complete();
            }

            progress.Detail = "Done";
            progress.PercentComplete = 100;
            progress.Completed();
        }

        /// <summary>
        /// Generates the list of SQL commands that cannot be in a transaction during database cleanup.
        /// </summary>
        private static List<string> GenerateDropArchivesSqlCommands(SqlConnection con)
        {
            List<string> commands = new List<string>();
            
            // need to drop all archive databases
            using (SqlCommand cmd = SqlCommandProvider.Create(con))
            {
                cmd.CommandText = "SELECT DbName FROM ArchiveSets";

                using (SqlDataReader reader = SqlCommandProvider.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        commands.Add(string.Format("IF DB_ID('{0}') IS NOT NULL" +
                                                   "  BEGIN" + 
                                                   "         ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE" + 
                                                   "         DROP DATABASE [{0}]" +
                                                   "  END", reader[0]));
                    }
                }
            }

            return commands;
        }

        /// <summary>
        /// Generate the list of all SQL commands required to get the database into a perfect 3x state
        /// </summary>
        [NDependIgnoreLongMethod]
        private static List<string> GenerateCleanupSqlCommands(SqlConnection con)
        {
            List<string> commands = new List<string>();

            // Need to drop all v2m tables
            using (SqlCommand cmd = SqlCommandProvider.Create(con))
            {
                cmd.CommandText = "SELECT name from sys.tables where name like 'v2m_%' OR name IN ('ArchiveLogs', 'ArchiveSets', 'Users')";

                using (SqlDataReader reader = SqlCommandProvider.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        commands.Add(string.Format("DROP TABLE [{0}]", reader[0]));
                    }
                }
            }

            // Need to drop all v2m procedures
            using (SqlCommand cmd = SqlCommandProvider.Create(con))
            {
                cmd.CommandText = "SELECT name from sys.procedures where name like 'v2m_%'";

                using (SqlDataReader reader = SqlCommandProvider.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        commands.Add(string.Format("DROP PROCEDURE [{0}]", reader[0]));
                    }
                }
            }

            // Drop all v2m functions
            using (SqlCommand cmd = SqlCommandProvider.Create(con))
            {
                cmd.CommandText = "SELECT * FROM sys.objects where (type='TF' or type='FN' or type='IF') AND name LIKE 'v2m_%'";

                using (SqlDataReader reader = SqlCommandProvider.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        commands.Add(string.Format("DROP FUNCTION [{0}]", reader[0]));
                    }
                }
            }

            // Some known indexes.  Check for existence because the tables they're on may have been dropped while getting V3 up-to-date
            commands.Add("IF EXISTS(SELECT object_id FROM sys.indexes WHERE name = 'v2m_IX_Orders') DROP INDEX [v2m_IX_Orders] ON [dbo].[Order]");
            commands.Add("IF EXISTS(SELECT object_id FROM sys.indexes WHERE name = 'v2m_IX_Orders2') DROP INDEX [v2m_IX_Orders2] ON [dbo].[Order]");
            commands.Add("IF EXISTS(SELECT object_id FROM sys.indexes WHERE name = 'v2m_IX_OrderItem') DROP INDEX [v2m_IX_OrderItem] ON [dbo].[OrderItem]");
            commands.Add("IF EXISTS(SELECT object_id FROM sys.indexes WHERE name = 'v2m_IX_EbayOrderItem_OrderID') DROP INDEX [v2m_IX_EbayOrderItem_OrderID] ON [dbo].[EbayOrderItem]");
            commands.Add("IF EXISTS(SELECT object_id FROM sys.indexes WHERE name = 'v2m_IX_Order_CustomerID') DROP INDEX [v2m_IX_Order_CustomerID] ON [dbo].[Order]");
            commands.Add("IF EXISTS(SELECT object_id FROM sys.indexes WHERE name = 'v2m_IX_Order_RollupItemCount') DROP INDEX [v2m_IX_Order_RollupItemCount] ON [dbo].[Order]");
            commands.Add("IF EXISTS(SELECT object_id FROM sys.indexes WHERE name = 'v2m_IX_EbayOrder_RollupItemCount') DROP INDEX [v2m_IX_EbayOrder_RollupItemCount] ON dbo.[EbayOrder]");
            commands.Add("IF EXISTS(SELECT object_id FROM sys.indexes WHERE name = 'v2m_IX_Customer_RollupOrderCount') DROP INDEX [v2m_IX_Customer_RollupOrderCount] ON dbo.[Customer]");
            commands.Add("IF EXISTS(SELECT object_id FROM sys.indexes WHERE name = 'v2m_IX_Note_ObjectID') DROP INDEX [v2m_IX_Note_ObjectID] ON dbo.[Note]");

            // Fix some constraints
            commands.Add("ALTER TABLE [dbo].[FedExPackage] DROP CONSTRAINT [FK_FedExPackage_FedExShipment]");
            commands.Add("ALTER TABLE [dbo].[FedExPackage] ADD CONSTRAINT [FK_FedExPackage_FedExShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[FedExShipment] ([ShipmentID]) ON DELETE CASCADE");
            commands.Add("ALTER TABLE [dbo].[UpsPackage] DROP CONSTRAINT [FK_UpsPackage_UpsShipment]");
            commands.Add("ALTER TABLE [dbo].[UpsPackage] ADD CONSTRAINT [FK_UpsPackage_UpsShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[UpsShipment] ([ShipmentID]) ON DELETE CASCADE");

            // Add back some FK's
            commands.Add("ALTER TABLE dbo.Note ADD CONSTRAINT FK_Note_User FOREIGN KEY (UserID) REFERENCES dbo.[User] (UserID)");

            // Drop indexes so we can alter the columns back to NOT NULL
            commands.Add("DROP INDEX [IX_Auto_RollupOrderCount] ON [dbo].[Customer]");
            commands.Add("DROP INDEX [IX_Auto_RollupItemCount] ON [dbo].[Order]");

            // Rollup columns were changed to allow null so we could track what work needed done
            commands.Add("ALTER TABLE [Order] ALTER COLUMN [RollupItemCount] int NOT NULL");
            commands.Add("ALTER TABLE [EbayOrder] ALTER COLUMN [RollupEbayItemCount] int NOT NULL");
            commands.Add("ALTER TABLE [Customer] ALTER COLUMN [RollupOrderCount] int NOT NULL");

            // Need to add back indexes after altering the above columns
            commands.Add("CREATE NONCLUSTERED INDEX [IX_Auto_RollupOrderCount] ON [dbo].[Customer] ([RollupOrderCount])");
            commands.Add("CREATE NONCLUSTERED INDEX [IX_Auto_RollupItemCount] ON [dbo].[Order] ([RollupItemCount])");

            return commands;
        }

        /// <summary>
        /// The finalization steps have completed
        /// </summary>
        private void OnAsyncUpdateComplete(IAsyncResult result)
        {
            if (InvokeRequired)
            {
                Invoke(new AsyncCallback(OnAsyncUpdateComplete), result);
                return;
            }

            Dictionary<string, object> userState = (Dictionary<string, object>) result.AsyncState;
            MethodInvoker<ProgressProvider> invoker = (MethodInvoker<ProgressProvider>) userState["invoker"];
            ProgressDlg progressDlg = (ProgressDlg) userState["progressDlg"];

            try
            {
                invoker.EndInvoke(result);

                progressDlg.FormClosed += (object sender, FormClosedEventArgs e) => { Wizard.MoveNext(); };
            }
            catch (OperationCanceledException)
            {
                progressDlg.ProgressProvider.Cancel();
                progressDlg.CloseForced();

                panelUpdating.Visible = false;
                Wizard.NextEnabled = true;
                Wizard.BackEnabled = true;
                Wizard.CanCancel = true;
            }
            catch (Exception ex)
            {
                if (ex is SqlScriptException || ex is SqlException || ex is MigrationException)
                {
                    log.ErrorFormat("An error occurred during upgrade.", ex);
                    progressDlg.ProgressProvider.Terminate(ex);

                    MessageHelper.ShowError(progressDlg, string.Format("An error occurred: {0}", ex.Message));
                    progressDlg.CloseForced();

                    panelUpdating.Visible = false;
                    Wizard.NextEnabled = true;
                    Wizard.BackEnabled = true;
                    Wizard.CanCancel = true;
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
