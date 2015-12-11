using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using ShipWorks.UI;
using System.IO;
using Interapptive.Shared;
using System.Threading;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text.RegularExpressions;
using ICSharpCode.SharpZipLib.Zip;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Users;
using ShipWorks.Data.Model.HelperClasses;
using Interapptive.Shared.IO.Zip;
using System.Data;
using System.Xml;
using System.Reflection;
using Interapptive.Shared.Utility;
using ShipWorks.Common.Threading;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Connection;
using System.Security.AccessControl;
using System.Security.Principal;
using ShipWorks.Users.Security;
using ShipWorks.Users.Audit;
using Interapptive.Shared.Data;
using System.Linq;
using ShipWorks.Data.Administration.UpdateFrom2x;
using ShipWorks.Data.Administration.UpdateFrom2x.LegacyCode;

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
        ProgressProvider progress;

        // The user that is doing the backup
        long userID = -1;

        // The SqlSession that holds the connection information for the database to backup\restore
        SqlSession sqlSession;

        // Static (global) event that can notify any time a restore is starting.
        public static event EventHandler RestoreStarting;

        /// <summary>
        /// Internal structure representing a database to be backed up
        /// </summary>
        class BackupDatabase
        {
            public string DatabaseName { get; set; }
            public string BackupFile { get; set; }
            public bool IsArchive { get; set; }
            public ProgressItem Progress { get; set; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipWorksBackup(SqlSession sqlSession, UserEntity user, ProgressProvider progress)
        {
            this.sqlSession = sqlSession;
            this.progress = progress;
            this.userID = user.UserID;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipWorksBackup(SqlSession sqlSession, long userID, ProgressProvider progress)
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

                    // Create the backups
                    foreach (BackupDatabase database in databases)
                    {
                        CreateSqlBackup(database.Progress, database.DatabaseName, database.BackupFile);
                    }

                    if (!IsCancelled)
                    {
                        // Create the archive to be zipped
                        ZipWriter zipWriter = new ZipWriter();
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
                    IsArchive = false
                };

            // Always have the primary database
            databases.Add(primary);

            // See if the ArchiveSets table exists.  Only valid for ShipWorks2 - but during upgrade we allow backing up - which could be for ShipWorks2
            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                foreach (string archiveDbName in ShipWorks2xArchiveUtility.GetArchiveDatabaseNames(con))
                {
                    BackupDatabase archive = new BackupDatabase
                       {
                           DatabaseName = archiveDbName,
                           BackupFile = Path.Combine(tempPath, archiveDbName + ".dat"),
                           Progress = new ProgressItem(string.Format("Create Archive Backup ({0})", databases.Count)),
                           IsArchive = true
                       };

                    databases.Add(archive);
                }
            }

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
        /// </summary>
        private void CreateSqlBackup(ProgressItem progressItem, string database, string backupFile)
        {
            log.InfoFormat("Backuping up '{0}' to '{1}'", database, backupFile);

            progressItem.Starting();
            progressItem.Detail = "Connecting to SQL Server";

            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                SqlCommand cmd = SqlCommandProvider.Create(con);
                cmd.CommandTimeout = (int) TimeSpan.FromHours(2).TotalSeconds;

                cmd.CommandText =
                    "BACKUP DATABASE @Database " +
                    " TO DISK = @FilePath      " +
                    " WITH INIT, NOUNLOAD, SKIP, STATS = 2, NOFORMAT";

                cmd.Parameters.AddWithValue("@Database", database);
                cmd.Parameters.AddWithValue("@FilePath", backupFile);

                Regex percentRegex = new Regex(@"(\d+) percent processed.");

                // InfoMessage will provide progress updates
                con.InfoMessage += delegate(object sender, SqlInfoMessageEventArgs e)
                {
                    Match match = percentRegex.Match(e.Message);
                    if (match.Success)
                    {
                        progressItem.PercentComplete = Convert.ToInt32(match.Groups[1].Value);
                        progressItem.Detail = string.Format("{0}% complete", progressItem.PercentComplete);
                    }
                };

                progressItem.Detail = "";

                try
                {
                    // The InfoMessage events only come back in real-time when using ExecuteScalar - NOT ExecuteNonQuery
                    SqlCommandProvider.ExecuteScalar(cmd);
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
        }

        /// <summary>
        /// Create the zip file from the sql server backup
        /// </summary>
        private void CreateBackupZip(ProgressItem progressItem, ZipWriter zipWriter, string zipFile)
        {
            progressItem.Starting();

            // Progress updates
            zipWriter.Progress += delegate(object sender, ZipWriterProgressEventArgs args)
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
                List<BackupDatabase> databases = ExtractRestoreDatabases(filename, tempPath, decompressProgress);

                if (IsCancelled)
                {
                    return;
                }

                // If the primary backup file is not there, then its an invalid backup file, or the db wasn't backed up
                if (databases.Count(d => !d.IsArchive) != 1)
                {
                    throw new InvalidDataException("The file is not a valid ShipWorks backup, or the SQL Server database was not included when the backup was created.");
                }

                // Add in any progress for archives (we already added the main one)
                foreach (BackupDatabase database in databases)
                {
                    if (database.IsArchive)
                    {
                        progress.ProgressItems.Add(database.Progress);
                    }
                    else
                    {
                        database.Progress = restoreProgress;
                    }
                }

                // Determine what archive db names the current database is using.  Since we are overwriting this database, any archives it's using
                // are valid to be overwritten.
                List<string> previousArchiveNames;
                using (SqlConnection con = SqlSession.Current.OpenConnection())
                {
                    previousArchiveNames = ShipWorks2xArchiveUtility.GetArchiveDatabaseNames(con);
                }

                // First do the primary
                RestoreSqlBackup(databases.Single(d => !d.IsArchive));

                // Now restore each of the archives
                foreach (BackupDatabase archive in databases.Where(d => d.IsArchive))
                {
                    using (SqlConnection con = SqlSession.Current.OpenConnection())
                    {
                        // If the database name of this archive already exists - and its not from the previous archive list - then
                        // we need to leave the existing one alone (we don't know who is using it) and create a copy instead.
                        if (SqlUtility.DoesDatabaseExist(con, archive.DatabaseName) && !previousArchiveNames.Contains(archive.DatabaseName))
                        {
                            log.InfoFormat("Archive already exists in target server, have to create copy. {0}", archive.DatabaseName);

                            archive.DatabaseName = GetAlternateArchiveName(con, archive.DatabaseName);
                        }
                    }

                    RestoreSqlBackup(archive);
                }
            }
        }

        /// <summary>
        /// Get a new database archive name to replace the given original name. This is for when we are restoring a backup with an archive, and
        /// the archive name is already in use.
        /// </summary>
        private string GetAlternateArchiveName(SqlConnection con, string originalName)
        {
            string alternateName = String.Format("{0}_archive_{1}", con.Database, Guid.NewGuid().ToString("N"));
            if (alternateName.Length > 128)
            {
                // just to maintain uniqueness in this crazy scenario
                alternateName = String.Format("shipworks_archive_{0}", Guid.NewGuid().ToString("N"));
            }

            // write it to the master database
            try
            {
                SqlCommand cmd = SqlCommandProvider.Create(con);
                cmd.CommandText = "UPDATE ArchiveSets SET DbName = @newDbName WHERE DbName = @oldDbName";
                cmd.Parameters.AddWithValue("@newDbName", alternateName);
                cmd.Parameters.AddWithValue("@oldDbName", originalName);

                SqlCommandProvider.ExecuteNonQuery(cmd);

                return alternateName;
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException(
                    "An error occurred while registering a cloned Archive Set database.\n\n" +
                    "Detail:\n" + ex.Message, ex);
            }
        }

        /// <summary>
        /// Extract the backup file to the to given path
        /// </summary>
        private List<BackupDatabase> ExtractRestoreDatabases(string zipFilePath, string tempPath, ProgressItem progress)
        {
            List<BackupDatabase> databases = new List<BackupDatabase>();

            progress.Starting();

            using (ZipReader reader = new ZipReader(zipFilePath))
            {
                // Progress
                reader.Progress += delegate(object sender, ZipReaderProgressEventArgs args)
                    {
                        progress.PercentComplete = (int) (((float) args.TotalBytesProcessed / (float) args.TotalBytesTotal) * 100);
                        progress.Detail = string.Format("{0} processed", StringUtility.FormatByteCount(args.TotalBytesProcessed));

                        args.Cancel = IsCancelled;
                    };

                foreach (ZipReaderItem item in reader.ReadItems())
                {
                    // We are only looking for database backup files
                    if (item.Name.StartsWith(@"Database\") && item.Name.EndsWith(".dat"))
                    {
                        log.InfoFormat("Extracting '{0}'...", item.Name);

                        string targetPath = Path.Combine(tempPath, Path.GetFileName(item.Name));
                        item.Extract(targetPath);

                        BackupDatabase database = new BackupDatabase();
                        database.BackupFile = targetPath;
                        database.IsArchive = !item.Name.EndsWith("shipworks.dat");
                        database.DatabaseName = database.IsArchive ? Path.GetFileNameWithoutExtension(item.Name) : SqlSession.Current.Configuration.DatabaseName;
                        database.Progress = database.IsArchive ? new ProgressItem(string.Format("Restore Archive ({0})", databases.Count(b => b.IsArchive) + 1)) : null;

                        databases.Add(database);
                    }
                }
            }

            if (!IsCancelled)
            {
                progress.Detail = "Done";
                progress.PercentComplete = 100;
            }

            progress.Completed();

            return databases;
        }

        /// <summary>
        /// Restore's the backup into SQL Server
        /// </summary>
        private void RestoreSqlBackup(BackupDatabase database)
        {
            ProgressItem progress = database.Progress;

            // Can't cancel during restore
            progress.CanCancel = false;
            progress.Starting();
            progress.Detail = "Connecting to SQL Server";

            log.InfoFormat("Restoring '{0}' (Archive:{1})...", database.DatabaseName, database.IsArchive);

            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                if (database.IsArchive)
                {
                    // if the database doesn't exist yet, create it
                    if (!SqlUtility.DoesDatabaseExist(con, database.DatabaseName))
                    {
                        log.InfoFormat("Creating new blank database to restore into");
                        ShipWorksDatabaseUtility.CreateDatabase(database.DatabaseName, con);
                    }
                }

                // Change into the database we are restoring into
                con.ChangeDatabase(database.DatabaseName);

                string targetPhysDb;
                string targetPhysLog;
                GetPhysicalFileLocations(con, out targetPhysDb, out targetPhysLog);

                log.InfoFormat("Data file: {0}", targetPhysDb);
                log.InfoFormat("Log file: {0}", targetPhysLog);

                if (targetPhysDb == null || targetPhysLog == null)
                {
                    throw new FileNotFoundException("Could not locate physical database files.");
                }

                string sourceLogicalDb = null;
                string sourceLogicalLog = null;

                // We have to move the original logical files to the the file locations we are restoring to.  First we find
                // the current logical file names.
                SqlCommand cmd = SqlCommandProvider.Create(con);
                cmd.CommandText = "restore filelistonly from disk = @FilePath";
                cmd.Parameters.AddWithValue("@FilePath", database.BackupFile);

                try
                {
                    // Determine logical names in the backup source
                    using (SqlDataReader reader = SqlCommandProvider.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            string type = (string) reader["Type"];

                            if (type == "D")
                            {
                                sourceLogicalDb = (string) reader["LogicalName"];
                            }

                            if (type == "L")
                            {
                                sourceLogicalLog = (string) reader["LogicalName"];
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

                log.InfoFormat("Logical Data file: {0}", sourceLogicalDb);
                log.InfoFormat("Logical Log file: {0}", sourceLogicalLog);

                // See if we have everything we need
                if (sourceLogicalDb == null || sourceLogicalLog == null)
                {
                    throw new InvalidOperationException("The logical file groups could not be located in the SQL Server backup.");
                }

                ExecuteSqlRestore(con, database.DatabaseName, database.BackupFile, sourceLogicalDb, sourceLogicalLog, targetPhysDb, targetPhysLog, progress);

                progress.Detail = "Done";
                progress.Completed();
            }
        }

        /// <summary>
        /// Gets the physical locations of the database files of the active database on the connection
        /// </summary>
        private static void GetPhysicalFileLocations(SqlConnection con, out string targetPhysDb, out string targetPhysLog)
        {
            SqlCommand cmd = SqlCommandProvider.Create(con);
            cmd.CommandText = "sp_helpfile";
            cmd.CommandType = CommandType.StoredProcedure;

            // set outputs
            targetPhysDb = null;
            targetPhysLog = null;

            // Determine current physical files
            using (SqlDataReader reader = SqlCommandProvider.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    string file = ((string) reader["filename"]).Trim();
                    string usage = ((string) reader["usage"]).Trim();

                    if (usage == "log only")
                    {
                        targetPhysLog = file;
                    }

                    if (usage == "data only")
                    {
                        targetPhysDb = file;
                    }
                }
            }
        }

        /// <summary>
        /// Execute the restore operation
        /// </summary>
        private void ExecuteSqlRestore(SqlConnection con, string databaseName, string backupFilePath, string sourceLogicalDb, string sourceLogicalLog, string targetPhysDb, string targetPhysLog, ProgressItem progress)
        {
            if (RestoreStarting != null)
            {
                RestoreStarting(this, EventArgs.Empty);
            }

            progress.Detail = "Logging off all users";

            // Disconnect all other users
            SqlUtility.SetSingleUser(con);

            // Get out of the database we are restoring to
            con.ChangeDatabase("master");

            try
            {
                // Determine physical names of the backup target
                SqlCommand cmdRestore = SqlCommandProvider.Create(con);
                cmdRestore.CommandText =
                    "RESTORE DATABASE @Database  " +
                    " FROM DISK = @FilePath      " +
                    " WITH NOUNLOAD, STATS = 3, RECOVERY, REPLACE, " +
                    "  MOVE '" + sourceLogicalDb + "' TO '" + targetPhysDb + "', " +
                    "  MOVE '" + sourceLogicalLog + "' TO '" + targetPhysLog + "'";

                cmdRestore.Parameters.AddWithValue("@FilePath", backupFilePath);
                cmdRestore.Parameters.AddWithValue("@Database", databaseName);

                Regex percentRegex = new Regex(@"(\d+) percent processed.");

                // InfoMessage will provide progress updates
                con.InfoMessage += delegate(object sender, SqlInfoMessageEventArgs e)
                {
                    Match match = percentRegex.Match(e.Message);
                    if (match.Success)
                    {
                        progress.PercentComplete = Convert.ToInt32(match.Groups[1].Value);
                        progress.Detail = string.Format("{0}% complete", progress.PercentComplete);
                    }
                };

                progress.Detail = "Initiating restore";

                // The InfoMessage events only come back in real-time when using ExecuteScalar - NOT ExecuteNonQuery
                cmdRestore.CommandTimeout = 1800;
                SqlCommandProvider.ExecuteScalar(cmdRestore);
            }
            finally
            {
                try
                {
                    con.ChangeDatabase(databaseName);

                    // Allow multiple connections again
                    SqlUtility.SetMultiUser(con);
                }
                catch (SqlException ex)
                {
                    log.Error("Failed to set database back to multi-user mode.", ex);
                }

                SqlConnection.ClearAllPools();
            }
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
            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                return (int) SqlCommandProvider.ExecuteScalar(con, "select count(*) from sys.tables");
            }
        }

        /// <summary>
        /// Apply folder permissons to the given path to make sure SQL Server can access it
        /// </summary>
        private void ApplyFolderPermissions(string filePath)
        {
            DirectoryInfo di = new DirectoryInfo(filePath);
            DirectorySecurity ds = di.GetAccessControl();

            ds.AddAccessRule(new FileSystemAccessRule("Everyone",
                FileSystemRights.Modify | FileSystemRights.ReadPermissions | FileSystemRights.Delete,
                InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                PropagationFlags.None,
                AccessControlType.Allow));

            di.SetAccessControl(ds);
        }


        #endregion
    }
}
