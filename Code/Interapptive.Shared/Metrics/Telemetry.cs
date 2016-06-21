using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Transactions;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Win32;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.Implementation;

namespace Interapptive.Shared.Metrics
{
    /// <summary>
    /// Telemetry client
    /// </summary>
#pragma warning disable CS3001 // Argument type is not CLS-compliant
#pragma warning disable CS3002 // Return type is not CLS-compliant
#pragma warning disable CS3003 // Type is not CLS-compliant
#pragma warning disable CS3024 // Constraint type is not CLS-compliant
    public static class Telemetry
    {
        private static readonly TelemetryClient telemetryClient;

        /// <summary>
        /// Static constructor
        /// </summary>
        static Telemetry()
        {
            TelemetryProcessorChainBuilder builder = TelemetryConfiguration.Active.TelemetryProcessorChainBuilder;
            builder.Use((next) => new DeobfuscationProcessor(next));
            builder.Build();

            TelemetryConfiguration.Active.InstrumentationKey = GetInstrumentationKey(Assembly.GetExecutingAssembly().GetName().Version);

            telemetryClient = new TelemetryClient();
            telemetryClient.Context.Session.Id = Guid.NewGuid().ToString("D");
            telemetryClient.Context.Component.Version = Assembly.GetExecutingAssembly().GetName().Version.ToString(4);
            telemetryClient.Context.Device.OperatingSystem = Environment.OSVersion.ToString();
            telemetryClient.Context.Device.ScreenResolution = $"{Screen.PrimaryScreen.Bounds.Width}x{Screen.PrimaryScreen.Bounds.Height}";
            telemetryClient.Context.Properties.Add("Build date", AssemblyDateAttribute.Read(Assembly.GetEntryAssembly()).ToString());

            // Bitness
            telemetryClient.Context.Properties.Add("x64 OS", MyComputer.Is64BitWindows ? "Yes" : "No");
            telemetryClient.Context.Properties.Add("x64 Process", MyComputer.Is64BitProcess ? "Yes" : "No");

            // .NET version
            telemetryClient.Context.Properties.Add(".NET Version", Environment.Version.ToString(4));

            // Session
            telemetryClient.Context.Properties.Add("TerminalServices", SystemInformation.TerminalServerSession ? "Yes" : "No");

            // IE Version
            telemetryClient.Context.Properties.Add("IE Version", MyComputer.IEVersion.ToString(4));
            telemetryClient.Context.Properties.Add("Windows Installer", MyComputer.WindowsInstallerVersion.ToString(4));

            // Log the current culture settings
            telemetryClient.Context.Properties.Add("CurrentCulture", Thread.CurrentThread.CurrentCulture.ToString());
            telemetryClient.Context.Properties.Add("CurrentUICulture", Thread.CurrentThread.CurrentUICulture.ToString());

            telemetryClient.Context.Properties.Add("StartupPath", Application.StartupPath);
        }

        /// <summary>
        /// Get the storage connection string
        /// </summary>
        private static string GetInstrumentationKey(Version version)
        {
            return version.Major > 0 ?
                "3e4ff59c-d801-426c-9e6c-4cc8fa705396" :
                "68bd8748-cd5d-4187-ba7f-8865ee128aae";
        }

        /// <summary>
        /// Track when ShipWorks as started
        /// </summary>
        public static void TrackStartShipworks(string tangoCustomerID, string instanceID)
        {
            telemetryClient.Context.User.Id = tangoCustomerID;
            telemetryClient.Context.User.AuthenticatedUserId = tangoCustomerID;
            telemetryClient.Context.Device.Id = instanceID;

            telemetryClient.Context.Properties.Add("Instance", instanceID);

            EventTelemetry eventTelemetry = new EventTelemetry("StartShipWorks")
            {
                Timestamp = DateTimeOffset.UtcNow
            };

            long memoryInKB = 0;
            NativeMethods.GetPhysicallyInstalledSystemMemory(out memoryInKB);
            Process process = Process.GetCurrentProcess();

            AddEventProperty(eventTelemetry, "Screens", Screen.AllScreens.Length);
            AddEventProperty(eventTelemetry, "CPUs", Environment.ProcessorCount.ToString());
            AddEventProperty(eventTelemetry, "TransactionManager.DefaultTimeout", TransactionManager.DefaultTimeout.TotalSeconds);
            AddEventProperty(eventTelemetry, "TransactionManager.MaximumTimeout", TransactionManager.MaximumTimeout.TotalSeconds);
            AddEventProperty(eventTelemetry, "Handles", process.HandleCount);
            AddEventProperty(eventTelemetry, "Threads", process.Threads.Count);
            AddEventProperty(eventTelemetry, "UserProcessorTime(m)", process.UserProcessorTime.TotalMinutes);
            AddEventProperty(eventTelemetry, "TotalProcessorTime(m)", process.TotalProcessorTime.TotalMinutes);
            AddEventProperty(eventTelemetry, "PhysicalMemory", StringUtility.FormatByteCount(memoryInKB * 1024));
            AddEventProperty(eventTelemetry, "UserObjects", NativeMethods.GetGuiResources(process.Handle, NativeMethods.GR_USEROBJECTS));
            AddEventProperty(eventTelemetry, "GDIObjects", NativeMethods.GetGuiResources(process.Handle, NativeMethods.GR_GDIOBJECTS));
            AddEventProperty(eventTelemetry, "ScreenDimensionsPrimary", $"{Screen.PrimaryScreen.Bounds.Width}x{Screen.PrimaryScreen.Bounds.Height}");

            telemetryClient.TrackEvent(eventTelemetry);
        }

        /// <summary>
        /// Track an exception
        /// </summary>
        public static void TrackException(Exception ex, Dictionary<string, string> properties)
        {
            telemetryClient.TrackException(ex, properties);
        }

        /// <summary>
        /// Flush any stored telemetry information
        /// </summary>
        public static void Flush()
        {
            telemetryClient.Flush();
        }

        /// <summary>
        /// Add a property to the telemetry item
        /// </summary>
        private static void AddEventProperty(ISupportProperties telemetry, string name, object value)
        {
            telemetry.Properties.Add(name, value.ToString());
        }
    }
}
