using System;
using Interapptive.Shared.Metrics;
using ShipWorks.Startup;

namespace ShipWorks.CommandLine
{
    class Program
    {
        /// <summary>
        /// Command line executable entry point
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please provide command line parameters.");
                return;
            }

            ContainerInitializer.Initialize();

            ShipWorks.Program.Main().GetAwaiter().GetResult();

            Telemetry.Flush();
        }
    }
}
