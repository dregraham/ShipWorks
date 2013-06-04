using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using System.IO;
using System.Reflection;

using ShipWorks.Stores.Platforms.Ebay;
using ShipWorks.Stores.Platforms.Ebay.Authorization;
using ShipWorks.Stores.Platforms.Ebay.Authorization.Readers;

namespace ShipWorks.Tests.Stores.eBay.Authorization.Readers
{
    /// <summary>
    /// Summary description for TokenXmlReaderTest
    /// </summary>
    [TestClass]
    public class XmlTokenReaderTest
    {
        XmlDocument tokenDocumentWithoutUserId;
        XmlDocument tokenDocumentWithUserId;

        XmlTokenReader testObject;

        public XmlTokenReaderTest()
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ShipWorks.Tests.Stores.eBay.Artifacts.TokenFileWithoutUserId.xml"))
            {
                tokenDocumentWithoutUserId = new XmlDocument();
                tokenDocumentWithoutUserId.Load(stream);
            }

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ShipWorks.Tests.Stores.eBay.Artifacts.TokenFileWithUserId.xml"))
            {
                tokenDocumentWithUserId = new XmlDocument();
                tokenDocumentWithUserId.Load(stream);
            }
        }

        [TestMethod]
        public void Read_FromTokenXmlWithoutUserId_PopulatesTokenKey_Test()
        {
            testObject = new XmlTokenReader(tokenDocumentWithoutUserId);
            string expectedKey = "AgAAAA**AQAAAA**aAAAAA**ZyUYUA**nY+sHZ2PrBmdj6wVnY+sEZ2PrA2dj6wFk4GhCZCCpAWdj6x9nY+seQ**mx0AAA**AAMAAA**/NP/FdHHa5sLL5UImXnWnhT1okn4npvrvkRmwEv9N6/3HYGzF1/CnD90UHw4T1u6wXGdJCW3YRoK7wf9EjFqFFMgHi0p9Ua8WogCQx1PbayLT4KIABEZ4ddnB9XZ2MpJQbqceiZh5BbJ2KkpV4HFE3zprjJCjvYKUMqdzvG+fWXc1sRUttD/ZbImpKn1aRPdl5XqHxmfoTL0K42EvHrw848Ms2z+HcWWhL9xClaPWrS2LHvYlpDj4NXV7NKWGr/PzGdZDkQuXLRNMyMGr+ELryDMZaOOwFCVWytNADOBPfhK0t8N6CkE1PKKMsuOFi4MRdoMFTtsBbYYhBtk4FDnYF7yjjQ9etn5s/zNLE7kXee3VXchlAWEHfzOpvBNk7Apjh4eKS0HY0Qnm7sNZY5xr0e6fz0IZ2OHbbcLalu0fWYCOyIxB4dXUG8WBc7IMVyODdxtuu8PQ/fW6rwgX2KJfTb6XjGemM8IXR8creaKXXnvKe1IG8q1v0fmvWWDDQ87s2Rfh3M3H6V7zO0RaWOGhxVEot71+iSsv4ncsULg/zpxn3FadAGsJa8qIe7vlZGVcaCbDfWTR76Un7hYNWUEmqH6IjXr83GpkifmWqrAYTEmzr26k+5A9VRbGewU+yRrI8xn1mAsFKgRsx+btYxA41yFg1C+BqqumT1tQv8F+9mJKDmMwtHdzDGb/jxW53zvvIWbPMVI6+Y6dBAgrNjHieNn1fZY+a9f8Gq/ICShHI8B77Dd7mHEMb/SSMaH7SFQ";

            TokenData tokenData = testObject.Read();
            Assert.AreEqual(expectedKey, tokenData.Key);
        }

        [TestMethod]
        public void Read_FromTokenXmlWithoutUserId_PopulatesTokenExpirationDate_Test()
        {
            testObject = new XmlTokenReader(tokenDocumentWithoutUserId);
            DateTime expectedExpirationDate = DateTime.ParseExact("2014-01-22 18:35:19", "yyyy-MM-dd HH:mm:ss", null);

            TokenData tokenData = testObject.Read();

            Assert.AreEqual(expectedExpirationDate, tokenData.ExpirationDate);
            Assert.AreEqual(1, tokenData.ExpirationDate.Month);
            Assert.AreEqual(22, tokenData.ExpirationDate.Day);
            Assert.AreEqual(2014, tokenData.ExpirationDate.Year);
            Assert.AreEqual(18, tokenData.ExpirationDate.Hour);
            Assert.AreEqual(35, tokenData.ExpirationDate.Minute);
            Assert.AreEqual(19, tokenData.ExpirationDate.Second);
        }

