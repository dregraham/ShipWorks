using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ShipWorks.Shipping.Carriers.iParcel.Net
{
    public interface IiParcelRequestElement
    {
        /// <summary>
        /// Builds the XML element containing the corresponding element required in an i-parcel request.
        /// </summary>
        /// <returns>An XElement object.</returns>
        XElement Build();
    }
}
