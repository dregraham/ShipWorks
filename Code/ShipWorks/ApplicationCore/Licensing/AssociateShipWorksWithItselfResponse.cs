using Interapptive.Shared.Utility;
using System.Xml;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Response from AssociateShipWorksWithItselfRequest
    /// </summary>
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
            else if (error == EnumHelper.GetApiValue(AssociateShipWorksWithItselfResponseType.POBoxNotAllowed))
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
