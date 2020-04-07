using System;
using System.IO;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore;
using static Interapptive.Shared.Utility.Functional;
using static Interapptive.Shared.Utility.SerializationUtility;

namespace ShipWorks.Api.Configuration
{
    /// <summary>
    /// ApiSettingsRepository
    /// </summary>
    [Component]
    public class ApiSettingsRepository : IApiSettingsRepository
    {
        private readonly string fullPath;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public ApiSettingsRepository(Func<Type, ILog> logFactory)
        {
            fullPath = Path.Combine(DataPath.InstanceSettings, "apiSettings.xml");
            log = logFactory(typeof(ApiSettingsRepository));
        }

        /// <summary>
        /// Loads api settings from disk
        /// </summary>
        public ApiSettings Load()
        {
            ApiSettings apiSettings = new ApiSettings();

            try
            {
                if (File.Exists(fullPath))
                {
                    var apiSettingsResult = DeserializeXml<ApiSettings>(File.ReadAllText(fullPath));
                    if (apiSettingsResult.Success)
                    {
                        apiSettings = apiSettingsResult.Value;
                    }
                    else
                    {
                        log.Warn($"Couldn't deserialize ApiSettings: '{apiSettingsResult.Message}'");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("An error occurred while trying to read Api Settings.", ex);
            }

            return apiSettings;
        }

        /// <summary>
        /// Saves ApiSettings to disk
        /// </summary>
        /// <param name="settings"></param>
        public void Save(ApiSettings settings)
        {
            MethodConditions.EnsureArgumentIsNotNull(settings, nameof(settings));

            try
            {
                string settingsString = SerializeToXml(settings);
                File.WriteAllText(fullPath, settingsString);
            }
            catch (Exception ex)
            {
                log.Error("An error occurred while trying to load Api Settings.", ex);
                throw;
            }
        }
    }
}
