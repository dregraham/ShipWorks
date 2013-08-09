using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Connection;
using System.IO;
using ShipWorks.Users.Security;
using log4net;
using ShipWorks.Actions.Tasks.Common.Editors;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// Task for backing up the database
    /// </summary>
    [ActionTask("Backup the database", "BackupDatabase", ActionTriggerClassifications.Scheduled)]
    public class BackupDatabaseTask : ActionTask
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(BackupDatabaseTask));
        private const string FileNameSuffixPattern = @".*\d{4}-\d{2}-\d{2} \d{2}-\d{2}-\d{2}\.swb";

        /// <summary>
        /// Constructor
        /// </summary>
        public BackupDatabaseTask()
        {
            KeepNumberOfBackups = 5;
            LimitNumberOfBackupsRetained = true;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public override ActionTaskEditor CreateEditor()
        {
            return new BackupDatabaseTaskEditor(this);
        }

        /// <summary>
        /// Directory in which to store scheduled backups
        /// </summary>
        public string BackupDirectory { get; set; }

        /// <summary>
        /// Base file name to which a timestamp will be appended
        /// </summary>
        public string FilePrefix { get; set; }

        /// <summary>
        /// How many backups should be kept in the backup directory
        /// </summary>
        public int KeepNumberOfBackups { get; set; }

        /// <summary>
        /// Backing up the database does not require input
        /// </summary>
        public override bool RequiresInput
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to [limit the number of backups retained].
        /// </summary>
        public bool LimitNumberOfBackupsRetained { get; set; }

        /// <summary>
        /// Runs the database backup and cleanup task
        /// </summary>
        protected override void Run()
        {
            if (string.IsNullOrWhiteSpace(BackupDirectory))
            {
                log.Warn("Cannot start backup because backup directory was not set.");
                return;
            }

            if (string.IsNullOrWhiteSpace(FilePrefix))
            {
                log.Warn("Cannot start backup because the file prefix was not set.");
                return;
            }

            try
            {
                ShipWorksBackup backup = new ShipWorksBackup(SqlSession.Current, SuperUser.Instance);
                backup.CreateBackup(BackupFilePath(DateTime.Now));
            }
            catch (InvalidOperationException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
            
            if (LimitNumberOfBackupsRetained)
            {
                DirectoryInfo backupPath = new DirectoryInfo(BackupDirectory);

                // The regex match greatly reduces the chance of deleting files that aren't created by this scheduled
                // task. Match on file prefix and the suffix pattern in case there are differently named backups in 
                // the same directory (don't want to delete a backup not related to this specific task)
                Regex backupFileNameMatcher = new Regex(string.Format("{0} {1}", FilePrefix, FileNameSuffixPattern));

                IEnumerable<FileInfo> filesToDelete = backupPath.GetFiles()
                       .Where(f => backupFileNameMatcher.IsMatch(f.Name))
                       .OrderByDescending(f => f.LastWriteTimeUtc)
                       .Skip(KeepNumberOfBackups);

                foreach (FileInfo file in filesToDelete)
                {
                    try
                    {
                        file.Delete();
                    }
                    catch (IOException)
                    {
                        log.WarnFormat("Could not delete backup {0} because it is in use.", file.Name);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        log.WarnFormat("Could not delete backup {0} because the user does not have access to it or the file is read-only.", file.Name);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the full file path for a backup created at a specified date
        /// </summary>
        /// <param name="forDate">Date that will be appended to the filename</param>
        private string BackupFilePath(DateTime forDate)
        {
            string fileName = string.Format("{0} {1:yyyy-MM-dd HH-mm-ss}.swb", FilePrefix, forDate);
            return Path.Combine(BackupDirectory, fileName);
        }
    }
}