        [TestMethod]
        public void Read_FromTokenXmlWithoutUserId_DoesNotPopulateTokenUserId_Test()
        {
            testObject = new XmlTokenReader(tokenDocumentWithoutUserId);

            TokenData tokenData = testObject.Read();
            Assert.AreEqual(string.Empty, tokenData.UserId);
        }


        [TestMethod]
        public void Read_FromTokenXmlWithUserId_PopulatesTokenKey_Test()
        {
            testObject = new XmlTokenReader(tokenDocumentWithUserId);
            string expectedKey = "AgAAAA**AQAAAA**aAAAAA**ZyUYUA**nY+sHZ2PrBmdj6wVnY+sEZ2PrA2dj6wFk4GhCZCCpAWdj6x9nY+seQ**mx0AAA**AAMAAA**/NP/FdHHa5sLL5UImXnWnhT1okn4npvrvkRmwEv9N6/3HYGzF1/CnD90UHw4T1u6wXGdJCW3YRoK7wf9EjFqFFMgHi0p9Ua8WogCQx1PbayLT4KIABEZ4ddnB9XZ2MpJQbqceiZh5BbJ2KkpV4HFE3zprjJCjvYKUMqdzvG+fWXc1sRUttD/ZbImpKn1aRPdl5XqHxmfoTL0K42EvHrw848Ms2z+HcWWhL9xClaPWrS2LHvYlpDj4NXV7NKWGr/PzGdZDkQuXLRNMyMGr+ELryDMZaOOwFCVWytNADOBPfhK0t8N6CkE1PKKMsuOFi4MRdoMFTtsBbYYhBtk4FDnYF7yjjQ9etn5s/zNLE7kXee3VXchlAWEHfzOpvBNk7Apjh4eKS0HY0Qnm7sNZY5xr0e6fz0IZ2OHbbcLalu0fWYCOyIxB4dXUG8WBc7IMVyODdxtuu8PQ/fW6rwgX2KJfTb6XjGemM8IXR8creaKXXnvKe1IG8q1v0fmvWWDDQ87s2Rfh3M3H6V7zO0RaWOGhxVEot71+iSsv4ncsULg/zpxn3FadAGsJa8qIe7vlZGVcaCbDfWTR76Un7hYNWUEmqH6IjXr83GpkifmWqrAYTEmzr26k+5A9VRbGewU+yRrI8xn1mAsFKgRsx+btYxA41yFg1C+BqqumT1tQv8F+9mJKDmMwtHdzDGb/jxW53zvvIWbPMVI6+Y6dBAgrNjHieNn1fZY+a9f8Gq/ICShHI8B77Dd7mHEMb/SSMaH7SFQ";

            TokenData tokenData = testObject.Read();
            Assert.AreEqual(expectedKey, tokenData.Key);
        }

        [TestMethod]
        public void Read_FromTokenXmlWithUserId_PopulatesTokenExpirationDate_Test()
        {
            testObject = new XmlTokenReader(tokenDocumentWithUserId);
            DateTime expectedExpirationDate = DateTime.Parse("2014-01-22 18:35:19");

            TokenData tokenData = testObject.Read();
            Assert.AreEqual(expectedExpirationDate, tokenData.ExpirationDate);
        }

        [TestMethod]
        public void Read_FromTokenXmlWithUserId_PopulatesTokenUserId_Test()
        {
            testObject = new XmlTokenReader(tokenDocumentWithUserId);

            TokenData tokenData = testObject.Read();
            Assert.AreEqual("testuser_shipworks", tokenData.UserId);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void Read_ThrowsEbayException_WhenUnableToReadXml_Test()
        {
            // Send an empty xml document to trigger the exception
            testObject = new XmlTokenReader(new XmlDocument());
            testObject.Read();
        }
    }
}
