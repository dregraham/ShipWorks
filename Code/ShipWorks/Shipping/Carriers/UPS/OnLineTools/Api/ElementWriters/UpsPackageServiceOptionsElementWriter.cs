﻿using System.Xml;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api.ElementWriters
{
    /// <summary>
    /// A class to write out the "PackageServiceOptions" XML element of the package element of a UPS request for 
    /// non-SurePost requests.
    /// </summary>
    public class UpsPackageServiceOptionsElementWriter
    {
        protected readonly XmlWriter xmlWriter;

        /// <summary>
        /// Prevents a default instance of the <see cref="UpsPackageServiceOptionsElementWriter" /> class from being created.
        /// </summary>
        /// <param name="xmlWriter">The XML writer.</param>
        public UpsPackageServiceOptionsElementWriter(XmlWriter xmlWriter)
        {
            this.xmlWriter = xmlWriter;
        }

        /// <summary>
        /// Writes the service options element to the XML writer provided in the constructor.
        /// </summary>
        /// <param name="upsShipment">The ups shipment.</param>
        /// <param name="package">The package.</param>
        /// <param name="servicePackageSettings">The service package settings.</param>
        public virtual void WriteServiceOptionsElement(UpsShipmentEntity upsShipment, UpsPackageEntity package, UpsServicePackageTypeSetting servicePackageSettings)
        {
            // Service options
            xmlWriter.WriteStartElement("PackageServiceOptions");

            // Delivery confirmation
            if (upsShipment.DeliveryConfirmation != (int)UpsDeliveryConfirmationType.None)
            {
                xmlWriter.WriteStartElement("DeliveryConfirmation");
                xmlWriter.WriteElementString("DCISType", UpsApiCore.GetDeliveryConfirmationCode((UpsDeliveryConfirmationType)upsShipment.DeliveryConfirmation));
                xmlWriter.WriteEndElement();
            }

            // COD
            if (upsShipment.CodEnabled && UpsUtility.IsCodAvailable(upsShipment.Shipment))
            {
                decimal amount = upsShipment.CodAmount / upsShipment.Packages.Count;

                xmlWriter.WriteStartElement("COD");
                xmlWriter.WriteElementString("CODFundsCode", (upsShipment.CodPaymentType == (int)UpsCodPaymentType.Cash) ? "0" : "8");
                xmlWriter.WriteElementString("CODCode", "3");
                xmlWriter.WriteStartElement("CODAmount");
                xmlWriter.WriteElementString("MonetaryValue", amount.ToString("0.00"));
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
            }

            if (package.DeclaredValue > 0 && servicePackageSettings.DeclaredValueAllowed)
            {
                xmlWriter.WriteStartElement("InsuredValue");
                xmlWriter.WriteElementString("MonetaryValue", package.DeclaredValue.ToString("0.00"));
                xmlWriter.WriteEndElement();
            }

            if (upsShipment.ShipperRelease)
            {
                // Element just needs to be present
                xmlWriter.WriteElementString("ShipperReleaseIndicator", string.Empty);
            }

            // End service options
            xmlWriter.WriteEndElement();
        }
    }
}
