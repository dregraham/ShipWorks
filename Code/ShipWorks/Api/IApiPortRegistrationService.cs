using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Api
{
    /// <summary>
    /// Represents the Api Port Registration Service
    /// </summary>
    [Service]
    public interface IApiPortRegistrationService
    {
        /// <summary>
        /// Register the given port number
        /// </summary>
        bool Register(long portNumber);
    }
}
