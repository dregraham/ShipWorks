using System.Xml;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api
{
    /// <summary>
    /// Response from a UpsWebClient call
    /// </summary>
    public class UpsWebClientResponse
    {
        /// <summary>
        /// The XmlDocument
        /// </summary>
        public XmlDocument XmlDocument;

        /// <summary>
        /// The time spent making the call
        /// </summary>
        public long ResponseTimeInMs;
    }
}
