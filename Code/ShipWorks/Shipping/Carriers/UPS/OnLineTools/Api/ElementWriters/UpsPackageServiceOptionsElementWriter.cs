using System.Xml;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Enums;

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
        /// Initializes a new <see cref="UpsPackageServiceOptionsElementWriter" /> instance.
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

            // Verbal confirmation
            if (package.VerbalConfirmationEnabled && !upsShipment.Shipment.ReturnShipment)
            {
                xmlWriter.WriteStartElement("VerbalConfirmation");
                xmlWriter.WriteStartElement("ContactInfo");

                xmlWriter.WriteElementString("Name", package.VerbalConfirmationName);

                xmlWriter.WriteStartElement("Phone");
                xmlWriter.WriteElementString("Number", package.VerbalConfirmationPhone);
                xmlWriter.WriteElementString("Extension", package.VerbalConfirmationPhoneExtension);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
            }

            // Dry ice
            if (package.DryIceEnabled)
            {
                WriteDryIceElement(package, servicePackageSettings);
            }

            // End service options
            xmlWriter.WriteEndElement();
        }

        /// <summary>
        /// Writes the dry ice element and its children.
        /// </summary>
        /// <param name="package">The package.</param>
        /// <param name="servicePackageSettings">The service package settings.</param>
        protected virtual void WriteDryIceElement(UpsPackageEntity package, UpsServicePackageTypeSetting servicePackageSettings)
        {
            double dryIceWeightInLbs = WeightUtility.Convert(servicePackageSettings.WeightUnitOfMeasure, WeightUnitOfMeasure.Pounds, package.DryIceWeight);

            xmlWriter.WriteStartElement("DryIce");
            xmlWriter.WriteElementString("RegulationSet", EnumHelper.GetApiValue((UpsDryIceRegulationSet)package.DryIceRegulationSet));

            xmlWriter.WriteStartElement("DryIceWeight");
            xmlWriter.WriteStartElement("UnitOfMeasurement");
            xmlWriter.WriteElementString("Code", "LBS");
            xmlWriter.WriteEndElement();
            xmlWriter.WriteElementString("Weight", dryIceWeightInLbs.ToString("##0.0"));
            xmlWriter.WriteEndElement();

            if (package.DryIceRegulationSet == (int)UpsDryIceRegulationSet.Cfr && package.DryIceIsForMedicalUse)
            {
                xmlWriter.WriteElementString("MedicalUseIndicator", null);
            }

            xmlWriter.WriteEndElement();
        }
    }
}
