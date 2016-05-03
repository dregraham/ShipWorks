using System;
using log4net;

namespace ShipWorks.ApplicationCore.Licensing
{

    /// <summary>
    /// A factory for creating ITangoWebClient instances. This is useful for testing/simulating responses
    /// from Tango before they have been implemented on the Tango side. To safeguard against a custom Tango
    /// web client from being introduced in the wild, a user must be an Interapptive user and also have a 
    /// registry value that contains the resolves to a valid type that implements the ITangoWebClient interface.
    /// </summary>
    public class TangoWebClientFactory : ITangoWebClientFactory
    {
        ILog log = LogManager.GetLogger(typeof(TangoWebClientFactory));
        private const string CustomizedTangoRegistryKeyName = "TangoWebClient";

        /// <summary>
        /// Creates an instance of ITangoWebClient. This allows us to return a mock/fake instance of an 
        /// ITangoWebClient for Interapptive users that have the appropriate registry setting configured;
        /// otherwise the normal web client will be returned.
        /// </summary>
        public ITangoWebClient CreateWebClient()
        {
            ITangoWebClient webClient = new TangoWebClientWrapper();

            if (InterapptiveOnly.IsInterapptiveUser)
            {
                log.Fatal($"IsInterapptiveUser is true.");

                // Check to see if the TangoWebClient key exists and try to use the 
                // web client indicated by the key
                string webClientTypeName = InterapptiveOnly.Registry.GetValue(CustomizedTangoRegistryKeyName, string.Empty);
                log.Fatal($"webClientTypeName:{webClientTypeName}");

                if (!string.IsNullOrWhiteSpace(webClientTypeName))
                {
                    // We have an entry for the custom tango client
                    Type type = Type.GetType(webClientTypeName);

                    if (type != null)
                    {
                        log.Fatal("type is not null");
                        webClient = Activator.CreateInstance(type) as ITangoWebClient;
                    }
                }
            }

            // Fall back to the TangoWebClientWrapper in case we tried to create a customized web client
            // that could not be resolved by the Activator
            return webClient ?? new TangoWebClientWrapper();
        }
    }
}
