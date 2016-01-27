using System.Xml;
using ShipWorks.ApplicationCore.Licensing;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing
{
    public class AddStoreResponseTest
    {
        [Fact]
        public void Key_IsSet_WhenXmlHasKey()
        {
            string key = "3NFXI-RYQAM-0KY3M-XGKY5-YAHOO-JJ@JJ.COM";

            string standardOutput = "<?xml version='1.0'?>" +
                                    "<CreateStoreActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>" +
                                    $"  <License><Key>{key}</Key></License>" +
                                    "</CreateStoreActivityResponse>";

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(standardOutput);

            AddStoreResponse testObject = new AddStoreResponse(xmlDocument);

            Assert.Equal(key, testObject.Key);
        }

        [Fact]
        public void Success_IsTrue_WhenNoError()
        {
            string key = "3NFXI-RYQAM-0KY3M-XGKY5-YAHOO-JJ@JJ.COM";

            string standardOutput = "<?xml version='1.0'?>" +
                                    "<CreateStoreActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>" +
                                    $"  <LicenseKey>{key}</LicenseKey>" +
                                    "</CreateStoreActivityResponse>";

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(standardOutput);

            AddStoreResponse testObject = new AddStoreResponse(xmlDocument);

            Assert.True(testObject.Success);
        }

        [Fact]
        public void Error_IsEmpty_WhenNoError()
        {
            string key = "3NFXI-RYQAM-0KY3M-XGKY5-YAHOO-JJ@JJ.COM";

            string standardOutput = "<?xml version='1.0'?>" +
                                    "<CreateStoreActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>" +
                                    $"  <LicenseKey>{key}</LicenseKey>" +
                                    "</CreateStoreActivityResponse>";

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(standardOutput);

            AddStoreResponse testObject = new AddStoreResponse(xmlDocument);

            Assert.True(string.IsNullOrEmpty(testObject.Error));
        }

        [Fact]
        public void Key_IsEmptyString_WhenNoKey()
        {
            string error = "some random error";

            string standardOutput = "<?xml version='1.0'?>" +
                                    "<CreateStoreActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>" +
                                    $"  <Error>{error}</Error>" +
                                    "</CreateStoreActivityResponse>";

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(standardOutput);

            AddStoreResponse testObject = new AddStoreResponse(xmlDocument);

            Assert.True(string.IsNullOrEmpty(testObject.Key));
        }

        [Fact]
        public void Error_IsSet_WhenError()
        {
            string error = "some random error";

            string standardOutput = "<?xml version='1.0'?>" +
                                    "<CreateStoreActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>" +
                                    $"  <Error>{error}</Error>" +
                                    "</CreateStoreActivityResponse>";

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(standardOutput);

            AddStoreResponse testObject = new AddStoreResponse(xmlDocument);

            Assert.Equal(error, testObject.Error);
        }

        [Fact]
        public void Success_IsFalse_WhenError()
        {
            string error = "some random error";
            string key = "random key";

            string standardOutput = "<?xml version='1.0'?>" +
                                    "<CreateStoreActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>" +
                                    $"  <Error>{error}</Error>" +
                                    $"  <LicenseKey>{key}</LicenseKey>" +
                                    "</CreateStoreActivityResponse>";

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(standardOutput);

            AddStoreResponse testObject = new AddStoreResponse(xmlDocument);

            Assert.False(testObject.Success);
        }
    }
}