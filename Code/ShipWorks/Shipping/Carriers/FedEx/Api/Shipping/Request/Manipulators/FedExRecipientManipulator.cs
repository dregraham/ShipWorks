using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Stores.Platforms.Amazon.WebServices.Associates;
using Address = ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.Address;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    /// <summary>
    /// Manipulator for adding recipient information to the FedEx request
    /// </summary>
    public class FedExRecipientManipulator : FedExShippingRequestManipulatorBase
    {
        private const int maxAddressLength = 35;
        private const int maxSmartPostLength = 30;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExRecipientManipulator" /> class.
        /// </summary>
        public FedExRecipientManipulator()
            : this(new FedExSettings(new FedExSettingsRepository()))
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExRecipientManipulator" /> class.
        /// </summary>
        /// <param name="fedExSettings">The fed ex settings.</param>
        public FedExRecipientManipulator(FedExSettings fedExSettings)
            : base(fedExSettings)
        {
        }

        /// <summary>
        /// Add the Recipient info to the FedEx carrier request
        /// </summary>
        /// <param name="request">The FedEx carrier request</param>
        public override void Manipulate(CarrierRequest request)
        {
            // Get the RequestedShipment object for the request
            RequestedShipment requestedShipment = FedExRequestManipulatorUtilities.GetShipServiceRequestedShipment(request);

            // Get the contact and address for the shipment
            ShipmentEntity shipment = request.ShipmentEntity;
            Contact contact = FedExRequestManipulatorUtilities.CreateContact<Contact>(new PersonAdapter(shipment, "Ship"));
            Address address = FedExRequestManipulatorUtilities.CreateAddress<Address>(new PersonAdapter(shipment, "Ship"));

            if (address.CountryCode.Equals("PR", StringComparison.OrdinalIgnoreCase))
            {
                address.StateOrProvinceCode = "PR";
            }

            if (FedExRequestManipulatorUtilities.IsGuam(address.CountryCode) || FedExRequestManipulatorUtilities.IsGuam(address.StateOrProvinceCode))
            {
                address.StateOrProvinceCode = string.Empty;
                address.CountryCode = "GU";
            }

            // FedEx cannot display more than 50 characters per line of address
            if (address.StreetLines[0].Length > maxAddressLength)
            {
                WrapAddress(address, shipment);
            }

            // Create the shipper
            Party recipient = new Party()
            {
                Contact = contact,
                Address = address
            };

            // Set residential info
            if (ShipmentTypeManager.GetType(ShipmentTypeCode.FedEx).IsResidentialStatusRequired(shipment))
            {
                recipient.Address.Residential = shipment.ResidentialResult;
                recipient.Address.ResidentialSpecified = true;
            }

            // Set the recipient on the requested shipment
            if (shipment.ReturnShipment)
            {
                requestedShipment.Shipper = recipient;
            }
            else
            {
                requestedShipment.Recipient = recipient;                
            }
        }

        /// <summary>
        /// Wraps the address to line 2 if first line isn't more than 35 characters and line 2 is currently empty
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="shipment"></param>
        private static void WrapAddress(Address address, ShipmentEntity shipment)
        {
            int calculatedMathLenght = ((FedExServiceType) shipment.FedEx.Service == FedExServiceType.SmartPost)
                ? maxSmartPostLength
                : maxAddressLength;

            // If only one address line is entered, wrap to the second line
            if (address.StreetLines.Length == 1 && address.StreetLines[0].Length > calculatedMathLenght)
            {
                StringBuilder newLine1 = new StringBuilder();
                StringBuilder newLine2 = new StringBuilder();
                bool line1Full = false;

                string[] addressWords = address.StreetLines[0].Split(' ');
                
                foreach (string line1Word in addressWords)
                {
                    if (!line1Full)
                    {
                        // line1 is full if the current length plus the new word plus a space is over maxAddressLength
                        line1Full = (newLine1.Length + line1Word.Length + 1) > calculatedMathLenght;
                    }

                    if (line1Full)
                    {
                        newLine2.AppendFormat(newLine2.Length == 0 ? "{0}" : " {0}", line1Word);
                    }
                    else
                    {
                        newLine1.AppendFormat(newLine1.Length == 0 ? "{0}" : " {0}", line1Word);
                    }
                }

                // if both lines are under max length, use them. If not, don't wrap.
                if (newLine1.Length <= calculatedMathLenght && newLine2.Length <= calculatedMathLenght)
                {
                    address.StreetLines = new[] { newLine1.ToString(), newLine2.ToString() };                    
                }
            }
        }
    }
}
