using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Data;
using log4net;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Connection;

namespace ShipWorks.ApplicationCore.CommandLineOptions
{
    /// <summary>
    /// Command line option to obtain a "lock" for upgrading Sql.
    /// </summary>
    public class GetUpgradeLockCommandLineOption : ICommandLineCommandHandler
    {
        private const int HoursForLockToExpire = 12;

        private static readonly ILog log = LogManager.GetLogger(typeof(GetUpgradeLockCommandLineOption));

        /// <summary>
        /// getupdatewindow
        /// </summary>
        public string CommandName => "getupdatelock";

        /// <summary>
        /// Tell the update service what the update window is
        /// </summary>
        public async Task Execute(List<string> args)
        {
            log.Info("Executing getupdatelock commandline");

            try
            {
                SqlSession.Initialize();

                if (!SqlSession.Current.CanConnect())
                {
                    log.Info("Cannot connect to SQL Server. Not getting lock");
                    return;
                }

                if (SqlSchemaUpdater.IsUpgradeRequired())
                {
                    log.Info("Update required. Not getting lock");
                    return;
                }

                using (DbConnection conn = new SqlConnection(SqlSession.Current.Configuration.GetConnectionString()))
                {
                    await conn.OpenAsync().ConfigureAwait(false);
                    int rowsUpdated = DbCommandProvider.ExecuteScalar<int>(conn, GetSql());
                    if (rowsUpdated == 0)
                    {
                        Environment.ExitCode = 1;
                    }
                }
            }
            catch (SqlException ex)
            {
                log.Error("Error when obtaining lock", ex);
                Environment.ExitCode = ex.ErrorCode;
            }
        }

        private string GetSql()
        {
            return "update Configuration " +
                "set AutoUpdateStartDate = SYSUTCDATETIME() " +
                $"where AutoUpdateStartDate < DATEADD(HOUR, -{HoursForLockToExpire}, SYSUTCDATETIME());" +
                "select @@ROWCOUNT";
        }
    }
}
