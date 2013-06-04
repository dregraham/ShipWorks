using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Xml.Linq;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore.Logging;
using System.Linq;

namespace ShipWorks.Shipping.Carriers.iParcel.Net.Authentication
{
    /// <summary>
    /// Authenticates credentials against iParcel
    /// </summary>
    public class iParcelAuthenticationRequest : iParcelRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="iParcelAuthenticationRequest" /> class.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        public iParcelAuthenticationRequest(iParcelCredentials credentials)
            : base(credentials, "iParcelAuthenticationRequest")
        {
            RequestElements.Add(new iParcelValidationElement(Credentials));
            RequestElements.Add(new iParcelVersionElement());
            RequestElements.Add(new iParcelIPAddressElement());
        }

        /// <summary>
        /// Gets the name of the operation being invoked on the i-parcel system. In this case,
        /// we're using the Geocode operation to test the credentials.
        /// </summary>
        /// <value>The name of the operation.</value>
        public override string OperationName
        {
            get { return "Geocode"; }
        }

        /// <summary>
        /// Gets the name of the root element for the XML sent in UploadXMLFile method of the 
        /// i-parcel web service.
        /// </summary>
        /// <value>The name of the root element.</value>
        public override string RootElementName
        {
            get { return "iparcelGeoCodeRequest"; }
        }

        /// <summary>
        /// Check the given response for a null response or and empty data set
        /// </summary>
        /// <param name="response">The response from iParcel.</param>
        /// <exception cref="iParcelException">No response was returned from i-Parcel.</exception>
        protected override void CheckForErrors(DataSet response)
        {
            // We don't perform the check for error info here, because we need
            // to check it in the IsValidUser method
            bool hasErrors = response == null || response.Tables.Count == 0 && response.Tables[0].Rows.Count == 0;

            if (hasErrors)
            {
                throw new iParcelException("No response was returned from i-Parcel.");
            }
        }
    }
}