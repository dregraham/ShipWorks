using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using Xunit;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.WebServices.Ship
{
    public class FedExWebServiceShipReferenceTest
    {

        private Type contactType;
        private Type addressType;

        public FedExWebServiceShipReferenceTest()
        {
            contactType = typeof(Contact);
            addressType = typeof(Address);
        }

        [Fact]
        public void Contact_ContactIdHasOrderElementTest()
        {
            GetXmlAttributeOrder(contactType, "ContactId", 0);
        }

        [Fact]
        public void Contact_PersonNameHasOrderElementTest()
        {
            GetXmlAttributeOrder(contactType, "PersonName", 1);
        }

        [Fact]
        public void Contact_TitleHasOrderElementTest()
        {
            GetXmlAttributeOrder(contactType, "Title", 2);
        }

        [Fact]
        public void Contact_CompanyNameHasOrderElementTest()
        {
            GetXmlAttributeOrder(contactType, "CompanyName", 3);
        }

        [Fact]
        public void Contact_PhoneNumberHasOrderElementTest()
        {
            GetXmlAttributeOrder(contactType, "PhoneNumber", 4);
        }

        [Fact]
        public void Contact_PhoneExtensionHasOrderElementTest()
        {
            GetXmlAttributeOrder(contactType, "PhoneExtension", 5);
        }

        [Fact]
        public void Contact_TollFreePhoneNumberHasOrderElementTest()
        {
            GetXmlAttributeOrder(contactType, "TollFreePhoneNumber", 6);
        }

        [Fact]
        public void Contact_PagerNumberHasOrderElementTest()
        {
            GetXmlAttributeOrder(contactType, "PagerNumber", 7);
        }

        [Fact]
        public void Contact_FaxNumberHasOrderElementTest()
        {
            GetXmlAttributeOrder(contactType, "FaxNumber", 8);
        }

        [Fact]
        public void Contact_EMailAddressHasOrderElementTest()
        {
            GetXmlAttributeOrder(contactType, "EMailAddress", 9);
        }

        [Fact]
        public void Address_CityHasOrderElementTest()
        {
            GetXmlAttributeOrder(addressType, "City", 1);
        }

        [Fact]
        public void Address_StateOrProvinceCodeHasOrderElementTest()
        {
            GetXmlAttributeOrder(addressType, "StateOrProvinceCode", 2);
        }
        [Fact]
        public void Address_PostalCodeHasOrderElementTest()
        {
            GetXmlAttributeOrder(addressType, "PostalCode", 3);
        }

        [Fact]
        public void Address_UrbanizationCodeHasOrderElementTest()
        {
            GetXmlAttributeOrder(addressType, "UrbanizationCode", 4);
        }

        [Fact]
        public void Address_CountryCodeHasOrderElementTest()
        {
            GetXmlAttributeOrder(addressType, "CountryCode", 5);
        }

        [Fact]
        public void Address_CountryNameHasOrderElementTest()
        {
            GetXmlAttributeOrder(addressType, "CountryName", 6);
        }

        [Fact]
        public void Address_ResidentialHasOrderElementTest()
        {
            GetXmlAttributeOrder(addressType, "Residential", 7);
        }

        private void GetXmlAttributeOrder(Type type, string propertyName, int orderValue)
        {
            PropertyInfo propertyInfo = type.GetProperty(propertyName);

            XmlElementAttribute[] xmlElementAttributes = (XmlElementAttribute[])propertyInfo.GetCustomAttributes(typeof(XmlElementAttribute), false);

            Assert.Equal(1, xmlElementAttributes.Length);
            Assert.Equal(orderValue, xmlElementAttributes[0].Order);
        }
    }
}
