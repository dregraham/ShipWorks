using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api.ElementWriters
{
    /// <summary>
    /// A class to write out the "PackageWeight" XML element of the package element of a UPS rate request for non SurePost
    /// request. Weight unit will be LBS
    /// </summary>
    class UpsRatePackageWeightElementWriter : UpsPackageWeightElementWriter
    {
 
        public UpsRatePackageWeightElementWriter(XmlTextWriter xmlTextWriter) :base(xmlTextWriter)
        {
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

            double weight = UpsUtility.GetPackageTotalWeight(package);

            // write out the UnitOfMeasurement
            xmlWriter.WriteStartElement("UnitOfMeasurement");
            xmlWriter.WriteElementString("Code", "LBS");
            xmlWriter.WriteElementString("Description", "Pounds");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteElementString("Weight", weight.ToString("##0.0"));
            xmlWriter.WriteEndElement();
        }
    }
}
