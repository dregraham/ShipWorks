using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping
{
    /// <summary>
    /// FedEx create label response
    /// </summary>
    public interface IFedExShipResponse
    {
        /// <summary>
        /// Process the response
        /// </summary>
        void Process();

        /// <summary>
        /// Apply the response manipulators
        /// </summary>
        GenericResult<IFedExShipResponse> ApplyManipulators();
    }
}
