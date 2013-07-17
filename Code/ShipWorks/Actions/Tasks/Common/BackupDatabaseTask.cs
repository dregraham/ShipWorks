using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data;
using System.Xml.Serialization;
using System.Media;
using ShipWorks.ApplicationCore;
using System.IO;
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

        private string backupPath;
        private string filePrefix;
        private int keepNumberOfBackups;

        /// <summary>
        /// Constructor
        /// </summary>
        public override ActionTaskEditor CreateEditor()
        {
            return new BackupDatabaseTaskEditor(this);
        }

        /// <summary>
        /// Path in which to store scheduled backups
        /// </summary>
        public string BackupPath { 
            get { return backupPath; }
            set { backupPath = value; }
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
    }
}
