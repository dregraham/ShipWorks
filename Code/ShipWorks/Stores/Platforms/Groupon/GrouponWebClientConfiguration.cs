using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Stores.Platforms.Groupon
{
    /// <summary>
    /// Configuration for the Groupon web client
    /// </summary>
    [Component]
    public class GrouponWebClientConfiguration : IGrouponWebClientConfiguration
    {
        /// <summary>
        /// Groupon API endpoint
        /// </summary>
        public string Endpoint => "https://scm.commerceinterface.com/api/v2";
    }
}
