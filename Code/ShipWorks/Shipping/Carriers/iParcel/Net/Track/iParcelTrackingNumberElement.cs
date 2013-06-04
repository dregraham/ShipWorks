using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.iParcel.Net.Track
{
    public class iParcelTrackingNumberElement : IiParcelRequestElement
    {
        private readonly IParcelPackageEntity package;

        /// <summary>
        /// Initializes a new instance of the <see cref="iParcelTrackingNumberElement" /> class.
        /// </summary>
        /// <param name="package">The package.</param>
        public iParcelTrackingNumberElement(IParcelPackageEntity package)
        {
            this.package = package;
        }

        /// <summary>
        /// Builds the XML element containing the corresponding element required in an i-parcel request.
        /// </summary>
        /// <returns>
        /// An XElement object.
        /// </returns>
        public XElement Build()
        {
            return new XElement("TrackingNumber", package.TrackingNumber);
        }
    }
}
