using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using System.IO;
using System.Reflection;

using ShipWorks.Stores.Platforms.Ebay.Authorization.Repositories;
using ShipWorks.ApplicationCore;
using ShipWorks.Stores.Platforms.Ebay;
using ShipWorks.Stores.Platforms.Ebay.Requests.Authorization;
using Moq;

namespace ShipWorks.Tests.Stores.eBay.Authorization
{
    [TestClass]
    public class EbayTokenRepositoryTest
    {
        private const string License = "06d639af-1279-4f87-ac9f-9d710bdd5d2f";
        private const string UserId = "TESTUSER_shipworks";
        private const string TokenKey = "AgAAAA**AQAAAA**aAAAAA**ZyUYUA**nY+sHZ2PrBmdj6wVnY+sEZ2PrA2dj6wFk4GhCZCCpAWdj6x9nY+seQ**mx0AAA**AAMAAA**/NP/FdHHa5sLL5UImXnWnhT1okn4npvrvkRmwEv9N6/3HYGzF1/CnD90UHw4T1u6wXGdJCW3YRoK7wf9EjFqFFMgHi0p9Ua8WogCQx1PbayLT4KIABEZ4ddnB9XZ2MpJQbqceiZh5BbJ2KkpV4HFE3zprjJCjvYKUMqdzvG+fWXc1sRUttD/ZbImpKn1aRPdl5XqHxmfoTL0K42EvHrw848Ms2z+HcWWhL9xClaPWrS2LHvYlpDj4NXV7NKWGr/PzGdZDkQuXLRNMyMGr+ELryDMZaOOwFCVWytNADOBPfhK0t8N6CkE1PKKMsuOFi4MRdoMFTtsBbYYhBtk4FDnYF7yjjQ9etn5s/zNLE7kXee3VXchlAWEHfzOpvBNk7Apjh4eKS0HY0Qnm7sNZY5xr0e6fz0IZ2OHbbcLalu0fWYCOyIxB4dXUG8WBc7IMVyODdxtuu8PQ/fW6rwgX2KJfTb6XjGemM8IXR8creaKXXnvKe1IG8q1v0fmvWWDDQ87s2Rfh3M3H6V7zO0RaWOGhxVEot71+iSsv4ncsULg/zpxn3FadAGsJa8qIe7vlZGVcaCbDfWTR76Un7hYNWUEmqH6IjXr83GpkifmWqrAYTEmzr26k+5A9VRbGewU+yRrI8xn1mAsFKgRsx+btYxA41yFg1C+BqqumT1tQv8F+9mJKDmMwtHdzDGb/jxW53zvvIWbPMVI6+Y6dBAgrNjHieNn1fZY+a9f8Gq/ICShHI8B77Dd7mHEMb/SSMaH7SFQ";

        private EbayTokenRepository testObject;
        private Mock<IEbayWebClient> mockedWebClient;

        public EbayTokenRepositoryTest()
        { }

        [TestInitialize]
        public void Initialize()
        {
            // Provide default implementations for our mocked web client
            mockedWebClient = new Mock<IEbayWebClient>();

            // Setup the GetTangoAuthorization method to return a valid token
            string tokenXml = string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
<eBayToken>
  <Token>{0}</Token>
  <Expiration>2014-01-22 18:35:19</Expiration>
</eBayToken>", TokenKey);

            XmlDocument tokenDocument = new XmlDocument();
            tokenDocument.LoadXml(tokenXml);
                        
            mockedWebClient.Setup(c => c.GetTangoAuthorization()).Returns(tokenDocument);
            

            // Setup the GetUserInfo method to return a populated user response 
            mockedWebClient.Setup(c => c.GetUserInfo(It.IsAny<string>()))
                .Returns
                (
                    new ShipWorks.Stores.Platforms.Ebay.WebServices.GetUserResponseType()
                    {
                        User = new ShipWorks.Stores.Platforms.Ebay.WebServices.UserType()
                        {
                            UserID = UserId
                        }
                    }
                );


            // Create our test object so it uses the mocked web client
            testObject = new EbayTokenRepository(mockedWebClient.Object);
        }

        [TestMethod]
        public void GetTokenData_DelegatesToWebClientToGetTokenData_Test()
        {
            testObject.GetTokenData();
            
            mockedWebClient.Verify(c => c.GetTangoAuthorization(), Times.Once());
        }

