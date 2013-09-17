using System;
using System.Linq;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using System.Xml;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api.ElementWriters
{
    /// <summary>
    /// A class to write out the "PackageWeight" XML element of the package element of a UPS request for a SurePost
    /// request. This will convert the weight to ounces if the service type is SurePost Less than a Pound.
    /// </summary>
    public class UpsSurePostPackageWeightWriter : UpsPackageWeightElementWriter
    {
        private readonly UpsServiceType serviceType;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsSurePostPackageWeightWriter"/> class.
        /// </summary>
        /// <param name="xmlWriter">The XML writer.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <exception cref="System.ArgumentException"></exception>
        public UpsSurePostPackageWeightWriter(XmlWriter xmlWriter, UpsServiceType serviceType)
            : base(xmlWriter)
        {
            if (!UpsUtility.IsUpsSurePostService(serviceType))
            {
                throw new ArgumentException(string.Format("The {0} service type provided is not a SurePost service.", EnumHelper.GetDescription(serviceType)));
            }

            this.serviceType = serviceType;
        }

        /// <summary>
        /// Writes the weight element to the XML writer.
        /// </summary>
        /// <param name="upsShipment">The ups shipment.</param>
        /// <param name="package">The package.</param>
        public override void WriteWeightElement(UpsShipmentEntity upsShipment, UpsPackageEntity package)
        {
            // Package Weight
            xmlWriter.WriteStartElement("PackageWeight");

            // Get the settings for this shipment/package type so we can determine weight unit of measure and declared value setting
            // Before calling this method, the shipment/package type should be validated so that a setting is always found if we make it this far.
            UpsServicePackageTypeSetting upsSetting = UpsServicePackageTypeSetting.ServicePackageValidationSettings
                                                                                  .FirstOrDefault(s => s.ServiceType == serviceType && s.PackageType == (UpsPackagingType)upsShipment.Packages[0].PackagingType);
            if (upsSetting == null)
            {
                // Throw a UPS exception causing the SurePost rate to be by-passed
                string exceptionMessage = string.Format("Shipping a {0} package with {1} is not supported by SurePost.",
                                               EnumHelper.GetDescription((UpsPackagingType)upsShipment.Packages[0].PackagingType),
                                               EnumHelper.GetDescription(serviceType));

                throw new UpsException(exceptionMessage);
            }

            // UPS required the unit of measurement be in OZS if the service type is SurePost less than a pound
            bool useOunces = upsSetting.WeightUnitOfMeasure == WeightUnitOfMeasure.Ounces || serviceType == UpsServiceType.UpsSurePostLessThan1Lb;

            double weight = UpsUtility.GetPackageTotalWeight(package);

            weight = WeightUtility.Convert(WeightUnitOfMeasure.Pounds, upsSetting.WeightUnitOfMeasure, weight);

            // write out the UnitOfMeasurement
            xmlWriter.WriteStartElement("UnitOfMeasurement");
            xmlWriter.WriteElementString("Code", useOunces ? "OZS" : "LBS");
            xmlWriter.WriteElementString("Description", useOunces ? "Ounces" : "Pounds");
            xmlWriter.WriteEndElement();

            if (serviceType == UpsServiceType.UpsSurePostLessThan1Lb)
            {
                // SurePost Less than a Pound requires that the weight be in ounces
                double weightInOunces = WeightUtility.Convert(upsSetting.WeightUnitOfMeasure, WeightUnitOfMeasure.Ounces, weight);
                xmlWriter.WriteElementString("Weight", weightInOunces.ToString());
            }
            else
            {
                xmlWriter.WriteElementString("Weight", weight.ToString("##0.0"));
            }

            xmlWriter.WriteEndElement();
        }
    }
}
