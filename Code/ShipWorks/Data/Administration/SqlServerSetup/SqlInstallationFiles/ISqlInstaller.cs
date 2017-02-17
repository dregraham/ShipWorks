using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Administration.SqlServerSetup.SqlInstallationFiles
{
    /// <summary>
    /// SQL Server installer information.
    /// </summary>
    public interface ISqlInstallerInfo
    {
        /// <summary>
        /// The edition to download the installer
        /// </summary>
        SqlServerEditionType Edition { get; set; }

        /// <summary>
        /// The Uri to download the installer
        /// </summary>
        Uri DownloadUri { get; set; }

        /// <summary>
        /// Is this a LocalDB installer
        /// </summary>
        bool IsLocalDB { get; set; }

        /// <summary>
        /// File size of the installer
        /// </summary>
        long FileSize { get; set; }

        /// <summary>
        /// Checksum of the installer
        /// </summary>
        string Checksum { get; set; }

        /// <summary>
        /// The minimum version of the OS for which this installer is valid
        /// </summary>
        Version MinOsVersion { get; set; }

        /// <summary>
        /// The minimum version of .Net for which this installer is valid
        /// </summary>
        Version MinDotNetVersion { get; set; }

        /// <summary>
        /// Is the installer 64 bit
        /// </summary>
        bool Is64Bit { get; set; }

        /// <summary>
        /// The local filename after download
        /// </summary>
        string LocalFileName { get; set; }

        /// <summary>
        /// The path to the location of the SQL installer for new installations on the local disk
        /// </summary>
        string LocalFilePath { get; }

        /// <summary>
        /// Get the server instance connection string for LocalDB
        /// </summary>
        string LocalDbServerInstance { get; set; }
    }
}
