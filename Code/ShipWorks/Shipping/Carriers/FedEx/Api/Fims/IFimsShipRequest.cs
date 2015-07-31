using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Fims
{
    /// <summary>
    /// Represents the data necessary to send to FIMS to process a shipment
    /// </summary>
    public interface IFimsShipRequest
    {
        /// <summary>
        /// The shipment to process
        /// </summary>
        ShipmentEntity Shipment { get; }

        /// <summary>
        /// The FIMS username
        /// </summary>
        string Username { get; }

        /// <summary>
        /// The FIMS password
        /// </summary>
        string Password { get; }
    }
}