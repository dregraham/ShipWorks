using ShipWorks.ApplicationCore.Licensing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing
{
    public class AssociateShipworksWithItselfResponseTest
    {
        [Fact]
        public void Constructor_ResponseTypeHasUnknownError()
        {
            var xml = new XmlDocument();
            xml.LoadXml(@"<?xml version='1.0'?>
                <AssociateShipWorksWithItselfActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
                  <Error>
                    <Code>42</Code>
                    <Description>UnknownError</Description>
                  </Error>
                </AssociateShipWorksWithItselfActivityResponse>");
            var response = new AssociateShipWorksWithItselfResponse(xml);

            Assert.Equal(AssociateShipWorksWithItselfResponseType.UnknownError, response.ResponseType);
        }

        [Fact]
        public void Constructor_ResponseTypeHasPOBoxError()
        {
            var xml = new XmlDocument();
            xml.LoadXml(@"<?xml version='1.0'?>
                <AssociateShipWorksWithItselfActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
                  <Error>
                    <Code>100</Code>
                    <Description>po box error</Description>
                  </Error>
                </AssociateShipWorksWithItselfActivityResponse>");
            var response = new AssociateShipWorksWithItselfResponse(xml);

            Assert.Equal(AssociateShipWorksWithItselfResponseType.POBoxNotAllowed, response.ResponseType);
        }

        [Fact]
        public void Constructor_ResponseTypeIsSuccess()
        {
            var xml = new XmlDocument();
            xml.LoadXml(@"<?xml version='1.0'?>
                <AssociateShipWorksWithItselfActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
                </AssociateShipWorksWithItselfActivityResponse>");
            var response = new AssociateShipWorksWithItselfResponse(xml);

            Assert.Equal(AssociateShipWorksWithItselfResponseType.Success, response.ResponseType);
        }
    }
}
