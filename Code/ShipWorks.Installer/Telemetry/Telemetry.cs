using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using log4net;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using ShipWorks.Installer.Enums;
using ShipWorks.Installer.Models;

namespace ShipWorks.Installer.Telemetry
{
    public static class Telemetry
    {
        private static readonly TelemetryClient telemetryClient;
        private static ILog log;

        /// <summary>
        /// Static constructor
        /// </summary>
        static Telemetry()
        {
            // We don't want a telemetry exception to stop the customer from being
            // able to install, so just log any exception and continue.
            try
            {
                log = LogManager.GetLogger(typeof(Telemetry));

                TelemetryConfiguration configuration = TelemetryConfiguration.CreateDefault();
                configuration.InstrumentationKey = GetInstrumentationKey(Assembly.GetExecutingAssembly().GetName().Version);

                telemetryClient = new TelemetryClient(configuration);
                telemetryClient.Context.Session.Id = Guid.NewGuid().ToString("D");
                telemetryClient.Context.Component.Version = Assembly.GetExecutingAssembly().GetName().Version.ToString(4);
                telemetryClient.Context.Device.OperatingSystem = Environment.OSVersion.ToString();
                telemetryClient.Context.GlobalProperties.Add("ScreenResolution", $"{Screen.PrimaryScreen.Bounds.Width}x{Screen.PrimaryScreen.Bounds.Height}");

                // .NET version
                telemetryClient.Context.GlobalProperties.Add(".NET Version", Environment.Version.ToString());

                // Session
                telemetryClient.Context.GlobalProperties.Add("TerminalServices", SystemInformation.TerminalServerSession ? "Yes" : "No");

                // Log the current culture settings
                telemetryClient.Context.GlobalProperties.Add("CurrentCulture", Thread.CurrentThread.CurrentCulture.ToString());
                telemetryClient.Context.GlobalProperties.Add("CurrentUICulture", Thread.CurrentThread.CurrentUICulture.ToString());
                telemetryClient.Context.GlobalProperties.Add("StartupPath", Application.StartupPath);

                GetCustomerID = () => "Unset";
            }
            catch (Exception ex)
            {
                log.Error("An error occurred during Telemetry static constructor.", ex);
            }
        }

        /// <summary>
        /// Function to retrieve a customer ID
        /// </summary>
        public static Func<string> GetCustomerID { get; set; }
        
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
        public static void TrackStartShipWorksInstaller()
        {
            SetCustomerID();

            EventTelemetry eventTelemetry = new EventTelemetry("StartShipWorksInstaller")
            {
                Timestamp = DateTimeOffset.UtcNow
            };

            Dictionary<string, string> commonProperties = CommonProperties();

            foreach (KeyValuePair<string, string> keyValuePair in commonProperties.Where(kvp => !eventTelemetry.Properties.ContainsKey(kvp.Key)))
            {
                AddProperty(eventTelemetry, keyValuePair.Key, keyValuePair.Value);
            }

            telemetryClient.TrackEvent(eventTelemetry);
        }

