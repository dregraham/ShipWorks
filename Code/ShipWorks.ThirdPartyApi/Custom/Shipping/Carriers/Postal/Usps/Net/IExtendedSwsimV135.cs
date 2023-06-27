using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.WebServices
{
    /// <summary>
    /// ISwsimV135 interface that exposes custom features
    /// </summary>
    public interface IExtendedSwsimV135 : ISwsimV135
    {
        /// <summary>
        /// Cancel an async request
        /// </summary>
        /// <param name="userState">Custom object used to correlate which request to cancel</param>
        void CancelAsync(object userState);

        /// <summary>
        /// Timeout for web requests
        /// </summary>
        int Timeout { get; set; }

        /// <summary>
        /// Cleanse an address with the optional userState object
        /// </summary>
        /// <param name="Item">Authentication information</param>
        /// <param name="Address">Address to cleanse</param>
        /// <param name="FromZIPCode">From zip code</param>
        /// <param name="userState">Custom object which can be used to correlate which request to cancel</param>
        /// <remarks>This is primarily used for canceling as we need the userState parameter
        /// for correleting which request we want to cancel</remarks>
        void CleanseAddressAsync(object Item, Address Address, string FromZIPCode, object userState);
    }
}
