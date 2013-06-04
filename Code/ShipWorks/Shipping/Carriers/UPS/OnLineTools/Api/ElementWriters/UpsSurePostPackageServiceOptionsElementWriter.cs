using System.Xml;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api.ElementWriters
{
    /// <summary>
    /// A class to write out the "PackageServiceOptions" XML element of the package element of a UPS request for 
    /// a SurePost request. This is primarily intended for the rate request as these requests should not include
    /// any service options.
    /// </summary>
    public class UpsSurePostPackageServiceOptionsElementWriter : UpsPackageServiceOptionsElementWriter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpsSurePostPackageServiceOptionsElementWriter"/> class.
        /// </summary>
        /// <param name="xmlWriter">The XML writer.</param>
        public UpsSurePostPackageServiceOptionsElementWriter(XmlWriter xmlWriter)
            : base(xmlWriter)
        { }

        /// <summary>
        /// Writes the service options element to the XML writer provided in the constructor.
        /// </summary>
        /// <param name="upsShipment">The ups shipment.</param>
        /// <param name="package">The package.</param>
        /// <param name="servicePackageSettings">The service package settings.</param>
        public override void WriteServiceOptionsElement(UpsShipmentEntity upsShipment, UpsPackageEntity package, UpsServicePackageTypeSetting servicePackageSettings)
        {
            xmlWriter.WriteStartElement("PackageServiceOptions");
            xmlWriter.WriteEndElement();
        }
    }
}
