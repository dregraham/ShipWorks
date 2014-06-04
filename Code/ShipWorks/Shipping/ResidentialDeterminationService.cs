using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx;
using log4net;

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
                            string additionalInfo = string.Empty;

                            if (shipment.ShipCountryCode != "US")
                            {
                                additionalInfo = string.Format("An error while looking up the residential/commercial status. FedEx is very strict about address formats when looking up residential status for non-US addresses. Be sure " +
                                                 "that standard address abbreviations are used.{0}{0}", Environment.NewLine);
                            }

                            throw new FedExAddressValidationException(string.Format("{0}{1}", additionalInfo, ex.Message), ex);
                        }
                    }
            }

            throw new InvalidOperationException("Invalid residential determination type: " + type);
        }
    }
}