        [TestMethod]
        public void GetTokenData_DelegatesWebClientToGetUserInfo_Test()
        {
            testObject.GetTokenData();
        
            // Checking that the method was called once and with the expected token key value
            mockedWebClient.Verify(c => c.GetUserInfo(TokenKey), Times.Once());
        }


        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetTokenData_ThrowsEbayException_WhenTokenElementIsNotFound_Test()
        {
            // Arrange - setup the web request to simulate a missing token element 
            string tokenXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<eBayToken>
  <Expiration>2014-01-22 18:35:19</Expiration>
</eBayToken>";

            XmlDocument tokenDocument = new XmlDocument();
            tokenDocument.LoadXml(tokenXml);

            mockedWebClient.Setup(c => c.GetTangoAuthorization()).Returns(tokenDocument);

            // Act - this should throw an eBay exception
            testObject.GetTokenData();
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetTokenData_ThrowsEbayException_WhenEbayElementIsNotFound_Test()
        {
            // Arrange - Simulate a response that does not have the "ebayToken" element
            string tokenXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<googleToken>
  <Token>AgAAAA**AQAAAA**aAAAAA**ZyUYUA**nY+sHZ2PrBmdj6wVnY+sEZ2PrA2dj6wFk4GhCZCCpAWdj6x9nY+seQ**mx0AAA**AAMAAA**/NP/FdHHa5sLL5UImXnWnhT1okn4npvrvkRmwEv9N6/3HYGzF1/CnD90UHw4T1u6wXGdJCW3YRoK7wf9EjFqFFMgHi0p9Ua8WogCQx1PbayLT4KIABEZ4ddnB9XZ2MpJQbqceiZh5BbJ2KkpV4HFE3zprjJCjvYKUMqdzvG+fWXc1sRUttD/ZbImpKn1aRPdl5XqHxmfoTL0K42EvHrw848Ms2z+HcWWhL9xClaPWrS2LHvYlpDj4NXV7NKWGr/PzGdZDkQuXLRNMyMGr+ELryDMZaOOwFCVWytNADOBPfhK0t8N6CkE1PKKMsuOFi4MRdoMFTtsBbYYhBtk4FDnYF7yjjQ9etn5s/zNLE7kXee3VXchlAWEHfzOpvBNk7Apjh4eKS0HY0Qnm7sNZY5xr0e6fz0IZ2OHbbcLalu0fWYCOyIxB4dXUG8WBc7IMVyODdxtuu8PQ/fW6rwgX2KJfTb6XjGemM8IXR8creaKXXnvKe1IG8q1v0fmvWWDDQ87s2Rfh3M3H6V7zO0RaWOGhxVEot71+iSsv4ncsULg/zpxn3FadAGsJa8qIe7vlZGVcaCbDfWTR76Un7hYNWUEmqH6IjXr83GpkifmWqrAYTEmzr26k+5A9VRbGewU+yRrI8xn1mAsFKgRsx+btYxA41yFg1C+BqqumT1tQv8F+9mJKDmMwtHdzDGb/jxW53zvvIWbPMVI6+Y6dBAgrNjHieNn1fZY+a9f8Gq/ICShHI8B77Dd7mHEMb/SSMaH7SFQ</Token>
  <Expiration>2014-01-22 18:35:19</Expiration>
</googleToken>";

            XmlDocument tokenDocument = new XmlDocument();
            tokenDocument.LoadXml(tokenXml);

            mockedWebClient.Setup(c => c.GetTangoAuthorization()).Returns(tokenDocument);

            // Act - should throw an exception
            testObject.GetTokenData();
        }

        [TestMethod]
        public void GetTokenData_ReturnsTokenXml_Test()
        {
            // We're just interested in checking whether the repository 
            // constructs an XML string after combining the results from
            // the user info request and the tango request
            string tokenXml = testObject.GetTokenData();
            Assert.IsTrue(IsValidXml(tokenXml));
        }

        [TestMethod]
        public void GetTokenData_AppendsUserIdElement_Test()
        {
            string tokenXml = testObject.GetTokenData();

            XmlDocument document = new XmlDocument();
            document.LoadXml(tokenXml);

            Assert.IsNotNull(document.SelectSingleNode("//UserID"));
        }

        [TestMethod]
        public void GetTokenData_UserIdElement_ContainsUserId_Test()
        {
            string tokenXml = testObject.GetTokenData();

            XmlDocument document = new XmlDocument();
            document.LoadXml(tokenXml);

            Assert.AreEqual(UserId, document.SelectSingleNode("//UserID").InnerText);
        }

        private bool IsValidXml(string xml)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                   
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
