using Interapptive.Shared.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ShipWorks.ApplicationCore.Licensing
{
    public class AssociateShipWorksWithItselfResponse
    {
        public AssociateShipWorksWithItselfResponseType ResponseType { get; private set; }
        public string Message { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssociateShipWorksWithItselfResponse"/> class.
        /// </summary>
        public AssociateShipWorksWithItselfResponse(XmlDocument xmlResponse)
        {
            XPathNamespaceNavigator navigator = new XPathNamespaceNavigator(xmlResponse);

            string error = XPathUtility.Evaluate(navigator, "//Error/Code", string.Empty);

            if (string.IsNullOrEmpty(error))
            {
                ResponseType = AssociateShipWorksWithItselfResponseType.Success;
            }
            else if (error=="100")
            {
                ResponseType = AssociateShipWorksWithItselfResponseType.POBoxNotAllowed;
                Message = EnumHelper.GetDescription(ResponseType);
            }
            else
            {
                string errorDescription = XPathUtility.Evaluate(navigator, "//Error/Description", string.Empty);

                ResponseType = AssociateShipWorksWithItselfResponseType.UnknownError;
                Message = $"Unknown error. Message = {errorDescription}";
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssociateShipWorksWithItselfResponse"/> class.
        /// </summary>
        public AssociateShipWorksWithItselfResponse(AssociateShipWorksWithItselfResponseType responseType, string message)
        {
            ResponseType = responseType;
            Message = message;
        }
    }
}
