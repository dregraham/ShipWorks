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
        private readonly ILog log;
        private string fullPath;

        /// <summary>
        /// Constructor
        /// </summary>
        public ApiSettingsRepository(Func<Type, ILog> logFactory)
        {
            log = logFactory(typeof(ApiSettingsRepository));
        }

        /// <summary>
        /// Full path
        /// </summary>
        private string FullPath
        {
            get
            {
                if (fullPath.IsNullOrWhiteSpace())
                {
                    try
                    {
                        fullPath = Path.Combine(DataPath.InstanceSettings, "apiSettings.xml");
                    }
                    catch
                    {
                        // Just continue.  DataPath isn't initialized during integration tests and this was throwing.
                    }
                }

                return fullPath;
            }
        }

        /// <summary>
        /// Loads api settings from disk
        /// </summary>
        public ApiSettings Load()
        {
            ApiSettings apiSettings = new ApiSettings();

            try
            {
                if (File.Exists(FullPath))
                {
                    var apiSettingsResult = DeserializeXml<ApiSettings>(File.ReadAllText(FullPath));
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
                File.WriteAllText(FullPath, settingsString);
            }
            catch (Exception ex)
            {
                log.Error("An error occurred while trying to load Api Settings.", ex);
                throw;
            }
        }
    }
}
