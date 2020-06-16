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
        public static void Start(Version targetVersion)
        {
            try
            {
                var data = new AutoUpdateTelemetry();
                data.Started = DateTime.UtcNow;
                data.StartVersion = GetVersionString(Assembly.GetExecutingAssembly().GetName().Version);
                data.TargetVersion = GetVersionString(targetVersion);

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
        private static AutoUpdateTelemetry? Load()
        {
            if(File.Exists(tempPath))
            {
                try
                {
                    string data = File.ReadAllText(tempPath);
                    return JsonConvert.DeserializeObject<AutoUpdateTelemetry>(data);
                }
                catch (Exception ex)
                {
                    return null;
                }
                finally
                {
                    File.Delete(tempPath);
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
        public static void CollectTelemetry()
        {
            var data = Load();
            if (data.HasValue)
            {
                using (var telemetryEvent = new TrackedEvent(""))
                {
                    telemetryEvent.AddProperty("CurrentVersion", data.Value.StartVersion);
                    telemetryEvent.AddProperty("TargetVersion", data.Value.TargetVersion);
                    telemetryEvent.AddProperty("MachineName", Environment.MachineName);
                    telemetryEvent.AddMetric("Duration", DateTime.UtcNow.Subtract(data.Value.Started).TotalMilliseconds);

                    if (Version.TryParse(data.Value.StartVersion, out Version started))
                    {
                        telemetryEvent.AddProperty("IsSuccessful", (Assembly.GetExecutingAssembly().GetName().Version > started).ToString());
                    }
                }
            }
        }
    }
}
