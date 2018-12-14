using System;
using System.Diagnostics.CodeAnalysis;
using Interapptive.Shared.Messaging.Logging;
using Interapptive.Shared.StackTraceHelper;
using Interapptive.Shared.Win32;
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
        [STAThread]
        [SuppressMessage("CSharp.Analyzers",
            "CS4014: Because this call is not awaited, execution of the current method continues before the call is completed",
            Justification = "The main program method cannot be async")]
        static void Main(string[] args)
        {
            // The default value of KeepTextBoxDisplaySynchronizedWithTextProperty depends on which version of dotnet the app targets
            // 4.0 defaults to false while 4.5 defaults to true, ShipWorks was built assuming the value is false so we set it here.
            // see https://docs.microsoft.com/en-us/dotnet/api/system.windows.frameworkcompatibilitypreferences.keeptextboxdisplaysynchronizedwithtextproperty?view=netframework-4.5
            System.Windows.FrameworkCompatibilityPreferences.KeepTextBoxDisplaySynchronizedWithTextProperty = false;
#if DEBUG
            if (InterapptiveOnly.MagicKeysDown)
            {
                InterceptorCore.Initialize("ShipWorks");

                MessageLogger.Current.AddConverters(() => new JsonConverter[] {
                    new ShipmentEntityJsonConverter(),
                    new StoreEntityJsonConverter()
                });
            }
#endif

            ContainerInitializer.Initialize();

            var registry = new RegistryHelper(@"Software\Interapptive\ShipWorks\Options");
            if (registry.GetValue("TraceTasks", false))
            {
                FlowReservoir.Enroll();
            }

            ShipWorks.Program.Main().GetAwaiter().GetResult();
        }
    }
}
