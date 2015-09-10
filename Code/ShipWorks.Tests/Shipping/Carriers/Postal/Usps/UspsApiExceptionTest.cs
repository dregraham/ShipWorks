using System.Linq;
using System.Web.Services.Protocols;
using System.Xml;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Usps
{
    public class UspsApiExceptionTest
    {
        [Fact]
        public void Message_WebServiceNamespaceIsRemoved_WhenSoapException()
        {
            string errorMessage = string.Format("An error occurred.  {0}  Something bad.", WebServiceNamespace);
            SoapException soapException = CreateSoapException(errorMessage); 

            UspsApiException testObject = new UspsApiException(soapException);

            string message = testObject.Message;

            Assert.Equal("An error occurred.    Something bad.", message);
        }

        /// <summary>
        /// Get the namespace of the current stamps api
        /// 
        /// This is basically the same code that UspsApiException.Message uses to determine the namespace,
        /// but I wanted to make sure this code doesn't blow up due to a missing attribute, changed method signature,
        /// etc...  So it should get tested at least twice...
        /// </summary>
        private static string WebServiceNamespace
        {
            get
            {
                SoapDocumentMethodAttribute soapDocumentMethodAttribute = UspsWebClient.WebServiceType.
                    GetMethod("AuthenticateUser").
                    GetCustomAttributes(true).
                    OfType<SoapDocumentMethodAttribute>().
                    FirstOrDefault();

                return string.Format("{0}:", soapDocumentMethodAttribute.ResponseNamespace);
            }
        }

        /// <summary>
        /// Creates a soap exception for the UspsApiException for testing.
        /// </summary>
        private static SoapException CreateSoapException(string errorMessage)
        {
            // Build the detail element of the SOAP fault.
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            System.Xml.XmlNode node = doc.CreateNode(XmlNodeType.Element, SoapException.DetailElementName.Name, SoapException.DetailElementName.Namespace);

            // Build specific details for the SoapException.
            // Add first child of detail XML element.
            System.Xml.XmlNode details = doc.CreateNode(XmlNodeType.Element, "Details", "http://tempuri.org/");
            System.Xml.XmlNode detailsChild = doc.CreateNode(XmlNodeType.Element, "FirstChild", "http://tempuri.org/");
            XmlAttribute codeAttribute = doc.CreateAttribute("code");
            
            // This is a value that is not one of the ones we check for, and not the default Express1 code that would cause the message check to be skipped.
            codeAttribute.Value = "00490102";
            details.Attributes.Append(codeAttribute);

            details.AppendChild(detailsChild);

            // Append the two child elements to the detail node.
            node.AppendChild(details);

            //Throw the exception.    
            SoapException se = new SoapException(errorMessage, SoapException.ClientFaultCode, WebServiceNamespace, node);

            return se;
        }
    }
}
