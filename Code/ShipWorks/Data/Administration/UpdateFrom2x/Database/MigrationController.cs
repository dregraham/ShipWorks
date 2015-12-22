using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks;
using Interapptive.Shared.Data;
using ShipWorks.Data.Connection;
using System.Data.SqlClient;
using ShipWorks.Common.Threading;
using System.Data;
using System.Collections;
using log4net;
using System.Threading;
using Interapptive.Shared;
using ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.Specialized;
using ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.PostMigration;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database
{
    /// <summary>
    /// Class that orchestrates and tracks the upgrade process from V2 to V3.
    /// </summary>
    public class MigrationController
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(MigrationController));
						  
        // Current state of the procedure
        MigrationState migrationState = MigrationState.Unknown;

        // current database version
        Version installedDbVersion; 

        // the runtime instantiation of the execution plan
        MigrationExecutionPlan executionPlan;

        // collection of name/value pairs used throughout the migration
        MigrationPropertyBag migrationProperties;

        // the registered task prototypes
        List<MigrationTaskBase> taskPrototypes;

        // pre-execution tasks to check the database and add any
        // MigrationNotices to present to the user before generating 
        // and executing the execution plan.
        List<MigrationTaskBase> inspectionTasks;

        // all of the registered archive databases
        List<ArchiveSet> archiveSets = new List<ArchiveSet>();

        /// <summary>
        /// Default script loader used throughout the process.
        /// </summary>
        MigrationSqlScriptLoader defaultScriptLoader;

        // track initiailization
        bool isInitialized = false;

        /// <summary>
        /// Returns the list of registered Archive Sets in the database
        /// </summary>
        public IList<ArchiveSet> ArchiveSets
        {
            get { return archiveSets.AsReadOnly(); }
        }

        /// <summary>
        /// Returns the default script loader the engine will use
        /// </summary>
        public SqlScriptLoader DefaultScriptLoader
        {
            get { return defaultScriptLoader; }
        }

        /// <summary>
        /// Returns the migration state of the database
        /// </summary>
        public MigrationState MigrationState
        {
            get { return migrationState; }
        }

        /// <summary>
        /// Create a task instance from its TypeCode.  Used during database deserialization
        /// </summary>
        public static MigrationTaskBase CreateTaskInstance(MigrationTaskTypeCode taskCode)
        {
            switch (taskCode)
            {
                case MigrationTaskTypeCode.PrepareDatabaseTask: return new PrepareMainDatabaseMigrationTask();
                case MigrationTaskTypeCode.UpgradeV2MigrationTask: return new UpdateV2MigrationTask();
                case MigrationTaskTypeCode.ScriptMigrationTask: return new ScriptMigrationTask();
                case MigrationTaskTypeCode.CheckConstraintsTask: return new CheckConstraintsMigrationTask();
                case MigrationTaskTypeCode.ConvertStatusCodesTask: return new ConvertStatusCodesMigrationTask();
                case MigrationTaskTypeCode.SslCertificateImportTask: return new SslCertificateImportMigrationTask();
                case MigrationTaskTypeCode.UpdateSchemaVersionTask: return new UpdateSchemaVersionMigrationTask();
                case MigrationTaskTypeCode.LoadStatusPresetsTask: return new LoadStatusPresetsMigrationTask();
                case MigrationTaskTypeCode.UpsNotificationUpgradeTask: return new UpsNotificationUpgradeMigrationTask();
                case MigrationTaskTypeCode.UpgradePaymentDetails: return new UpgradePaymentDetailsMigrationTask();
                default:
                    throw new MigrationException(string.Format("Unknown MigrationTaskCode '{0}'", taskCode));
            }
        }

        /// <summary>
        /// Creates and configure the migration engine for moving data from a V2 database to V3.
        /// </summary>
        [NDependIgnoreLongMethod]
        public static MigrationController CreateV2ToV3Controller(Version installedDBVersion)
        {
            MigrationController controller = new MigrationController(installedDBVersion, "ShipWorks.Data.Administration.UpdateFrom2x.Database.Scripts");

            // populate the engine with the various steps

            // first all databases need to be up-to-date V2 databases
            controller.AddTask(new UpdateV2MigrationTask());

            // Check constraints before we do anything - This will need to be moved to a pre-execution step because user interaction may be required
            controller.AddTask(new CheckConstraintsMigrationTask());
           
            // drop foreign keys in every database
            controller.AddScriptTask("Drop2xForeignKeys", "Drop2xForeignKeys.sql", "Removing constraints...", MigrationTaskInstancing.MainDatabaseAndArchives, MigrationTaskRunPattern.RunOnce);

            // rename v2 tables in every database
            controller.AddScriptTask("Rename2xTables", "Rename2xTables.sql", "Renaming tables...", MigrationTaskInstancing.MainDatabaseAndArchives, MigrationTaskRunPattern.RunOnce);

            // remove triggers
            controller.AddScriptTask("Drop2xTriggers", "Drop2xTriggers.sql", "Dropping triggers...", MigrationTaskInstancing.MainDatabaseAndArchives, MigrationTaskRunPattern.RunOnce);

            // remove v2 sored procedures
            controller.AddScriptTask("Drop2xProcs", "Drop2xProcs.sql", "Removing stored procedures...", MigrationTaskInstancing.MainDatabaseAndArchives, MigrationTaskRunPattern.RunOnce);

            // remove some indexes
            controller.AddScriptTask("Drop2xIndexes", "Drop2xIndexes.sql", "Removing a few Indexes...", MigrationTaskInstancing.MainDatabaseAndArchives, MigrationTaskRunPattern.RunOnce);

            // get everything ready in the various databases
            controller.AddScriptTask("CreateMigrationUtilities", "CreateMigrationUtilities.sql", "Preparing...", MigrationTaskInstancing.MainDatabaseAndArchives, MigrationTaskRunPattern.RunOnce);

            // temporary tables used to shuffle data around
            controller.AddScriptTask("CreateIntermediateTables", "CreateIntermediateTables.sql", "Preparing...", MigrationTaskInstancing.MainDatabaseAndArchives, MigrationTaskRunPattern.RunOnce);

            // clear archve db's archive logs - this only contains information about past un-archive operations which just gets in our way now
            controller.AddScriptTask("ClearArchiveLog", "ClearArchiveLog.sql", "Copying archive log...", MigrationTaskInstancing.ArchiveDatabasesOnly, MigrationTaskRunPattern.RunOnce);

            // move subsets of the archive logs to its repective archive database for faster querying
            controller.AddScriptTask("TransferArchiveLog", "TransferArchiveLog.sql", "Copying archive log...", MigrationTaskInstancing.ArchiveDatabasesOnly, MigrationTaskRunPattern.Repeated);

            // Perform any pre-migration database updates
            controller.AddTask(new PrepareMainDatabaseMigrationTask());

            // generate the 3x schema in the main database
            controller.AddScriptTask("Create30Schema", "Create30Schema.sql", "Creating 3.0 schema...", MigrationTaskInstancing.MainDatabaseOnly, MigrationTaskRunPattern.RunOnce);

            // copy users
            controller.AddScriptTask("MoveUsers", "MigrateData_Users.sql", "Creating users...", MigrationTaskInstancing.MainDatabaseOnly, MigrationTaskRunPattern.RunOnce);

            // Delete 3x indexes so we can copy data faster
            controller.AddScriptTask("Drop3xIndexes", "Drop3xIndexes.sql", "Disabling indexes...", MigrationTaskInstancing.MainDatabaseOnly, MigrationTaskRunPattern.RunOnce);

            // create some indexes too
            controller.AddScriptTask("CreateMigrationIndexes", "CreateMigrationIndexes.sql", "Creating temporary indexes...", MigrationTaskInstancing.MainDatabaseAndArchives, MigrationTaskRunPattern.RunOnce);

            // Disable specific constraints
            controller.AddScriptTask("Disable3xConstraints", "Disable3xConstraints.sql", "Disabling constraints...", MigrationTaskInstancing.MainDatabaseOnly, MigrationTaskRunPattern.RunOnce);

            // move store data
            controller.AddScriptTask("MoveStores", "MigrateData_Stores.sql", "Upgrading stores...", MigrationTaskInstancing.MainDatabaseOnly, MigrationTaskRunPattern.Repeated);

            // convert store client/api certificates
            controller.AddTask(new SslCertificateImportMigrationTask());

            // order/orderitem status presets for the stores
            controller.AddTask(new LoadStatusPresetsMigrationTask());

            // move email accounts
            controller.AddScriptTask("MoveEmailAccounts", "MigrateData_EmailAccounts.sql", "Upgrading email accounts...", MigrationTaskInstancing.MainDatabaseOnly, MigrationTaskRunPattern.Repeated);

            // Yahoo Inventory - this will only be populated in the main SW database. It's never moved during archival
            controller.AddScriptTask("MoveYahooInventory", "MigrateData_YahooInventory.sql", "Upgrading Yahoo! products...", MigrationTaskInstancing.MainDatabaseOnly, MigrationTaskRunPattern.Repeated);

            // Amazon ASIN/Inventory - this will only be populated in the main SW database. It's never moved during archival
            controller.AddScriptTask("MoveAmazonInventory", "MigrateData_AmazonInventory.sql", "Upgrading Amazon products...", MigrationTaskInstancing.MainDatabaseOnly, MigrationTaskRunPattern.Repeated);

            // move Customers
            controller.AddScriptTask("MoveCustomers", "MigrateData_Customers.sql", "Upgrading customers...", MigrationTaskInstancing.MainDatabaseAndArchives, MigrationTaskRunPattern.Repeated);

            // move Order data
            controller.AddScriptTask("MoveOrders", "MigrateData_Orders.sql", "Upgrading orders...", MigrationTaskInstancing.MainDatabaseAndArchives, MigrationTaskRunPattern.Repeated);

            // move Order Items
            controller.AddScriptTask("MoveOrderItems", "MigrateData_OrderItems.sql", "Upgrading order items...", MigrationTaskInstancing.MainDatabaseAndArchives, MigrationTaskRunPattern.Repeated);

            // move Order Items
            controller.AddScriptTask("MoveOrderItemAttributes", "MigrateData_OrderItemAttributes.sql", "Upgrading order items...", MigrationTaskInstancing.MainDatabaseAndArchives, MigrationTaskRunPattern.Repeated);

            // move Order Charges
            controller.AddScriptTask("MoveOrderCharges", "MigrateData_OrderCharges.sql", "Upgrading order charges...", MigrationTaskInstancing.MainDatabaseAndArchives, MigrationTaskRunPattern.Repeated);

            // move Order Payment Details
            controller.AddScriptTask("MoveOrderPaymentDetails", "MigrateData_PaymentDetails.sql", "Upgrading payment details...", MigrationTaskInstancing.MainDatabaseAndArchives, MigrationTaskRunPattern.Repeated);
            controller.AddTask(new UpgradePaymentDetailsMigrationTask());

            // move MivaSebenzaMsgs
            controller.AddScriptTask("MoveMivaSebenzaMessages", "MigrateData_MivaSebenzaMsgs.sql", "Moving Sebenza order messages...", MigrationTaskInstancing.MainDatabaseAndArchives, MigrationTaskRunPattern.Repeated);

            // move Endicia Shippers - we're just going to import address info since V3 is label server
            controller.AddScriptTask("MoveEndiciaShippers", "MigrateData_EndiciaShippers.sql", "Upgrading Endicia shippers...", MigrationTaskInstancing.MainDatabaseOnly, MigrationTaskRunPattern.RunOnce);

            // move Endicia Shippers - we're just going to import address info since V3 is label server
            controller.AddScriptTask("MoveUpsShippers", "MigrateData_UpsShippers.sql", "Upgrading UPS shippers...", MigrationTaskInstancing.MainDatabaseOnly, MigrationTaskRunPattern.RunOnce);

            // FedEx shippers
            controller.AddScriptTask("MoveFedExShippers", "MigrateData_FedexShippers.sql", "Upgarding FedEx shippers...", MigrationTaskInstancing.MainDatabaseOnly, MigrationTaskRunPattern.RunOnce);

            // move shipments
            controller.AddScriptTask("MoveShipments", "MigrateData_Shipments.sql", "Upgrading shipments...", MigrationTaskInstancing.MainDatabaseAndArchives, MigrationTaskRunPattern.Repeated);

            // move shipment commodities
            controller.AddScriptTask("MoveShipmentCommodities", "MigrateData_ShipmentCommodities.sql", "Upgrading shipment commodities...", MigrationTaskInstancing.MainDatabaseAndArchives, MigrationTaskRunPattern.Repeated);

            // Handle some XML serialized, ups notification column
            controller.AddTask(new UpsNotificationUpgradeMigrationTask());

            // fedex Closings
            controller.AddScriptTask("MoveFedexClosings", "MigrateData_FedexClosings.sql", "Upgrading FedEx End of Day Closings...", MigrationTaskInstancing.MainDatabaseOnly, MigrationTaskRunPattern.Repeated);
            
            // update order table rollups
            controller.AddScriptTask("UpdateOrderRollups", "UpdateOrderRollups.sql", "Updating order counts...", MigrationTaskInstancing.MainDatabaseOnly, MigrationTaskRunPattern.Repeated);

            // update eBayOrder table rollups
            controller.AddScriptTask("UpdateeBayOrderRollups", "UpdateeBayOrderRollups.sql", "Updating eBay counts...", MigrationTaskInstancing.MainDatabaseOnly, MigrationTaskRunPattern.Repeated);

            // update customer table rollups
            controller.AddScriptTask("UpdateCustomerRollups", "UpdateCustomerRollups.sql", "Updating customer counts...", MigrationTaskInstancing.MainDatabaseOnly, MigrationTaskRunPattern.Repeated);

            // create default shipping profiles for the carriers
            controller.AddScriptTask("CreateShippingProfiles", "CreateShippingProfiles.sql", "Generating shipping profiles...", MigrationTaskInstancing.MainDatabaseOnly, MigrationTaskRunPattern.RunOnce);

            // update the Store table to change cart types of unsupported V3 carts
            controller.AddScriptTask("UpdateUnsupportedCarts", "UpdateUnsupportedCarts.sql", "Updating stores...", MigrationTaskInstancing.MainDatabaseOnly, MigrationTaskRunPattern.RunOnce);

            // update status code XML blobs, and translate OnlineStatusCode to OnlineStatus display text on stores with Xml blobs for status codes
            controller.AddTask(new ConvertStatusCodesMigrationTask());

            // Enable specific constraints
            controller.AddScriptTask("Enable3xConstraints", "Enable3xConstraints.sql", "Enabling constraints...", MigrationTaskInstancing.MainDatabaseOnly, MigrationTaskRunPattern.RunOnce);

            // create the 3x indexes
            controller.AddScriptTask("Create3xIndexes", "Create3xIndexes.sql", "Creating indexes...", MigrationTaskInstancing.MainDatabaseOnly, MigrationTaskRunPattern.Repeated);          

            // Update the database to be marked with the correct db version
            controller.AddTask(new UpdateSchemaVersionMigrationTask());

            return controller;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private MigrationController(Version installedDbVersion, string scriptLoaderBase)
        {
            migrationProperties = new MigrationPropertyBag();
            migrationProperties["MasterDatabase"] = SqlSession.Current.Configuration.DatabaseName;

            taskPrototypes = new List<MigrationTaskBase>();
            inspectionTasks = new List<MigrationTaskBase>();

            this.installedDbVersion = installedDbVersion;

            defaultScriptLoader = new MigrationSqlScriptLoader(scriptLoaderBase);

            DetectMigrationState();
        }

        /// <summary>
        /// Checks the current state of the migration
        /// </summary>
        private void DetectMigrationState()
        {
            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                // make sure we are set to go
                EnsureRuntimeTablesExist(con);

                // is our migration plan table empty?
                int rowCount = (int) SqlCommandProvider.ExecuteScalar(con, "SELECT COUNT(*) FROM v2m_MigrationPlan");
                if (rowCount == 0)
                {
                    migrationState = MigrationState.NotStarted;
                }
                else
                {
                    // rows in the execution plan table indicates a failed run.  
                    // at this point, assuming we'll be able to resum.  This gets 
                    // validated at a later point
                    migrationState = MigrationState.ResumeRequired;
                }
            }
        }

        /// <summary>
        /// Makes sure the migration tables exist
        /// </summary>
        private void EnsureRuntimeTablesExist(SqlConnection con)
        {
            // execute the script to verify the tables exist, and create them if they don't
            DefaultScriptLoader["CreateMigrationTables"].Execute(con);
        }

        /// <summary>
        /// Initialize the execution engine once all tasks have been added
        /// </summary>
        public void Initialize()
        {
            // find all of the involved archive sets
            DiscoverArchiveSets();

            // create the execution plan
            executionPlan = new MigrationExecutionPlan(this, taskPrototypes);
            executionPlan.Initialize();

            isInitialized = true;
        }

        /// <summary>
        /// Determines if the migration is resumable from prior failed migration
        /// </summary>
        public bool CanResume()
        {
            EnsureInitialization();

            return executionPlan.Resumable;
        }

        /// <summary>
        /// Discovers any registered Archive Sets and ensures connectivity
        /// </summary>
        private void DiscoverArchiveSets()
        {
            log.Info("Discovering Archive Sets");

            // clear out any previously discovered archive sets
            archiveSets.Clear();

            // query the main database for registered ArchiveSets
            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                using (SqlCommand command = SqlCommandProvider.Create(con))
                {
                    command.CommandText = "IF (OBJECT_ID('ArchiveSets') IS NOT NULL) SELECT ArchiveSetName, DbName FROM ArchiveSets";
                    command.CommandType = CommandType.Text;

                    using (SqlDataReader reader = SqlCommandProvider.ExecuteReader(command))
                    {
                        // read all of the sets
                        while (reader.Read())
                        {
                            archiveSets.Add(new ArchiveSet { ArchiveSetName = (string)reader["ArchiveSetName"], DBName = (string)reader["DbName"] });
                        }
                    }
                }

                // attempt to connect to each archive set.
                archiveSets.ForEach(s => s.TestConnectivity(con));

                // now check for failures and raise an exception
                string message = "";
                archiveSets.ForEach(s =>
                {
                    if (!s.CanConnect)
                    {
                        if (message.Length > 0)
                        {
                            message += "," + Environment.NewLine;
                        }

                        message += String.Format("{0} in database {1}", s.ArchiveSetName, s.DBName);
                    }
                });

                // if there's a message, raise the exception
                if (message.Length > 0)
                {
                    message = String.Format("Unable to connect to the following Archive Sets:{0}{1}", Environment.NewLine, message);
                    throw new MigrationException(message);
                }
            }
        }

        /// <summary>
        /// Begin the execution
        /// </summary>
        public bool Execute(ProgressProvider progress)
        {
            EnsureInitialization();

            using (MigrationContext context = new MigrationContext(migrationProperties))
            {
                // create the progress items
                ProgressItem estimateProgressItem = progress.ProgressItems.Add("Prepare for Upgrade");
                estimateProgressItem.CanCancel = false;                

                ProgressItem executeProgressItem = progress.ProgressItems.Add("Upgrade Data");

                // first calculate execution estimates
                context.ProgressItem = estimateProgressItem;
                context.ExecutionPhase = MigrationTaskExecutionPhase.Estimate;
                executionPlan.CalculateEstimates();
                
                // kick off the actual work
                context.ExecutionPhase = MigrationTaskExecutionPhase.Execute;
                context.ProgressItem = executeProgressItem;
                executionPlan.Execute();

                if (progress.CancelRequested)
                {
                    // done with a cancellation
                    migrationState = Database.MigrationState.ResumeRequired;
                    return false;
                }

            }

            // done with main execution
            migrationState = Database.MigrationState.MainExecutionComplete;

            // all done, no cancellation
            return true;
        }

        /// <summary>
        /// Checks to make sure the engine has been initialized.  Don't want
        /// to just do it on demand because it could take a while and should
        /// be explicit.
        /// </summary>
        private void EnsureInitialization()
        {
            if (!isInitialized)
            {
                throw new InvalidOperationException("MigrationController must be initialized first.");
            }
        }

        /// <summary>
        /// Indicates if there is any pending migration work that still needs done.  This could be actual data migration from 2x to 3x schema, or post 3x schema work.
        /// </summary>
        public static bool IsMigrationInProgress()
        {
            return ExistingConnectionScope.ExecuteWithCommand(cmd =>
            {
                cmd.CommandText = @"SELECT COALESCE(OBJECT_ID('dbo.v2m_MigrationPlan'), 0)";

                return (int)SqlCommandProvider.ExecuteScalar(cmd) > 0;
            });
        }

        #region Task Management

        /// <summary>
        /// Adds a migration task to a specified engine task collection.
        /// </summary>
        private void AddTaskToCollection(MigrationTaskBase task, List<MigrationTaskBase> collection)
        {
            // safety
            if (isInitialized)
            {
                throw new InvalidOperationException("Cannot add migration tasks to an initialized MigrationController");
            }

            // default to our script loader
            if (task.ScriptLoader == null)
            {
                task.ScriptLoader = defaultScriptLoader;
            }

            // make sure no duplicate Ids exist
            if (collection.Any(t => t.Identifier == task.Identifier))
            {
                throw new InvalidOperationException(String.Format("A task with identifier '{0}' has already been added to the migration engine.", task.Identifier));
            }

            // add to the collection of tasks
            collection.Add(task);
        }

        /// <summary>
        /// Adds a migration task to the engine.
        /// 
        /// Cannot be called once the engine has been initialized and the
        /// execution plan generated.
        /// </summary>
        public MigrationTaskBase AddTask(MigrationTaskBase task)
        {
            // add the task to the prototypes collection
            AddTaskToCollection(task, taskPrototypes);

            return task;
        }

        /// <summary>
        /// Adds an inspection task to the engine.
        /// 
        /// Cannot be called once the engine has been initialized and 
        /// the execution plan generated.
        /// </summary>
        public MigrationTaskBase AddInspectionTask(MigrationTaskBase task)
        {
            // add the task to the inspections collection
            AddTaskToCollection(task, inspectionTasks);

            return task;
        }

        /// <summary>
        /// Convenience method for creating a ScriptMigrationTask and adding it to the
        /// engine.
        /// </summary>
        public MigrationTaskBase AddScriptTask(string identifier, string scriptResource, string progressDetail, MigrationTaskInstancing instancing, MigrationTaskRunPattern runPattern)
        {
            return AddTask(new ScriptMigrationTask(identifier, scriptResource, instancing, runPattern, progressDetail));
        }

        /// <summary>
        /// Adds a script task to be run once per database migration, with a RunPattern of RunOnce.
        /// </summary>
        public MigrationTaskBase AddScriptTask(string identifier, string scriptResource, string progressDetail)
        {
            return AddScriptTask(identifier, scriptResource, progressDetail, MigrationTaskInstancing.MainDatabaseOnly, MigrationTaskRunPattern.RunOnce);
        }

       

        #endregion
    }
}
