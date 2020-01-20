using System.Threading.Tasks;

namespace ShipWorks.ShipEngine
{
    /// <summary>
    /// Api key for communicating with ShipEngine
    /// </summary>
    public interface IShipEngineApiKey
    {
        /// <summary>
        /// Actual API Key value
        /// </summary>
        string Value { get; }


        /// <summary>
        /// Ensures the ApiKey contains a value ascynchronously
        /// </summary>
        Task ConfigureAsync();

        /// <summary>
        /// Ensures the ApiKey contains a value
        /// </summary>
        void Configure();

        /// <summary>
        /// Get the partner api key
        /// </summary>
        /// <returns></returns>
        string GetPartnerApiKey();
    }
}