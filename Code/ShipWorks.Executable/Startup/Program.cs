using System;
using System.Diagnostics.CodeAnalysis;

namespace ShipWorks.Startup
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        [SuppressMessage("CSharp.Analyzers",
            "CS4014: Because this call is not awaited, execution of the current method continues before the call is completed",
            Justification = "The main program method cannot be async")]
        static void Main(string[] args)
        {
            ContainerInitializer.Initialize();

            ShipWorks.Program.Main();
        }
    }
}
