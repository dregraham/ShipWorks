using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading;
using log4net;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.ApplicationCore.Services.Installers;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Connection;

namespace ShipWorks.ApplicationCore.CommandLineOptions
{
    /// <summary>
    /// Command line option that gets called by the ShipWorks uninstaller
    /// </summary>
    public class UninstallShipWorksCommandLineOption : ICommandLineCommandHandler
    {
        /// <summary>
        /// The CommandName that can be sent to the ShipWorks.exe
        /// </summary>
        public string CommandName
        {
            get { return "uninstall"; }
        }

        /// <summary>
        /// Execute the command
        /// </summary>
        public void Execute(List<string> args)
        {
            // Shutdown logging so we can cleanup the log folders
            LogManager.Shutdown();

            // Shot down any running services
            UninstallServicesCommand uninstallServices = new UninstallServicesCommand();
            uninstallServices.Execute(new List<string> { "/all" });

            Thread.Sleep(TimeSpan.FromSeconds(1));

            // Figure out SQL Session to see if we have anything to cleanup
            SqlSession.Initialize();

            // If it's LocalDB, we know no one else is trying to use the database, so we detatch it.
            if (SqlSession.IsConfigured && SqlSession.Current.Configuration.IsLocalDb())
            {
                try
                {
                    using (DbConnection con = SqlSession.Current.OpenConnection())
                    {
                        ShipWorksDatabaseUtility.DetachDatabase(con.Database, con);
                    }
                }
                catch (SqlException)
                {
                    // Nothing to log - we are in the middle of wiping the log folder!
                }
            }

            //  Try to delete all files
            foreach (string file in Directory.EnumerateFiles(DataPath.InstanceRoot, "*", SearchOption.AllDirectories))
            {
                try
                {
                    File.Delete(file);
                }
                catch (Exception)
                {
                    // Nothing to log - we are in the middle of wiping the log folder!
                }
            }

            //  Try to delete all folders
            foreach (string folder in Directory.EnumerateDirectories(DataPath.InstanceRoot, "*", SearchOption.AllDirectories).Union(new string[] { DataPath.InstanceRoot }))
            {
                try
                {
                    Directory.Delete(folder, true);
                }
                catch (Exception)
                {
                    // Nothing to log - we are in the middle of wiping the log folder!
                }
            }
        }
    }
}
