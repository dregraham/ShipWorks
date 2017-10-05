using System.Threading.Tasks;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// Api key for communicating with ShipEngine
    /// </summary>
    public interface IShipEngineApiKey
    {
        /// <summary>
        /// Ensures the ApiKey contains a value
        /// </summary>
        Task Configure();

        /// <summary>
        /// Actual API Key value
        /// </summary>
        string Value { get; }
    }
}