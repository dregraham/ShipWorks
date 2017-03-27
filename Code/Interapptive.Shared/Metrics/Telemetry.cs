using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
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
        public const string TotalShipmentsKey = "TotalShipments";
        public const string TotalSuccessfulShipmentsKey = "TotalSuccessfulShipments";
        public const string ParallelActionQueueUsed = "ParallelActionQueueUsed";

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
            telemetryClient.Context.Properties.Add("ScreenResolution", $"{Screen.PrimaryScreen.Bounds.Width}x{Screen.PrimaryScreen.Bounds.Height}");
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

            GetCustomerID = () => "Unset";
        }

        /// <summary>
        /// The user id being used by the telemetry client.
        /// </summary>
        public static string UserId => telemetryClient.Context.User.Id;

        /// <summary>
        /// The user id being used by the telemetry client.
        /// </summary>
        public static string SessionId => telemetryClient.Context.Session.Id;

        /// <summary>
        /// Function to retrieve a customer ID
        /// </summary>
        public static Func<string> GetCustomerID { get; set; }

        /// <summary>
        /// Set execution mode
        /// </summary>
        public static void SetExecutionMode(string executionMode)
        {
            telemetryClient.Context.Properties.Add("ExecutionMode", executionMode);
        }

        /// <summary>
        /// Set the instance ID
        /// </summary>
        public static void SetInstance(string instanceID)
        {
            telemetryClient.Context.Device.Id = instanceID;

            if (!telemetryClient.Context.Properties.ContainsKey("Instance"))
            {
                telemetryClient.Context.Properties.Add("Instance", instanceID);
            }
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
        public static void TrackStartShipworks(IDictionary<string, string> additionalProperties)
        {
            SetCustomerID();

            EventTelemetry eventTelemetry = new EventTelemetry("StartShipWorks")
            {
                Timestamp = DateTimeOffset.UtcNow
            };

            Dictionary<string, string> commonProperties = CommonProperties();

            foreach (KeyValuePair<string, string> keyValuePair in commonProperties.Where(kvp => !eventTelemetry.Properties.ContainsKey(kvp.Key)))
            {
                eventTelemetry.Properties.Add(keyValuePair.Key, keyValuePair.Value);
            }

            // Add any additional properties passed
            foreach (KeyValuePair<string, string> keyValuePair in additionalProperties.Where(kvp => !eventTelemetry.Properties.ContainsKey(kvp.Key)))
            {
                eventTelemetry.Properties.Add(keyValuePair.Key, keyValuePair.Value);
            }

            telemetryClient.TrackEvent(eventTelemetry);
        }

        /// <summary>
        /// Set the customer ID on the context
        /// </summary>
        private static void SetCustomerID()
        {
            string tangoCustomerID = GetCustomerID();

            telemetryClient.Context.User.Id = tangoCustomerID;
            telemetryClient.Context.User.AuthenticatedUserId = tangoCustomerID;
        }

        /// <summary>
        /// Track an exception
        /// </summary>
        public static void TrackException(Exception ex, Dictionary<string, string> properties)
        {
            Dictionary<string, string> commonProperties = CommonProperties();

            foreach (KeyValuePair<string, string> keyValuePair in commonProperties.Where(kvp => !properties.ContainsKey(kvp.Key)))
            {
                properties.Add(keyValuePair.Key, keyValuePair.Value);
            }

            SetCustomerID();
            telemetryClient.TrackException(ex, properties);

            // Flush so that this exception gets sent immediately!
            Flush();
        }

        /// <summary>
        /// Track an event
        /// </summary>
        public static void TrackEvent(EventTelemetry eventTelemetry)
        {
            SetCustomerID();
            telemetryClient.TrackEvent(eventTelemetry);
        }

        /// <summary>
        /// Flush any stored telemetry information
        /// </summary>
        public static void Flush()
        {
            telemetryClient.Flush();
        }

        /// <summary>
        /// Gets a Dictionary of common properties to track.
        /// </summary>
        [SuppressMessage("Recommendations", "RECS0022: Empty general catch clause suppresses any error",
            Justification = "Exceptions for telemetry should not crash ShipWorks")]
        private static Dictionary<string, string> CommonProperties()
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();

            // We don't want to cause a crash when using Telemetry, so wrap these in a try/catch
            try
            {
                long memoryInBytes = NativeMethods.GetPhysicallyInstalledSystemMemory();

                Process process = Process.GetCurrentProcess();
                properties.Add("Screens", Screen.AllScreens.Length.ToString());
                properties.Add("CPUs", Environment.ProcessorCount.ToString());
                properties.Add("TransactionManager.DefaultTimeout", TransactionManager.DefaultTimeout.TotalSeconds.ToString(CultureInfo.InvariantCulture));
                properties.Add("TransactionManager.MaximumTimeout", TransactionManager.MaximumTimeout.TotalSeconds.ToString(CultureInfo.InvariantCulture));
                properties.Add("Handles", process.HandleCount.ToString());
                properties.Add("Threads", process.Threads.Count.ToString());
                properties.Add("UserProcessorTime(m)", process.UserProcessorTime.TotalMinutes.ToString(CultureInfo.InvariantCulture));
                properties.Add("TotalProcessorTime(m)", process.TotalProcessorTime.TotalMinutes.ToString(CultureInfo.InvariantCulture));
                properties.Add("PhysicalMemory", StringUtility.FormatByteCount(memoryInBytes, "{0:#,##0}"));
                properties.Add("UserObjects", NativeMethods.GetGuiResources(process.Handle, NativeMethods.GR_USEROBJECTS).ToString());
                properties.Add("GDIObjects", NativeMethods.GetGuiResources(process.Handle, NativeMethods.GR_GDIOBJECTS).ToString());
                properties.Add("ScreenDimensionsPrimary", $"{Screen.PrimaryScreen.Bounds.Width}x{Screen.PrimaryScreen.Bounds.Height}");
                properties.Add("ScreenDpiPrimary", MyComputer.GetSystemDpi());
            }
            catch
            {
            }

            return properties;
        }
    }
}
