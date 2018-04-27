using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using CefSharp;
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
        [SuppressMessage("CSharp.Analyzers",
            "CS4014: Because this call is not awaited, execution of the current method continues before the call is completed",
            Justification = "The main entry point cannot be awaited")]
        [STAThread]
        [SuppressMessage("CSharp.Analyzers",
            "CS4014: Because this call is not awaited, execution of the current method continues before the call is completed",
            Justification = "The main program method cannot be async")]
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.AssemblyResolve += Resolver;

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

            var registry = new RegistryHelper(@"Software\Interapptive\ShipWorks\Options");
            if (registry.GetValue("TraceTasks", false))
            {
                FlowReservoir.Enroll();
            }

            LoadApp();
        }

        /// <summary>
        /// Load the app after configuring Chromium
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void LoadApp()
        {
            var settings = new CefSettings();

            // Set BrowserSubProcessPath based on app bitness at runtime
            settings.BrowserSubprocessPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                                                   Environment.Is64BitProcess ? "x64" : "x86",
                                                   "CefSharp.BrowserSubprocess.exe");

            // Make sure you set performDependencyCheck false
            Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);

            ShipWorks.Program.Main();
        }

        /// <summary>
        /// Resolve CEF assemblies using correct bitness
        /// </summary>
        private static Assembly Resolver(object sender, ResolveEventArgs args)
        {
            if (args.Name.StartsWith("CefSharp"))
            {
                string assemblyName = args.Name.Split(new[] { ',' }, 2)[0] + ".dll";
                string archSpecificPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                                                       Environment.Is64BitProcess ? "x64" : "x86",
                                                       assemblyName);

                if (File.Exists(archSpecificPath))
                {
                    return Assembly.LoadFile(archSpecificPath);
                }
            }

            return null;
        }
    }
}
