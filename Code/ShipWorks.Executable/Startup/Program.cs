using System;
using System.Diagnostics.CodeAnalysis;

namespace ShipWorks.Startup
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [SuppressMessage("CSharp.Analyzers",
            "CS4014: Because this call is not awaited, execution of the current method continues before the call is completed",
            Justification = "The main entry point cannot be awaited")]
        [STAThread]
        static void Main(string[] args)
        {
            ContainerInitializer.Initialize();

            ShipWorks.Program.Main();
        }
    }
}
