using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Insurance
{
    /// <summary>
    /// Wrapper interface for InsuranceUtility
    /// </summary>
    public interface IInsuranceUtility
    {
        /// <summary>
        /// Get the cost of insurance for the given shipment given the specified declared value
        /// </summary>
        InsuranceCost GetInsuranceCost(ShipmentEntity shipment, decimal declaredValue);

        /// <summary>
        /// Show the InsurancePennyOneDlg and save values if necessary
        /// </summary>
        void ShowInsurancePennyOneDlg(ShipmentTypeCode shipmentTypeCode);

        /// <summary>
        /// Show the InsuranceBenefitsDlg and save values if necessary
        /// </summary>
        void ShowInsuranceBenefitsDlg(ShipmentEntity shipment, InsuranceCost insuranceCost);

        /// <summary>
        /// Validate the given shipment
        /// </summary>
        void ValidateShipment(ShipmentEntity shipment);
    }
}
