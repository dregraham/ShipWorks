using System.Xml;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api.ElementWriters
{
    /// <summary>
    /// A class to write out the "Service" XML element of the package element of a UPS request for 
    /// non-SurePost requests.
    /// </summary>
    public class UpsRateServiceElementWriter
    {
        protected readonly XmlWriter xmlWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsRateServiceElementWriter"/> class.
        /// </summary>
        /// <param name="xmlWriter">The XML writer.</param>
        public UpsRateServiceElementWriter(XmlWriter xmlWriter)
        {
            this.xmlWriter = xmlWriter;
        }

        /// <summary>
        /// Writes the service element to the XML writer.
        /// </summary>
        public virtual void WriteServiceElement()
        {
            // Do nothing in this implementation- we don't need to provide
            // the <Service> node in most cases.
        }
    }
}
