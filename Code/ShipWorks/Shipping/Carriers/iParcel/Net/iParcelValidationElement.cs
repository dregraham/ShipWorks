using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ShipWorks.Shipping.Carriers.iParcel.Net
{
    public class iParcelValidationElement : IiParcelRequestElement
    {
        private readonly iParcelCredentials credentials;

        /// <summary>
        /// Initializes a new instance of the <see cref="iParcelValidationElement" /> class.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        public iParcelValidationElement(iParcelCredentials credentials)
        {
            this.credentials = credentials;
        }

        /// <summary>
        /// Builds the XML element for the validation information.
        /// </summary>
        /// <returns>An XElement object.</returns>
        public virtual XElement Build()
        {
            XElement element = new XElement("Validation",
                                            new XElement("UserName", credentials.Username),
                                            new XElement("Password", credentials.DecryptedPassword),
                                            new XElement("AgreeTerms", "1"),
                                            new XElement("SDNDPLChecked", "1"),
                                            new XElement("ExportLicenseChecked", "1")
                                );
            return element;
        }
    }
}
