using System;
using Interapptive.Shared.Metrics;
using ShipWorks.ApplicationCore;

namespace ShipWorks.CommandLine
{
    /// <summary>
    /// Command line version of ShipWorks
    /// </summary>
    class Program
    {
        /// <summary>
        /// Command line executable entry point
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // Require the user to give us some command line args
            // we don't pass the args to the ShipWorks Main method
            // they are pulled in downstream via Environment.GetCommandLineArgs()
            if (args.Length == 0)
            {
                Console.WriteLine("Please provide command line parameters.");
                return;
            }

            ShipWorks.Startup.ContainerInitializer.Initialize();

            ShipWorks.Program.Main().GetAwaiter().GetResult();

            Telemetry.Flush();
        }
    }
}
