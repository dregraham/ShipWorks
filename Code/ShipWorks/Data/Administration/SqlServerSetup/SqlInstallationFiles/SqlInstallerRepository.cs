﻿using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Data.Administration.SqlServerSetup.SqlInstallationFiles
{
    /// <summary>
    /// Class that creates SqlInstallerInfo objects
    /// </summary>
    [Component]
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
                        Edition = SqlServerEditionType.Express2014,
                        DownloadUri = new Uri(@"http://www.interapptive.com/download/components/newinstallersql42016/sqlserver2014/express/SQLEXPR_x86_ENU.exe"),
                        Checksum = "QwRailFjurfxA0EcTX+b63C+0iU=",
                        FileSize = 287690752,
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
                        DownloadUri = new Uri(@"http://www.interapptive.com/download/components/newinstallersql42016/sqlserver2014/express/SQLEXPR_x64_ENU.exe"),
                        Checksum = "NNfyu6+UUqQRZBVuw3uaQvgUOQw=",
                        FileSize = 326873088,
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
                        DownloadUri = new Uri(@"http://www.interapptive.com/download/components/newinstallersql42016/sqlserver2014/localdb/x86/SqlLocalDB.msi"),
                        Checksum = "cZiP0Nfei4JS2xKuIR0vi9c2+d4=",
                        FileSize = 38662144,
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
                        DownloadUri = new Uri(@"http://www.interapptive.com/download/components/newinstallersql42016/sqlserver2014/localdb/x64/SqlLocalDB.msi"),
                        Checksum = "qtqPAQAR8bRplnpMmzMuMyLawc8=",
                        FileSize = 45563904,
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
                        Edition = SqlServerEditionType.Express2017,
                        DownloadUri = new Uri(@"http://www.interapptive.com/download/components/sqlserver2017/express/SQLEXPR_x64_ENU.exe"),
                        Checksum = "4KWjiCVSRPH16y+/Rr3HKS9+PY4=",
                        FileSize = 289058768,
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
                        Edition = SqlServerEditionType.LocalDb2017,
                        DownloadUri = new Uri(@"http://www.interapptive.com/download/components/sqlserver2017/localdb/SqlLocalDB.msi"),
                        Checksum = "cTIVWe/5IwZnmdf8JrbHrt1B7uE=",
                        FileSize = 48230400,
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
