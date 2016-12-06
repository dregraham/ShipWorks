using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;

namespace ShipWorks.Data.Administration.SqlServerSetup.SqlInstallationFiles
{
    /// <summary>
    /// Class that creates SqlInstallerInfo objects
    /// </summary>
    public class SqlInstallerRepository : ISqlInstallerRepository
    {
        private List<ISqlInstallerInfo> sqlInstallers = new List<ISqlInstallerInfo>();
        private IClrHelper clrHelper;
        private IEnvironmentWrapper environment;

        /// <summary>
        /// Constructor
        /// </summary>
        public SqlInstallerRepository(IClrHelper clrHelper, IEnvironmentWrapper environment)
        {
            this.clrHelper = clrHelper;
            this.environment = environment;
        }

        /// <summary>
        /// Returns the SQL Server installer infos that apply to this machine.
        /// </summary>
        public IEnumerable<ISqlInstallerInfo> SqlInstallersForThisMachine()
        {
            if (sqlInstallers.None())
            {
                sqlInstallers.Add(
                    new SqlInstallerInfo()
                    {
                        Edition = SqlServerEditionType.Express2008R2Sp2,
                        DownloadUri = new Uri(@"http://www.interapptive.com/download/components/sqlexpress08r2sp2/SQLEXPR_x86_ENU.exe"),
                        Checksum = "bONPV6E+De2VyvRAX60TPoWa3jA=",
                        FileSize = 115763632,
                        IsLocalDB = false,
                        MinDotNetVersion = new Version(3, 5, 0, 0),
                        MinOsVersion = new Version(5, 1, 0, 0),
                        Is64Bit = false,
                        LocalFileName = "SQLEXPR_x86_ENU.exe"
                    }
                );

                sqlInstallers.Add(
                    new SqlInstallerInfo()
                    {
                        Edition = SqlServerEditionType.Express2008R2Sp2,
                        DownloadUri = new Uri(@"http://www.interapptive.com/download/components/sqlexpress08r2sp2/SQLEXPR_x64_ENU.exe"),
                        Checksum = "52ijtw4/O1lu/6n1fYEvlcCgUGs=",
                        FileSize = 128331696,
                        IsLocalDB = false,
                        MinDotNetVersion = new Version(3, 5, 0, 0),
                        MinOsVersion = new Version(5, 1, 0, 0),
                        Is64Bit = true,
                        LocalFileName = "SQLEXPR_x64_ENU.exe"
                    }
                );

                sqlInstallers.Add(
                    new SqlInstallerInfo()
                    {
                        Edition = SqlServerEditionType.Express2014,
                        DownloadUri = new Uri(@"http://www.interapptive.com/download/components/sqlserver2014/express/SQLEXPR_x86_ENU.exe"),
                        Checksum = "m58AVBOeZ4yLsTqb/w/NUTAVcSw=",
                        FileSize = 176626720,
                        IsLocalDB = false,
                        MinDotNetVersion = new Version(3, 5, 30729, 0),
                        MinOsVersion = new Version(6, 1, 7601, 0),
                        Is64Bit = false,
                        LocalFileName = "SQLEXPR_x86_ENU.exe"
                    }
                );

                sqlInstallers.Add(
                    new SqlInstallerInfo()
                    {
                        Edition = SqlServerEditionType.Express2014,
                        DownloadUri = new Uri(@"http://www.interapptive.com/download/components/sqlserver2014/express/SQLEXPR_x64_ENU.exe"),
                        Checksum = "OODQjwavL5B+GuvcoERLgorjVrU=",
                        FileSize = 206300720,
                        IsLocalDB = false,
                        MinDotNetVersion = new Version(3, 5, 30729, 0),
                        MinOsVersion = new Version(6, 1, 7601, 0),
                        Is64Bit = true,
                        LocalFileName = "SQLEXPR_x64_ENU.exe"
                    }
                );

                sqlInstallers.Add(
                    new SqlInstallerInfo()
                    {
                        Edition = SqlServerEditionType.LocalDb2014,
                        DownloadUri = new Uri(@"http://www.interapptive.com/download/components/sqlserver2014/localdb/x86/SqlLocalDB.msi"),
                        Checksum = "a1VDOq7dl8hc2Ww6OHTAuQy0A9A=",
                        FileSize = 38428672,
                        IsLocalDB = true,
                        MinDotNetVersion = new Version(3, 5, 30729, 0),
                        MinOsVersion = new Version(6, 1, 7601, 0),
                        Is64Bit = false,
                        LocalFileName = "SqlLocalDB.msi",
                        LocalDbServerInstance = @"(LocalDB)\MSSQLLocalDB"
                    }
                );

                sqlInstallers.Add(
                    new SqlInstallerInfo()
                    {
                        Edition = SqlServerEditionType.LocalDb2014,
                        DownloadUri = new Uri(@"http://www.interapptive.com/download/components/sqlserver2014/localdb/x64/SqlLocalDB.msi"),
                        Checksum = "qS6fy2NM6DnpCmvJbbFt6A9B34M=",
                        FileSize = 45215744,
                        IsLocalDB = true,
                        MinDotNetVersion = new Version(3, 5, 30729, 0),
                        MinOsVersion = new Version(6, 1, 7601, 0),
                        Is64Bit = true,
                        LocalFileName = "SqlLocalDB.msi",
                        LocalDbServerInstance = @"(LocalDB)\MSSQLLocalDB"
                    }
                );

                sqlInstallers.Add(
                    new SqlInstallerInfo()
                    {
                        Edition = SqlServerEditionType.Express2016,
                        DownloadUri = new Uri(@"http://www.interapptive.com/download/components/sqlserver2016/express/SQLEXPR_x64_ENU.exe"),
                        Checksum = "fXGkQJZlDikWuSG+TsMRTvlah3U=",
                        FileSize = 315579864,
                        IsLocalDB = false,
                        MinDotNetVersion = new Version(3, 5, 30729, 0), // The installer will install .Net 4.6, so just need 3.5 SP1
                        MinOsVersion = new Version(6, 2, 0, 0),
                        Is64Bit = true,
                        LocalFileName = "SQLEXPR_x64_ENU.exe"
                    }
                );

                sqlInstallers.Add(
                    new SqlInstallerInfo()
                    {
                        Edition = SqlServerEditionType.LocalDb2016,
                        DownloadUri = new Uri(@"http://www.interapptive.com/download/components/sqlserver2016/localdb/SqlLocalDB.msi"),
                        Checksum = "Hh1+nGXRGnt2cbX+WfpvmrH66xM=",
                        FileSize = 46149632,
                        IsLocalDB = true,
                        MinDotNetVersion = new Version(3, 5, 30729, 0), // The installer will install .Net 4.6, so just need 3.5 SP1
                        MinOsVersion = new Version(6, 2, 0, 0),
                        Is64Bit = true,
                        LocalFileName = "SqlLocalDB.msi",
                        LocalDbServerInstance = @"(LocalDB)\MSSQLLocalDB"
                    }
                );
            }

            sqlInstallers = sqlInstallers.Where(si => environment.OSVersion.Version >= si.MinOsVersion &&
                                      si.Is64Bit == environment.Is64BitOperatingSystem &&
                                      clrHelper.ClrVersions.Any(installedClrVersion => installedClrVersion >= si.MinDotNetVersion))
                                      .OrderByDescending(si => (int)si.Edition)
                                      .ToList();

            return sqlInstallers;
        }
    }
}
