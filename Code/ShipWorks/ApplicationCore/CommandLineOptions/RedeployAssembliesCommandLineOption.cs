using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Connection;
using log4net;

namespace ShipWorks.ApplicationCore.CommandLineOptions
{
    /// <summary>
    /// Command line option for redeploying the ShipWorks assemblies to Sql Server.
    /// </summary>
    public class RedeployAssembliesCommandLineOption : ICommandLineCommandHandler
    {
        static readonly ILog log = LogManager.GetLogger(typeof(RedeployAssembliesCommandLineOption));

        /// <summary>
        /// The CommandName that can be sent to the ShipWorks.exe
        /// </summary>
        public string CommandName
        {
            get { return "redeployassemblies"; }
        }

        /// <summary>
        /// Execute the command
        /// </summary>
        public void Execute(List<string> args)
        {
            try
            {
                log.Info("Processing request to redeploy assemblies.");

                SqlSession.Initialize();
                using (SqlConnection sqlConnection = SqlSession.Current.OpenConnection())
                {
                    using (SqlTransaction transaction = sqlConnection.BeginTransaction())
                    {
                        SqlAssemblyDeployer.DropAssemblies(sqlConnection, transaction);
                        SqlAssemblyDeployer.DeployAssemblies(sqlConnection, transaction);

                        transaction.Commit();

                        log.Info("Successfully redeployed assemblies.");   
                    }
                }
            }
            catch (SqlException ex)
            {
                log.Error("Failed to redeploy assemblies.", ex);
                Environment.ExitCode = ex.ErrorCode;
            }
        }
    }
}
