using System;
using System.Diagnostics.CodeAnalysis;
using SD.Tools.OrmProfiler.Interceptor;
using ShipWorks.ApplicationCore;

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
        [SuppressMessage("CSharp.Analyzers",
            "CS4014: Because this call is not awaited, execution of the current method continues before the call is completed",
            Justification = "The main program method cannot be async")]
        static void Main(string[] args)
        {
#if DEBUG
            if (InterapptiveOnly.MagicKeysDown)
            {
                InterceptorCore.Initialize("ShipWorks");
            }
#endif
            ContainerInitializer.Initialize();

            ShipWorks.Program.Main();
        }
    }
}
