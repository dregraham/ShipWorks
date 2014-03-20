using ShipWorks.Data.Model.EntityClasses;
using System.Xml;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api.ElementWriters
{
    /// <summary>
    /// A class to write out the "PackageServiceOptions" XML element of the package element of a UPS request for 
    /// non-SurePost rate requests.
    /// </summary>
    public class UpsRatePackageServiceOptionsElementWriter : UpsPackageServiceOptionsElementWriter
    {
        /// <summary>
        /// Initializes a new <see cref="UpsRatePackageServiceOptionsElementWriter" /> instance.
        /// </summary>
        /// <param name="xmlWriter">The XML writer.</param>
        public UpsRatePackageServiceOptionsElementWriter(XmlWriter xmlWriter)
            : base(xmlWriter) { }

        /// <summary>
        /// Does nothing; dry ice is not sent for rate requests.
        /// </summary>
        protected override void WriteDryIceElement(UpsPackageEntity package, UpsServicePackageTypeSetting servicePackageSettings)
        {
            // no-op
        }

        /// <summary>
        /// Writes the declared value to the request.  
        /// </summary>
        protected override void WriteDeclaredValue(UpsPackageEntity package, UpsServicePackageTypeSetting servicePackageSettings)
        {
            if (package.DeclaredValue > 0)
            {
                xmlWriter.WriteStartElement("InsuredValue");
                xmlWriter.WriteElementString("MonetaryValue", package.DeclaredValue.ToString("0.00"));
                xmlWriter.WriteEndElement();
            }
        }
    }
}
