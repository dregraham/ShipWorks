namespace ShipWorks.Api
{
    /// <summary>
    /// Represents the Api Port Registration Service
    /// </summary>
    public interface IApiPortRegistrationService
    {
        /// <summary>
        /// Register the given port number
        /// </summary>
        bool Register(long portNumber, bool useHttps);

        /// <summary>
        /// Register the given port running the process as admin
        /// </summary>
        bool RegisterAsAdmin(long portNumber);
    }
}
