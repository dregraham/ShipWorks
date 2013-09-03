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
    }
}
