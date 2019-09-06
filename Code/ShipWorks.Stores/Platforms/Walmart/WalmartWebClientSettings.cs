using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Stores.Platforms.Walmart
{
    /// <summary>
    /// Class for handling Walmart API Related Settings
    /// </summary>
    [Component]
    public class WalmartWebClientSettings : IWalmartWebClientSettings
    {
        private const string LiveEndpoint = "https://marketplace.walmartapis.com";
        private readonly IInterapptiveOnly interapptiveOnly;

        /// <summary>
        /// Constructor
        /// </summary>
        public WalmartWebClientSettings(IInterapptiveOnly interapptiveOnly)
        {
            this.interapptiveOnly = interapptiveOnly;
        }

        /// <summary>
        /// Root endpoint
        /// </summary>
        public string Endpoint
        {
            get
            {
                if (interapptiveOnly.IsInterapptiveUser)
                {
                    var useLiveEndpoint = interapptiveOnly.Registry.GetValue("WalmartLive", true);
                    var endpointOverride = interapptiveOnly.Registry.GetValue("WalmartEndpoint", string.Empty);

                    if (!useLiveEndpoint && !string.IsNullOrWhiteSpace(endpointOverride))
                    {
                        return endpointOverride;
                    }

                    return LiveEndpoint;
                }

                return LiveEndpoint;
            }
        }
    }
}
