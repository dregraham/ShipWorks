using System.Linq;
using System.Xml;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api.ElementWriters
{
    /// <summary>
    /// A class to write out the "PackageWeight" XML element of the package element of a UPS request for 
    /// non-SurePost requests
    /// </summary>
    public class UpsPackageWeightElementWriter
    {
        protected readonly XmlWriter xmlWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsPackageWeightElementWriter"/> class.
        /// </summary>
        /// <param name="xmlWriter">The XML writer.</param>
        public UpsPackageWeightElementWriter(XmlWriter xmlWriter)
        {
            this.xmlWriter = xmlWriter;
        }

        /// <summary>
        /// Writes the weight element to the XML writer.
        /// </summary>
        /// <param name="upsShipment">The ups shipment.</param>
        /// <param name="package">The package.</param>
        public virtual void WriteWeightElement(UpsShipmentEntity upsShipment, UpsPackageEntity package)
        {
            // Package Weight
            xmlWriter.WriteStartElement("PackageWeight");

            double weight = UpsUtility.GetPackageTotalWeight(package);
            // Get the settings for this shipment/package type so we can determine weight unit of measure and declared value setting
            // Before calling this method, the shipment/package type should be validated so that a setting is always found if we make it this far.
            UpsServicePackageTypeSetting upsSetting = UpsServicePackageTypeSetting.ServicePackageValidationSettings
                                                                                  .FirstOrDefault(s => s.ServiceType == (UpsServiceType) upsShipment.Service && s.PackageType == (UpsPackagingType) upsShipment.Packages[0].PackagingType);

            weight = WeightUtility.Convert(WeightUnitOfMeasure.Pounds, upsSetting.WeightUnitOfMeasure, weight);

            // write out the UnitOfMeasurement
            xmlWriter.WriteStartElement("UnitOfMeasurement");
            xmlWriter.WriteElementString("Code", upsSetting.WeightUnitOfMeasure == WeightUnitOfMeasure.Pounds ? "LBS" : "OZS");
            xmlWriter.WriteElementString("Description", upsSetting.WeightUnitOfMeasure == WeightUnitOfMeasure.Pounds ? "Pounds" : "Ounces");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteElementString("Weight", weight.ToString("##0.##"));
            xmlWriter.WriteEndElement();
        }
    }
}
