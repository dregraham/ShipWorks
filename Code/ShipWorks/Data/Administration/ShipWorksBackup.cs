using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Interapptive.Shared;
using Interapptive.Shared.Data;
using Interapptive.Shared.IO.Zip;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Users;
using ShipWorks.Users.Audit;
using ShipWorks.Users.Security;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Performs backup and restore of ShipWorks
    /// </summary>
    class ShipWorksBackup
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(ShipWorksBackup));

        // File the backup will be written to \ restored from
        readonly IProgressProvider progress;

        // The user that is doing the backup
        readonly long userID = -1;

        // The SqlSession that holds the connection information for the database to backup\restore
        readonly SqlSession sqlSession;

        // Static (global) event that can notify any time a restore is starting.
        public static event EventHandler RestoreStarting;

        /// <summary>
        /// Internal structure representing a database to be backed up
        /// </summary>
        class BackupDatabase
        {
            public string DatabaseName { get; set; }
            public string BackupFile { get; set; }
            public ProgressItem Progress { get; set; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipWorksBackup(SqlSession sqlSession, UserEntity user, IProgressProvider progress)
        {
            this.sqlSession = sqlSession;
            this.progress = progress;
            this.userID = user.UserID;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipWorksBackup(SqlSession sqlSession, long userID, IProgressProvider progress)
        {
            this.sqlSession = sqlSession;
            this.progress = progress;
            this.userID = userID;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipWorksBackup(SqlSession sqlSession, UserEntity user)
            : this(sqlSession, user, null)
        {

        }

        /// <summary>
        /// Indicates if the current operation has been cancelled.
        /// </summary>
        private bool IsCancelled
        {
            get
            {
                return progress != null ? progress.CancelRequested : false;
            }
        }

        #region Backup

        /// <summary>
        /// Create a backup
        /// </summary>
        [NDependIgnoreLongMethod]
        public void CreateBackup(string filename)
        {
            if (filename == null)
            {
                throw new ArgumentNullException("filename");
            }

            using (SqlSessionScope scope = new SqlSessionScope(sqlSession))
            {
                if (!SqlSession.Current.IsLocalServer())
                {
                    throw new InvalidOperationException("A ShipWorks backup can only be made on the computer that is running SQL Server.");
                }

                EnsureUserPermission(PermissionType.DatabaseBackup);

                log.Info("Starting backup");

                // Make sure we can create and write to the file
                using (StreamWriter writer = File.CreateText(filename))
                {

                }

                // Create a temporary working directory
                string tempPath = DataPath.CreateUniqueTempPath();

                try
                {
                    // We need to make sure SQL Server can write to it
                    ApplyFolderPermissions(tempPath);

                    List<BackupDatabase> databases = GetDatabasesToBackup(tempPath);

                    // Create the progress items that occur
                    ProgressItem compressProgress = new ProgressItem("Compress Backup");

                    if (progress != null)
                    {
                        foreach (BackupDatabase database in databases)
                        {
                            progress.ProgressItems.Add(database.Progress);
                        }

                        progress.ProgressItems.Add(compressProgress);
                    }

                    SqlServerEditionIdType sqlServerEditionId = SqlServerEditionIdType.Express;
                    // Create the backups
                    foreach (BackupDatabase database in databases)
                    {
                        sqlServerEditionId = CreateSqlBackup(database.Progress, database.DatabaseName, database.BackupFile);
                    }

                    if (!IsCancelled)
                    {
                        // Create the archive to be zipped
                        ZipWriter zipWriter = new ZipWriter();

                        // If the edition supports SQL backup compression we will only store the backup in the zip file.  If
                        // SQL backup compression is not supported, we need to compression it.
                        zipWriter.CompressionLevel = sqlServerEditionId.SupportsCompression() ? 0 : 1;

                        zipWriter.Items.Add(new ZipWriterFileItem(CreateBackupInfoFile(tempPath), "backup.xml"));

                        // Add in each backup.  Could be more than one if this was a 2x database with archiving
                        foreach (BackupDatabase database in databases)
                        {
                            zipWriter.Items.Add(new ZipWriterFileItem(database.BackupFile, string.Format(@"Database\{0}", Path.GetFileName(database.BackupFile))));
                        }

                        CreateBackupZip(compressProgress, zipWriter, filename);
                    }

                    // If the cancelled, we have to get rid of the file
                    if (IsCancelled)
                    {
                        CleanupFile(filename);
                    }
                    else
                    {
                        bool audited = false;

                        try
                        {
                            if (SqlSchemaUpdater.IsCorrectSchemaVersion())
                            {
                                AuditUtility.Audit(AuditActionType.BackupDatabase, userID);
                                audited = true;
                            }
                        }
                        catch (InvalidShipWorksDatabaseException)
                        {

                        }

                        if (!audited)
                        {
                            log.Warn("Not auditing backup due to incorrect DB version.");
                        }
                    }
                }
                catch
                {
                    CleanupFile(filename);

                    throw;
                }
                finally
                {
                    try
                    {
                        Directory.Delete(tempPath, true);
                    }
                    catch (IOException ex)
                    {
                        log.Warn("Failed to clean up temporary backup location.", ex);
                    }
                }
            }
        }

        /// <summary>
        /// Get the list of database objects to be backed up.  Will only be more than one if the connected SqlSesssion is a v2 database with archives.
        /// </summary>
        private List<BackupDatabase> GetDatabasesToBackup(string tempPath)
        {
            List<BackupDatabase> databases = new List<BackupDatabase>();

            BackupDatabase primary = new BackupDatabase
            {
                DatabaseName = SqlSession.Current.Configuration.DatabaseName,
                BackupFile = Path.Combine(tempPath, "shipworks.dat"),
                Progress = new ProgressItem("Create SQL Server Backup"),
            };

            // Always have the primary database
            databases.Add(primary);

            return databases;
        }

        /// <summary>
        /// Create the xml file that contains the metadata about the backup
        /// </summary>
        private string CreateBackupInfoFile(string tempPath)
        {
            // Generate the info file that goes in
            string infoPath = Path.Combine(tempPath, "backup.xml");

            XmlTextWriter xmlWriter = new XmlTextWriter(infoPath, Encoding.UTF8);

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Backup");

            xmlWriter.WriteElementString("Date", DateTime.UtcNow.ToString("u"));

            xmlWriter.WriteStartElement("SqlServer");
            xmlWriter.WriteElementString("Instance", SqlSession.Current.Configuration.ServerInstance);
            xmlWriter.WriteElementString("Database", SqlSession.Current.Configuration.DatabaseName);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("ShipWorks");
            xmlWriter.WriteElementString("AppVersion", Assembly.GetExecutingAssembly().GetName().Version.ToString());
            xmlWriter.WriteElementString("SchemaVersion", SqlSchemaUpdater.GetInstalledSchemaVersion().ToString());
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();

            xmlWriter.Close();

            return infoPath;
        }

        /// <summary>
        /// Cleanup the given file after a failed backup
        /// </summary>
        private static void CleanupFile(string filename)
        {
            // Since we did not succesfully create the backup, cleanup the file.
            if (File.Exists(filename))
            {
                try
                {
                    File.Delete(filename);
                }
                catch (IOException ex)
                {
                    log.Error("Failed cleaning up backup file.", ex);
                }
            }
        }

        /// <summary>
        /// Create a backup of SQL Server to the specified backup file
        ///
        /// For EditionIDs, see https://docs.microsoft.com/en-us/sql/t-sql/functions/serverproperty-transact-sql?view=sql-server-2017
        ///
        /// For compression support, see https://technet.microsoft.com/en-us/library/cc645993(v=sql.105).aspx
        /// Then change the "Other Versions" drop down to see each SQL Server version's features.
        /// </summary>
        private SqlServerEditionIdType CreateSqlBackup(ProgressItem progressItem, string database, string backupFile)
        {
            log.InfoFormat("Backing up '{0}' to '{1}'", database, backupFile);

            SqlServerEditionIdType sqlServerEditionId = SqlServerEditionIdType.Express;

            progressItem.Starting();
            progressItem.Detail = "Connecting to SQL Server";

            using (DbConnection con = SqlSession.Current.OpenConnection())
            {
                DbCommand cmd = DbCommandProvider.Create(con);
                cmd.CommandTimeout = (int) TimeSpan.FromHours(2).TotalSeconds;

                cmd.CommandText = $@"
                    DECLARE @EditionTypeId sql_variant

                    set @EditionTypeId = serverproperty('EditionID')

                    /* These are editions that support compression 
                    Enterprise:									1804890536
                    Enterprise Edition: Core-based Licensing:	1872460670 
                    Enterprise Evaluation:						610778273
                    Standard:									-1534726760
                    Business Intelligence:						284895786
                    Developer:									-2117995310
                    */
                    IF    1804890536 = @EditionTypeId 
                      or  1872460670 = @EditionTypeId
                      or  610778273  = @EditionTypeId
                      or -1534726760 = @EditionTypeId 
                      or  284895786  = @EditionTypeId 
                      or -2117995310 = @EditionTypeId
                        BEGIN
                            BACKUP DATABASE @Database 
                            TO DISK = @FilePath      
                            WITH FORMAT, INIT, SKIP, NOREWIND, NOUNLOAD, COMPRESSION,  STATS = 2
                        END
                    ELSE
                        BEGIN
                            BACKUP DATABASE @Database 
                            TO DISK = @FilePath      
                            WITH INIT, NOUNLOAD, SKIP, STATS = 2, FORMAT
                        END

                    SELECT serverproperty('EditionID')";

                cmd.AddParameterWithValue("@Database", database);
                cmd.AddParameterWithValue("@FilePath", backupFile);

                Regex percentRegex = new Regex(@"(\d+) percent processed.");

                // InfoMessage will provide progress updates
                SqlConnection sqlConn = con.AsSqlConnection();
                if (sqlConn != null)
                {
                    sqlConn.InfoMessage += delegate (object sender, SqlInfoMessageEventArgs e)
                    {
                        Match match = percentRegex.Match(e.Message);
                        if (match.Success)
                        {
                            progressItem.PercentComplete = Convert.ToInt32(match.Groups[1].Value);
                            progressItem.Detail = string.Format("{0}% complete", progressItem.PercentComplete);
                        }
                    };
                }

                progressItem.Detail = "";

                try
                {
                    // The InfoMessage events only come back in real-time when using ExecuteScalar - NOT ExecuteNonQuery
                    object commandResult = DbCommandProvider.ExecuteScalar(cmd);
                    sqlServerEditionId = sqlServerEditionId.Parse(commandResult.ToString(), SqlServerEditionIdType.Express);
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 262 && ex.Message.ToLower().Contains("backup"))
                    {
                        throw new ApplicationException(
                            string.Format("The SQL Server user '{0}' does not have permission to backup the database.",
                           SqlUtility.GetUsername(con)), ex);
                    }
                    else
                    {
                        throw;
                    }
                }

                progressItem.PercentComplete = 100;
                progressItem.Detail = "Done";
                progressItem.Completed();
            }

            return sqlServerEditionId;
        }

        /// <summary>
        /// Create the zip file from the sql server backup
        /// </summary>
        private void CreateBackupZip(ProgressItem progressItem, ZipWriter zipWriter, string zipFile)
        {
            progressItem.Starting();

            // Progress updates
            zipWriter.Progress += delegate (object sender, ZipWriterProgressEventArgs args)
                {
                    progressItem.PercentComplete = (int) (((float) args.TotalBytesProcessed / (float) args.TotalBytesTotal) * 100);
                    progressItem.Detail = string.Format("{0} processed", StringUtility.FormatByteCount(args.TotalBytesProcessed));

                    args.Cancel = IsCancelled;
                };

            zipWriter.Save(zipFile);

            if (!IsCancelled)
            {
                progressItem.Detail = "Done";
            }

            progressItem.Completed();
        }

        #endregion

        #region Restore

        /// <summary>
        /// Restore a backup from the given filename
        /// </summary>
        public void RestoreBackup(string filename)
        {
            if (filename == null)
            {
                throw new ArgumentNullException("filename");
            }

            // Create the progress items that occur
            ProgressItem decompressProgress = new ProgressItem("Decompress Backup");
            ProgressItem restoreProgress = new ProgressItem("Restore SQL Server Backup");

            if (progress != null)
            {
                progress.ProgressItems.Add(decompressProgress);
                progress.ProgressItems.Add(restoreProgress);
            }

            // Create a temporary working directory
            string tempPath = DataPath.CreateUniqueTempPath();

            ApplyFolderPermissions(tempPath);

            using (SqlSessionScope scope = new SqlSessionScope(sqlSession))
            {
                if (!SqlSession.Current.IsLocalServer())
                {
                    throw new InvalidOperationException("A ShipWorks restore can only be done on the computer that is running SQL Server.");
                }

                if (!SqlSession.Current.IsSqlServer2008OrLater())
                {
                    throw new InvalidOperationException("A ShipWorks restore can only be done when connected to SQL Server 2008 or better.");
                }

                EnsureUserPermission(PermissionType.DatabaseRestore);

                log.Info("Starting restore");

                // Extract all the database files
                BackupDatabase database = ExtractRestoreDatabase(filename, tempPath, decompressProgress);

                if (IsCancelled)
                {
                    return;
                }

                if (database == null)
                {
                    throw new InvalidDataException("The file is not a valid ShipWorks backup, or the SQL Server database was not included when the backup was created.");
                }

                database.Progress = restoreProgress;

                RestoreSqlBackup(database);
            }
        }

        /// <summary>
        /// Extract the backup file to the to given path
        /// </summary>
        private BackupDatabase ExtractRestoreDatabase(string zipFilePath, string tempPath, ProgressItem progress)
        {
            BackupDatabase database = null;

            progress.Starting();

            using (ZipReader reader = new ZipReader(zipFilePath))
            {
                // Progress
                reader.Progress += delegate (object sender, ZipReaderProgressEventArgs args)
                    {
                        progress.PercentComplete = (int) (((float) args.TotalBytesProcessed / (float) args.TotalBytesTotal) * 100);
                        progress.Detail = string.Format("{0} processed", StringUtility.FormatByteCount(args.TotalBytesProcessed));

                        args.Cancel = IsCancelled;
                    };

                ZipReaderItem item = reader.ReadItems().OfType<ZipReaderItem>()
                    .FirstOrDefault(x => x.Name.StartsWith(@"Database\") && x.Name.EndsWith("shipworks.dat"));

                if (item != null)
                {
                    log.InfoFormat("Extracting '{0}'...", item.Name);

                    string targetPath = Path.Combine(tempPath, Path.GetFileName(item.Name));
                    item.Extract(targetPath);

                    database = new BackupDatabase
                    {
                        BackupFile = targetPath,
                        DatabaseName = SqlSession.Current.Configuration.DatabaseName,
                        Progress = null,
                    };
                }
            }

            if (!IsCancelled)
            {
                progress.Detail = "Done";
                progress.PercentComplete = 100;
            }

            progress.Completed();

            return database;
        }

        /// <summary>
        /// Restore's the backup into SQL Server
        /// </summary>
        [NDependIgnoreLongMethod]
        private void RestoreSqlBackup(BackupDatabase database)
        {
            ProgressItem progress = database.Progress;
            string sourceDb = null;
            string sourceLog = null;
            string targetDb;
            string targetLog;

            // Can't cancel during restore
            progress.CanCancel = false;
            progress.Starting();
            progress.Detail = "Connecting to SQL Server";

            log.InfoFormat("Restoring '{0}'...", database.DatabaseName);

            using (DbConnection con = SqlSession.Current.OpenConnection())
            {
                // Change into the database we are restoring into
                con.ChangeDatabase(database.DatabaseName);

                GetPhysicalFileLocations(con, out targetDb, out targetLog);

                log.InfoFormat("Data file: {0}", targetDb);
                log.InfoFormat("Log file: {0}", targetLog);

                if (targetDb == null || targetLog == null)
                {
                    throw new FileNotFoundException("Could not locate physical database files.");
                }

                // We have to move the original logical files to the the file locations we are restoring to.  First we find
                // the current logical file names.
                DbCommand cmd = DbCommandProvider.Create(con);
                cmd.CommandText = "restore filelistonly from disk = @FilePath";
                cmd.AddParameterWithValue("@FilePath", database.BackupFile);

                try
                {
                    // Determine logical names in the backup source
                    using (DbDataReader reader = DbCommandProvider.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            string type = (string) reader["Type"];

                            if (type == "D")
                            {
                                sourceDb = (string) reader["LogicalName"];
                            }

                            if (type == "L")
                            {
                                sourceLog = (string) reader["LogicalName"];
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 262 && ex.Message.ToLower().Contains("create"))
                    {
                        throw new ApplicationException(
                            string.Format("The SQL Server user ('{0}') does not have permissions to restore database backups.",
                           SqlUtility.GetUsername(con)), ex);
                    }
                    else
                    {
                        throw;
                    }
                }

                log.InfoFormat("Logical Data file: {0}", sourceDb);
                log.InfoFormat("Logical Log file: {0}", sourceLog);

                // See if we have everything we need
                if (sourceDb == null || sourceLog == null)
                {
                    throw new InvalidOperationException("The logical file groups could not be located in the SQL Server backup.");
                }
            }

            using (DbConnection masterConnection = SqlSession.OpenConnectionToMaster())
            {
                string restoreSqlString = CreateRestoreSqlString(sourceDb, sourceLog, targetDb, targetLog);
                DbCommand cmdRestore = CreateRestoreCommand(restoreSqlString, masterConnection, database.DatabaseName);
                SetupRestoreParameters(database, cmdRestore);
                ExecuteSqlRestore(masterConnection, cmdRestore, database.DatabaseName, progress);
            }

            progress.Detail = "Done";
            progress.Completed();
        }

        /// <summary>
        /// Gets the physical locations of the database files of the active database on the connection
        /// </summary>
        private static void GetPhysicalFileLocations(DbConnection con, out string targetDb, out string targetLog)
        {
            DbCommand cmd = DbCommandProvider.Create(con);
            cmd.CommandText = "sp_helpfile";
            cmd.CommandType = CommandType.StoredProcedure;

            // set outputs
            targetDb = null;
            targetLog = null;

            // Determine current physical files
            using (DbDataReader reader = DbCommandProvider.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    string file = ((string) reader["filename"]).Trim();
                    string usage = ((string) reader["usage"]).Trim();

                    if (usage == "log only")
                    {
                        targetLog = file;
                    }

                    if (usage == "data only")
                    {
                        targetDb = file;
                    }
                }
            }
        }

        /// <summary>
        /// Execute the restore operation
        /// </summary>
        private void ExecuteSqlRestore(DbConnection con, DbCommand cmdRestore, string databaseName, ProgressItem progress)
        {
            RestoreStarting?.Invoke(this, EventArgs.Empty);

            progress.Detail = "Logging off all users";

            try
            {
                Regex percentRegex = new Regex(@"(\d+) percent processed.");

                // InfoMessage will provide progress updates
                SqlConnection sqlConn = con.AsSqlConnection();
                if (sqlConn != null)
                {
                    sqlConn.InfoMessage += delegate (object sender, SqlInfoMessageEventArgs e)
                    {
                        Match match = percentRegex.Match(e.Message);
                        if (match.Success)
                        {
                            progress.PercentComplete = Convert.ToInt32(match.Groups[1].Value);
                            progress.Detail = string.Format("{0}% complete", progress.PercentComplete);
                        }
                    };
                }

                progress.Detail = "Initiating restore";

                // The InfoMessage events only come back in real-time when using ExecuteScalar - NOT ExecuteNonQuery
                DbCommandProvider.ExecuteScalar(cmdRestore);
            }
            finally
            {
                SqlConnection.ClearAllPools();
            }
        }

        /// <summary>
        /// Creates the initial sql string needed for a DbCommand
        /// </summary>
        private static string CreateRestoreSqlString(string sourceDb, string sourceLog, string targetDb, string targetLog)
        {
            return $@"RESTORE DATABASE @Database
                    FROM DISK = @FilePath 
                    WITH NOUNLOAD, STATS = 3, RECOVERY, REPLACE,   
                    MOVE '{sourceDb}' TO '{targetDb}',   
                    MOVE '{sourceLog}' TO '{targetLog}';";
        }

        /// <summary>
        /// Creates the Restore DbCommand
        /// </summary>
        private static DbCommand CreateRestoreCommand(string sqlString, DbConnection con, string databaseName)
        {
            DbCommand cmdRestore = DbCommandProvider.Create(con);
            string sql = $@"{sqlString}
                

                {SqlUtility.ConfigureSqlServerForClrSql(databaseName, SqlUtility.GetUsername(con))}";

            cmdRestore.CommandText = SqlUtility.WrapSqlInSingleUserMode(sql, databaseName);

            cmdRestore.CommandTimeout = 1800;

            return cmdRestore;
        }

        /// <summary>
        /// Sets the FilePath and Database parameters
        /// </summary>
        private static void SetupRestoreParameters(BackupDatabase database, DbCommand cmdRestore)
        {
            cmdRestore.AddParameterWithValue("@FilePath", database.BackupFile);
            cmdRestore.AddParameterWithValue("@Database", database.DatabaseName);
        }

        #endregion

        #region Common

        /// <summary>
        /// Ensure the user has permission for the given event.
        /// </summary>
        private void EnsureUserPermission(PermissionType type)
        {
            // See if we don't have a user
            if (userID < 0)
            {
                // If its a totally fresh database, its OK for a restore
                if (type == PermissionType.DatabaseRestore && GetTableCount() == 0)
                {
                    log.Info("Access to restore granted to null user due to fresh database.");
                    return;
                }

                if (SqlSchemaUpdater.GetInstalledSchemaVersion() < new Version(3, 0))
                {
                    log.Info("Access granted to null user due to 2x schema.");
                    return;
                }

                if (!UserUtility.HasAdminUsers())
                {
                    log.Info("Access granted to null user due to no admin users.");
                    return;
                }

                throw new InvalidOperationException("A user is required.");
            }

            // If we have a user, make sure they have backup permissions
            if (userID > 0)
            {
                if (!SecurityContext.HasPermission(userID, type))
                {
                    throw new PermissionException(type);
                }
            }
        }

        /// <summary>
        /// Get the number of tables in the current database. Just used to determine if its a fresh database.
        /// </summary>
        private int GetTableCount()
        {
            using (DbConnection con = SqlSession.Current.OpenConnection())
            {
                return (int) DbCommandProvider.ExecuteScalar(con, "select count(*) from sys.tables");
            }
        }

        /// <summary>
        /// Apply folder permissons to the given path to make sure SQL Server can access it
        /// </summary>
        private void ApplyFolderPermissions(string filePath)
        {
            DirectoryInfo di = new DirectoryInfo(filePath);
            DirectorySecurity ds = di.GetAccessControl();

            SecurityIdentifier sid = new SecurityIdentifier(WellKnownSidType.WorldSid, null);

            ds.AddAccessRule(new FileSystemAccessRule(sid,
                FileSystemRights.Modify | FileSystemRights.ReadPermissions | FileSystemRights.Delete,
                InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                PropagationFlags.None,
                AccessControlType.Allow));

            di.SetAccessControl(ds);
        }


        #endregion
    }
}
