using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ShipWorks.Shipping.Carriers.iParcel.Net.Ship
{
    public class iParcelShipValidationElement : iParcelValidationElement, IiParcelRequestElement
    {
        private readonly bool isDomestic;

        /// <summary>
        /// Initializes a new instance of the <see cref="iParcelShipValidationElement" /> class.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <param name="isDomestic">if set to <c>true</c> [is domestic].</param>
        /// <param name="useForRates">if set to <c>true</c> [use for rates].</param>
        public iParcelShipValidationElement(iParcelCredentials credentials, bool isDomestic, bool useForRates)
            : base(credentials)
        {
            this.isDomestic = isDomestic;
            UsedForRates = useForRates;
        }

        /// <summary>
        /// Gets a value indicating whether this is being used to obtain rates.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [used for rates]; otherwise, <c>false</c>.
        /// </value>
        public bool UsedForRates { get; private set; }

        /// <summary>
        /// Builds the XML element for the validation information required for a shipping request.
        /// </summary>
        /// <returns>An XElement object.</returns>
        public override XElement Build()
        {
            XElement element = base.Build();
            
            // Add child nodes that are required for shipping
            element.Add
            (
                new XElement("RequestType", UsedForRates ? "TEST" : "LIVE"), 
                new XElement("ReturnInfo", "ALL"), // We want to return all label info
                new XElement("DomesticShipping", isDomestic ? "1" : "0") // i-parcel expects that bool values are 0 or 1
            );

            return element;
        }
    }
}
