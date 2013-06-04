using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml.Linq;
using Interapptive.Shared.Net;
using log4net;

namespace ShipWorks.Shipping.Carriers.iParcel.Net.Authentication
{
    public class iParcelIPAddressElement : IiParcelRequestElement
    {
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="iParcelIPAddressElement" /> class.
        /// </summary>
        public iParcelIPAddressElement()
            : this(LogManager.GetLogger(typeof(iParcelIPAddressElement)))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="iParcelIPAddressElement" /> class.
        /// </summary>
        /// <param name="logger">The logger to use.</param>
        public iParcelIPAddressElement(ILog logger)
        {
            log = logger;
        }

        /// <summary>
        /// Builds the XML element containing the corresponding element required in an i-parcel request.
        /// </summary>
        /// <returns>An XElement object.</returns>
        public XElement Build()
        {
            try
            {
                IPAddress[] ipAddresses = Dns.GetHostAddresses("www.shipworks.com");
                string ipAddress = ipAddresses[0].ToString();

                return new XElement("IPAddresses", new XElement("IPAddress", ipAddress));
            }
            catch (Exception ex)
            {
                log.Error("Error in GetIPAddress", ex);
                throw WebHelper.TranslateWebException(ex, typeof(iParcelException));
            }
        }

    }
}
