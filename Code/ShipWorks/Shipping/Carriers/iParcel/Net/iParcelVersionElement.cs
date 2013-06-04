using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ShipWorks.Shipping.Carriers.iParcel.Net
{
    public class iParcelVersionElement : IiParcelRequestElement
    {
        private const string VersionNumber = "3.3";
        
        /// <summary>
        /// Builds the XML element for the version information.
        /// </summary>
        /// <returns>An XElement object.</returns>
        public XElement Build()
        {
            return new XElement("Version", VersionNumber);
        }


    }
}
