﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using log4net;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Connection;

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
        public Task Execute(List<string> args)
        {
            try
            {
                log.Info("Processing request to redeploy assemblies.");

                SqlSession.Initialize();
                using (DbConnection sqlConnection = SqlSession.Current.OpenConnection())
                {
                    using (DbTransaction transaction = sqlConnection.BeginTransaction())
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

            return Task.CompletedTask;
        }
    }
}
