using System;

namespace ShipWorks.Api
{
    /// <summary>
    /// Represents the ShipWorks Api
    /// </summary>
    public interface IApiService: IDisposable
    {
        /// <summary>
        /// Start the Shipworks Api
        /// </summary>
        void Start();
    }
}
