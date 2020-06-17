using System;
using System.IO;
using System.Reflection;
using System.Windows.Ink;
using Interapptive.Shared.Metrics;
using Newtonsoft.Json;

namespace Interapptive.Shared.AutoUpdate
{
    /// <summary>
    /// Collect auto update telemetry
    /// </summary>
    public class AutoUpdateTelemetryCollector
    {
        private static string tempPath = Path.Combine(Path.GetTempPath(), "ShipWorks/UpdateTelemetry.txt");

        /// <summary>
        /// Start collecting telemetry
        /// </summary>
        public static void Start(Version targetVersion, Guid instance)
        {
            try
            {
                var data = new AutoUpdateTelemetry();
                data.Started = DateTime.UtcNow;
                data.StartVersion = GetVersionString(Assembly.GetExecutingAssembly().GetName().Version);
                data.TargetVersion = GetVersionString(targetVersion);
                data.Instance = instance;

                File.WriteAllText(tempPath, JsonConvert.SerializeObject(data));
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Load telemetry from disk
        /// </summary>
        /// <returns></returns>
        private static AutoUpdateTelemetry? Load(Guid instance)
        {
            if(File.Exists(tempPath))
            {
                try
                {
                    string data = File.ReadAllText(tempPath);
                    var telemetry = JsonConvert.DeserializeObject<AutoUpdateTelemetry>(data);
                    if(telemetry.Instance == instance)
                    {
                        File.Delete(tempPath);
                        return telemetry;
                    }
                }
                catch (Exception)
                {
                }
            }
            return null;
        }

        /// <summary>
        /// Get the version string
        /// </summary>
        private static string GetVersionString(Version version) =>
            $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";

        /// <summary>
        /// Collect telemetry
        /// </summary>
        /// <param name="telemetryEvent"></param>
        public static void CollectTelemetry(Guid instance)
        {
            var data = Load(instance);
            if (data.HasValue)
            {
                using (var telemetryEvent = new TrackedEvent("AutoUpdate"))
                {
                    telemetryEvent.AddProperty("CurrentVersion", data.Value.StartVersion);
                    telemetryEvent.AddProperty("TargetVersion", data.Value.TargetVersion);
                    telemetryEvent.AddProperty("MachineName", Environment.MachineName);
                    telemetryEvent.AddMetric("DurationMS", DateTime.UtcNow.Subtract(data.Value.Started).TotalMilliseconds);

                    if (Version.TryParse(data.Value.StartVersion, out Version started))
                    {
                        telemetryEvent.AddProperty("IsSuccessful", (Assembly.GetExecutingAssembly().GetName().Version > started).ToString());
                    }
                }
            }
        }
    }
}
