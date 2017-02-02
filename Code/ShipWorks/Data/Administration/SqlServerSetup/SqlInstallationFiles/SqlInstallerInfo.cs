using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Administration.SqlServerSetup.SqlInstallationFiles;

namespace ShipWorks.Data.Administration.SqlServerSetup.SqlInstallationFiles
{
    /// <summary>
    /// SQL Server installer information.
    /// </summary>
    public class SqlInstallerInfo : ISqlInstallerInfo
    {
        /// <summary>
        /// The edition to download the installer
        /// </summary>
        public SqlServerEditionType Edition { get; set; }

        /// <summary>
        /// The Uri to download the installer
        /// </summary>
        public Uri DownloadUri { get; set; }

        /// <summary>
        /// Is this a LocalDB installer
        /// </summary>
        public bool IsLocalDB { get; set; }

        /// <summary>
        /// File size of the installer
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// Checksum of the installer
        /// </summary>
        public string Checksum { get; set; }

        /// <summary>
        /// The minimum version of the OS for which this installer is valid
        /// </summary>
        public Version MinOsVersion { get; set; }

        /// <summary>
        /// The minimum version of .Net for which this installer is valid
        /// </summary>
        public Version MinDotNetVersion { get; set; }

        /// <summary>
        /// Is the installer 64 bit
        /// </summary>
        public bool Is64Bit { get; set; }

        /// <summary>
        /// The local filename after download
        /// </summary>
        public string LocalFileName { get; set; }

        /// <summary>
        /// The path to the location of the SQL installer for new installations on the local disk
        /// </summary>
        public string LocalFilePath
        {
            get
            {
                return Path.Combine(DataPath.Components, LocalFileName);
            }
        }

        /// <summary>
        /// Get the server instance connection string for LocalDB
        /// </summary>
        public string LocalDbServerInstance { get; set; }
    }
}
