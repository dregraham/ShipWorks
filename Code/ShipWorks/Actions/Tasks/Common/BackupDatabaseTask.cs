using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data;
using System.Xml.Serialization;
using System.Media;
using ShipWorks.ApplicationCore;
using System.IO;
using ShipWorks.Users.Security;
using log4net;
using ShipWorks.Actions.Tasks.Common.Editors;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// Task for backing up the database
    /// </summary>
    [ActionTask("Backup the database", "BackupDatabase", true)]
    public class BackupDatabaseTask : ActionTask
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(BackupDatabaseTask));

        private string backupDirectory;
        private string filePrefix;
        private int keepNumberOfBackups;
        private bool cleanOldBackups;
        private readonly Regex backupFileNameMatcher = new Regex(@".*\d{4}-\d{2}-\d{2} \d{2}-\d{2}-\d{2}\.swb");

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
        public string BackupDirectory { 
            get { return backupDirectory; }
            set { backupDirectory = value; }
        }

        /// <summary>
        /// Base file name to which a timestamp will be appended
        /// </summary>
        public string FilePrefix
        {
            get { return filePrefix; }
            set { filePrefix = value; }
        }

        /// <summary>
        /// How many backups should be kept in the backup directory
        /// </summary>
        public int KeepNumberOfBackups
        {
            get { return keepNumberOfBackups; }
            set { keepNumberOfBackups = value; }
        }

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
        /// Should old backups be cleaned
        /// </summary>
        public bool CleanOldBackups
        {
            get { return cleanOldBackups; }
            set { cleanOldBackups = value; }
        }

        /// <summary>
        /// Runs the database backup and cleanup task
        /// </summary>
        protected override void Run()
        {
            try
            {
                ShipWorksBackup backup = new ShipWorksBackup(SqlSession.Current, SuperUser.Instance);
                backup.CreateBackup(BackupFilePath(DateTime.Now));
            }
            catch (InvalidOperationException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
            
            if (CleanOldBackups)
            {
                // The regex match greatly reduces the chance of deleting files
                // that aren't created by this scheduled task
                DirectoryInfo backupPath = new DirectoryInfo(BackupDirectory);
                IEnumerable<FileInfo> filesToDelete = backupPath.GetFiles()
                       .Where(f => backupFileNameMatcher.IsMatch(f.Name))
                       .OrderByDescending(f => f.LastWriteTimeUtc)
                       .Skip(keepNumberOfBackups);

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
