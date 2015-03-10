using System;
using System.Text.RegularExpressions;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Other;

namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// Gets a description of the carrier selected in the shipment
    /// </summary>
    public class CarrierDescription
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CarrierDescription(ShipmentEntity shipment) :
            this(shipment, EnsureOtherShipmentIsLoaded)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CarrierDescription(ShipmentEntity shipment, Action<ShipmentEntity> ensureOtherIsLoaded)
        {
            if (ensureOtherIsLoaded == null)
            {
                throw new ArgumentNullException("ensureOtherIsLoaded");
            }

            ensureOtherIsLoaded(shipment);

            BuildCarrierDetails(shipment.Other.Carrier);   
        }

        /// <summary>
        /// Is the shipment USPS?
        /// </summary>
        public bool IsUSPS { get; private set; }

        /// <summary>
        /// Is the shipment UPS?
        /// </summary>
        public bool IsUPS { get; private set; }

        /// <summary>
        /// Is the shipment FedEx?
        /// </summary>
        public bool IsFedEx { get; private set; }

        /// <summary>
        /// Is the shipment DHL?
        /// </summary>
        public bool IsDHL { get; private set; }

        /// <summary>
        /// Name of the shipment, normalized if possible
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Get the carrier name for the given free text carrier name.  i.e. "U S P S", "Fed Ex", "FedEx"
        /// </summary>
        /// <returns>
        /// If freeTextCarrierName can be parsed, "UPS", "USPS", or "FedEx" will be returned.
        /// Otherwise, freeTextCarrierName will be returned.
        /// </returns>
        private void BuildCarrierDetails(string freeTextCarrierName)
        {
            // Strip out any characters that aren't in UPS, FedEx, or USPS
            string parsedCarrierName = Regex.Replace(freeTextCarrierName, "[^fedxupsdhlFEDXUPSDHL]", "");

            // See if this is UPS, USPS, or FedEx
            if (parsedCarrierName.IndexOf("ups", 0, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                IsUPS = true;
                Name = "UPS";
            }
            else if (parsedCarrierName.IndexOf("usps", 0, StringComparison.OrdinalIgnoreCase) >= 0
                 || freeTextCarrierName.IndexOf("postal", 0, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                IsUSPS = true;
                Name = "USPS";
            }
            else if (parsedCarrierName.IndexOf("fedex", 0, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                IsFedEx = true;
                Name = "FedEx";
            }
            else if (parsedCarrierName.IndexOf("dhl", 0, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                IsDHL = true;
                Name = "DHL";
            }
            else
            {
                Name = freeTextCarrierName;
            }
        }

        /// <summary>
        /// Ensure the 'Other' shipment data is loaded
        /// </summary>
        private static void EnsureOtherShipmentIsLoaded(ShipmentEntity shipment)
        {
            if (shipment.Other != null)
            {
                return;
            }

            OtherShipmentType otherShipmentType = new OtherShipmentType();
            otherShipmentType.LoadShipmentData(shipment, false);
        }
    }
}
