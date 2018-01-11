using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Terms and Conditions
    /// </summary>
    public interface IUspsTermsAndConditions
    {
        /// <summary>
        /// Validates that user has accepted the terms and conditions
        /// </summary>
        void Validate(IShipmentEntity shipment);

        /// <summary>
        /// Create a dialog that will allow a customer to accept the terms and conditions for USPS
        /// </summary>
        void Show();
    }
}