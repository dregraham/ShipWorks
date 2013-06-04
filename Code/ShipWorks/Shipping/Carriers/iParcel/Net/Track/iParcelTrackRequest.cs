using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.iParcel.Net.Track
{
    /// <summary>
    /// A representation of an i-parcel request used to obtain tracking information through the i-parcel web service.
    /// </summary>
    public class iParcelTrackRequest : iParcelRequest
    {
        private readonly IParcelPackageEntity package;

        /// <summary>
        /// Initializes a new instance of the <see cref="iParcelTrackRequest" /> class.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <param name="package">The package.</param>
        public iParcelTrackRequest(iParcelCredentials credentials, IParcelPackageEntity package)
            : base(credentials, "iParcelTrackRequest")
        {
            this.package = package;

            RequestElements.Add(new iParcelTrackingNumberElement(package));
        }

        /// <summary>
        /// Gets the name of the operation being invoked on the i-parcel system.
        /// </summary>
        /// <value>The name of the operation.</value>
        public override string OperationName
        {
            get { return "Track"; }
        }

        /// <summary>
        /// Gets the name of the root element for the XML sent in UploadXMLFile method of the
        /// i-parcel web service.
        /// </summary>
        /// <value>The name of the root element.</value>
        public override string RootElementName
        {
            get { return "iparcelTrackingRequest"; }
        }
    }
}
