using System;
using ShipWorks.AddressValidation.Enums;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Provides functionality to determine residential address information for a shipment
    /// </summary>
    [Component]
    public class ResidentialDeterminationService : IResidentialDeterminationService
    {
        /// <summary>
        /// Uses the address and shipment config to determine what the residential status flag should be set to.
        /// </summary>
        public bool IsResidentialAddress(ShipmentEntity shipment) => DetermineResidentialAddress(shipment);

        /// <summary>
        /// Uses the address and shipment config to determine what the residential status flag should be set to.
        /// </summary>
        public static bool DetermineResidentialAddress(ShipmentEntity shipment)
        {
            ResidentialDeterminationType type = (ResidentialDeterminationType) shipment.ResidentialDetermination;

            switch (type)
            {
                case ResidentialDeterminationType.Residential:
                    return true;

                case ResidentialDeterminationType.Commercial:
                    return false;

                case ResidentialDeterminationType.CommercialIfCompany:
                    return string.IsNullOrEmpty(shipment.ShipCompany);

                case ResidentialDeterminationType.FromAddressValidation:
                    return AddressValidationResidentialDetermination(shipment);
            }

            throw new InvalidOperationException("Invalid residential determination type: " + type);
        }


        /// <summary>
        /// Determine the address residential status based on address validation
        /// fall back to the old method if address validation failed
        /// </summary>
        private static bool AddressValidationResidentialDetermination(ShipmentEntity shipment)
        {
            switch (shipment.ShipResidentialStatus)
            {
                case (int) ValidationDetailStatusType.Yes:
                    return true;
                case (int) ValidationDetailStatusType.No:
                    return false;
                default:
                    // Just fall back on testing whether the company is set to determine if the address is commercial
                    return string.IsNullOrEmpty(shipment.ShipCompany);
            }
        }
    }
}
