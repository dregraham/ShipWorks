using System;
using Interapptive.Shared.Business;
using ShipWorks.AddressValidation;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx;
using log4net;
using ShipWorks.AddressValidation.Enums;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Provides functionality to determine residential address information for a shipment
    /// </summary>
    public static class ResidentialDeterminationService
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ResidentialDeterminationService));

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

                case ResidentialDeterminationType.FedExAddressLookup:
                    {
                        if (shipment.ShipmentType != (int) ShipmentTypeCode.FedEx)
                        {
                            throw new ShippingException("Cannot use FedEx address lookup for a non FedEx shipment.");
                        }

                        try
                        {
                            bool result = FedExApiAddressValidation.IsResidentialAddress(shipment);

                            log.InfoFormat("Shipment {0}  - FedEx address lookup ({1})", shipment.ShipmentID, result);

                            return result;
                        }
                        catch (FedExException ex)
                        {
                            // If there are no FedEx accounts the user must be trying to get counter rates
                            // grab the residential status using the old method. If they choose to add a FedEx
                            // account we will get the residential status from the account they add.
                            if (ex.Message == "No FedEx account is selected for the shipment.")
                            {
                                return AddressValidationResidentialDetermination(shipment);
                            }

                            throw new FedExAddressValidationException(ex.Message, ex);
                        }
                    }

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
