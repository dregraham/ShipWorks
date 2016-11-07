﻿using System;
using log4net;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Api;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Provides functionality to determine residential address information for a shipment
    /// </summary>
    [Component]
    public class ResidentialDeterminationService : IResidentialDeterminationService
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ResidentialDeterminationService));

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
                            throw new FedExAddressValidationException(ex.Message, ex);
                        }
                    }

                case ResidentialDeterminationType.FromAddressValidation:
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

            throw new InvalidOperationException("Invalid residential determination type: " + type);
        }
    }
}
