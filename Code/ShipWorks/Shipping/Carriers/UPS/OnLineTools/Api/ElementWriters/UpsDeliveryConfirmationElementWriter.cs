using System.Xml;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api.ElementWriters
{
    /// <summary>
    /// A class to write out the "DeliveryConfirmation" XML element of the shipment or the package 
    /// </summary>
    public class UpsDeliveryConfirmationElementWriter
    {
        private readonly XmlWriter xmlWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsDeliveryConfirmationElementWriter"/> class.
        /// </summary>
        /// <param name="xmlWriter">The XML writer.</param>
        public UpsDeliveryConfirmationElementWriter(XmlWriter xmlWriter)
        {
            this.xmlWriter = xmlWriter;
        }

        /// <summary>
        /// Writes the delivery confirmation element at the package level, if valid, to the XML writer.
        /// </summary>
        public void WritePackageDeliveryConfirmationElement(UpsShipmentEntity upsShipment)
        {
            // From UPS Tech Support:  Delivery confirmation can be on the shipment or package level as follows:
            // ShipmentServiceOptions - international shipment (Canada to US) or (US to Canada) => OriginCountryCode != ShipCountryCode
            // PackageServiceOptions - domestic shipment (Canada to Canada) or (US to US)       => OriginCountryCode == ShipCountryCode
            // /ShipmentConfirmRequest/Shipment/Package/PackageServiceOptions/DeliveryConfirmation/DCISType
            // Valid values are:
            // 1 - Delivery Confirmation
            // 2 - Delivery Confirmation Signature Required 
            // 3 - Delivery Confirmation Adult Signature Required
            // 4 - USPS Delivery Confirmation
            if (upsShipment.Shipment.AdjustedOriginCountryCode() == upsShipment.Shipment.AdjustedShipCountryCode())
            {
                // Delivery confirmation
                if (upsShipment.DeliveryConfirmation != (int)UpsDeliveryConfirmationType.None)
                {
                    xmlWriter.WriteStartElement("DeliveryConfirmation");
                    xmlWriter.WriteElementString("DCISType", UpsApiCore.GetPackageLevelDeliveryConfirmationCode((UpsDeliveryConfirmationType)upsShipment.DeliveryConfirmation));
                    xmlWriter.WriteEndElement();
                }
            }
        }

        /// <summary>
        /// Writes the delivery confirmation element at the shipment level, if valid, to the XML writer.
        /// </summary>
        public void WriteShipmentDeliveryConfirmationElement(UpsShipmentEntity upsShipment)
        {
            // From UPS Tech Support:  Delivery confirmation can be on the shipment or package level as follows:
            // ShipmentServiceOptions - international shipment (Canada to US) or (US to Canada) => OriginCountryCode != ShipCountryCode
            // PackageServiceOptions - domestic shipment (Canada to Canada) or (US to US)       => OriginCountryCode == ShipCountryCode
            // Valid values are: 
            // 1 - Delivery Confirmation Signature Required 
            // 2 - Delivery Confirmation Adult Signature Required
            if (upsShipment.Shipment.AdjustedOriginCountryCode() != upsShipment.Shipment.AdjustedShipCountryCode())
            {
                // Delivery confirmation
                UpsDeliveryConfirmationType upsDeliveryConfirmationType = (UpsDeliveryConfirmationType)upsShipment.DeliveryConfirmation;
                if (upsDeliveryConfirmationType == UpsDeliveryConfirmationType.AdultSignature || upsDeliveryConfirmationType == UpsDeliveryConfirmationType.Signature)
                {
                    xmlWriter.WriteStartElement("DeliveryConfirmation");
                    xmlWriter.WriteElementString("DCISType", UpsApiCore.GetShipmentLevelDeliveryConfirmationCode((UpsDeliveryConfirmationType)upsShipment.DeliveryConfirmation));
                    xmlWriter.WriteEndElement();
                }
            }
        }
    }
}