        /// <summary>
        /// Track when ShipWorks Installer is finished
        /// </summary>
        public static void TrackFinish(InstallSettings installSettings, NavigationPageType navigationPageType)
        {
            SetCustomerID();

            EventTelemetry eventTelemetry = new EventTelemetry("FinishShipWorksInstaller")
            {
                Timestamp = DateTimeOffset.UtcNow
            };

            AddProperty(eventTelemetry, "LastPage", navigationPageType.ToString());
            AddProperty(eventTelemetry, "OwnDb", installSettings.OwnDb ? "Yes" : "No");
            AddProperty(eventTelemetry, "InstallSucceeded", navigationPageType == NavigationPageType.UseShipWorks && installSettings.Error == InstallError.None ? "Yes" : "No");
            AddProperty(eventTelemetry, "DownloadingShipWorksSucceeded", installSettings.Error == InstallError.DownloadingShipWorks ? "No" : "Yes");

            if (installSettings.Error != InstallError.None)
            {
                AddProperty(eventTelemetry, $"{installSettings.Error}Succeeded", "No");
            }


            //AddProperty(eventTelemetry, "InstallShipWorksSucceeded", installSettings.Error == InstallError.InstallShipWorks ? "No" : "Yes");
            //AddProperty(eventTelemetry, "DatabaseSucceeded", installSettings.Error == InstallError.Database ? "No" : "Yes");
            //AddProperty(eventTelemetry, "SystemCheckSucceeded", installSettings.Error == InstallError.SystemCheck ? "No" : "Yes");
            //AddProperty(eventTelemetry, "UnknownFailure", installSettings.Error == InstallError.Unknown ? "Yes" : "No");

            if (installSettings.Error == InstallError.SystemCheck)
            {
                AddProperty(eventTelemetry, "SystemCheckFailed.CpuMeetsRequirement", installSettings.CheckSystemResult.CpuMeetsRequirement ? "Yes" : "No");
                AddProperty(eventTelemetry, "SystemCheckFailed.HddMeetsRequirement", installSettings.CheckSystemResult.HddMeetsRequirement ? "Yes" : "No");
                AddProperty(eventTelemetry, "SystemCheckFailed.OsMeetsRequirement", installSettings.CheckSystemResult.OsMeetsRequirement ? "Yes" : "No");
                AddProperty(eventTelemetry, "SystemCheckFailed.RamMeetsRequirement", installSettings.CheckSystemResult.RamMeetsRequirement ? "Yes" : "No");
            }

            if (!string.IsNullOrWhiteSpace(installSettings.AutoInstallErrorMessage))
            {
                AddProperty(eventTelemetry, "AutoInstallErrorMessage", installSettings.AutoInstallErrorMessage);
            }

            telemetryClient.TrackEvent(eventTelemetry);
            Flush();
        }

        /// <summary>
        /// Add a property to telemetry and log it to disk.
        /// </summary>
        private static void AddProperty(EventTelemetry eventTelemetry, string propertyName, string value)
        {
            if (eventTelemetry.Properties.ContainsKey(propertyName))
            {
                return;
            }

            eventTelemetry.Properties.Add(propertyName, value);
            log.Info($"Telemetry - {propertyName}:{value}");
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
        public static void TrackException(Exception ex)
        {
            SetCustomerID();
            telemetryClient.TrackException(ex);

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
        /// Track button click
        /// </summary>
        public static void TrackButtonClick(string eventName, string postfix = "")
        {
            postfix = string.IsNullOrWhiteSpace(postfix) ? string.Empty : $".{postfix}".Replace(" ", string.Empty);
            TrackEvent(new EventTelemetry($"ShipWorks.Installer.Button.Click.{eventName}{postfix}"));
        }

        /// <summary>
        /// Flush any stored telemetry information
        /// </summary>
        public static void Flush()
        {
            telemetryClient.Flush();
            Task.Delay(2000).Wait();
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
                Process process = Process.GetCurrentProcess();
                properties.Add("Screens", Screen.AllScreens.Length.ToString());
                properties.Add("CPUs", Environment.ProcessorCount.ToString());
                properties.Add("TransactionManager.DefaultTimeout", TransactionManager.DefaultTimeout.TotalSeconds.ToString(CultureInfo.InvariantCulture));
                properties.Add("TransactionManager.MaximumTimeout", TransactionManager.MaximumTimeout.TotalSeconds.ToString(CultureInfo.InvariantCulture));
                properties.Add("Handles", process.HandleCount.ToString());
                properties.Add("Threads", process.Threads.Count.ToString());
                properties.Add("UserProcessorTime(m)", process.UserProcessorTime.TotalMinutes.ToString(CultureInfo.InvariantCulture));
                properties.Add("TotalProcessorTime(m)", process.TotalProcessorTime.TotalMinutes.ToString(CultureInfo.InvariantCulture));
                properties.Add("ScreenDimensionsPrimary", $"{Screen.PrimaryScreen.Bounds.Width}x{Screen.PrimaryScreen.Bounds.Height}");
            }
            catch
            {
            }

            return properties;
        }
    }
}
