using System;
using System.Diagnostics.CodeAnalysis;
using Interapptive.Shared.Messaging.Logging;
using Newtonsoft.Json;
using SD.Tools.OrmProfiler.Interceptor;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.JsonConverters;

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

            MessageLogger.Current.AddConverters(() => new JsonConverter[] {
                new ShipmentEntityJsonConverter(),
                new StoreEntityJsonConverter()
            });

            ContainerInitializer.Initialize();

            ShipWorks.Program.Main();
        }
    }
}
