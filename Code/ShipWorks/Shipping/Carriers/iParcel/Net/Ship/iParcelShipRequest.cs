using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using System.Data;

namespace ShipWorks.Shipping.Carriers.iParcel.Net.Ship
{
    /// <summary>
    /// A representation of an i-parcel request used to create a shipment/generate a label through 
    /// the i-parcel web service.
    /// </summary>
    public class iParcelShipRequest : iParcelRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="iParcelShipRequest" /> class.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <param name="shipment">The shipment.</param>
        public iParcelShipRequest(iParcelCredentials credentials, ShipmentEntity shipment)
            : base(credentials, "iParcelShipRequest")
        {
            bool isDomestic = shipment.AdjustedOriginCountryCode().ToUpperInvariant() == shipment.AdjustedShipCountryCode().ToUpperInvariant();

            // Default the validation element to domestic for now
            RequestElements.Add(new iParcelShipValidationElement(credentials, isDomestic, false));
            RequestElements.Add(new iParcelVersionElement());
            RequestElements.Add(new iParcelPackageInfoElement(shipment, new iParcelTokenProcessor(), isDomestic, false));
        }

        /// <summary>
        /// Gets the name of the operation being invoked on the i-parcel system.
        /// </summary>
        /// <value>The name of the operation.</value>
        public override string OperationName
        {
            get { return "SubmitPack"; }
        }

        /// <summary>
        /// Gets the name of the root element for the XML sent in UploadXMLFile method of the
        /// i-parcel web service.
        /// </summary>
        /// <value>The name of the root element.</value>
        public override string RootElementName
        {
            get { return "iparcelPackageUpload"; }
        }        
    }
}
