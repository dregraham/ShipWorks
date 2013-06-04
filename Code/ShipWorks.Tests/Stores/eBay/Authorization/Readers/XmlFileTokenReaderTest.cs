using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

using ShipWorks.Stores.Platforms.Ebay.Authorization;
using ShipWorks.Stores.Platforms.Ebay.Authorization.Readers;
using ShipWorks.Stores.Platforms.Ebay;

namespace ShipWorks.Tests.Stores.eBay.Authorization.Readers
{
    [TestClass]
    [DeploymentItem(@"Stores\eBay\Artifacts\TokenFileWithUserId.tkn")]
    [DeploymentItem(@"Stores\eBay\Artifacts\TokenFileWithoutUserId.tkn")]
    public class XmlFileTokenReaderTest
    {
        FileInfo tokenFileWithoutUserId;
        FileInfo tokenFileWithUserId;

        XmlFileTokenReader testObject;

        public XmlFileTokenReaderTest()
        {
            tokenFileWithoutUserId = new FileInfo("TokenFileWithoutUserId.tkn");
            tokenFileWithUserId = new FileInfo("TokenFileWithUserId.tkn");
        }

        [TestMethod]
        public void Read_FromTokenFileWithoutUserId_PopulatesTokenKey_Test()
        {            
            testObject = new XmlFileTokenReader(tokenFileWithoutUserId);

            string expectedKey = "AgAAAA**AQAAAA**aAAAAA**ZyUYUA**nY+sHZ2PrBmdj6wVnY+sEZ2PrA2dj6wFk4GhCZCCpAWdj6x9nY+seQ**mx0AAA**AAMAAA**/NP/FdHHa5sLL5UImXnWnhT1okn4npvrvkRmwEv9N6/3HYGzF1/CnD90UHw4T1u6wXGdJCW3YRoK7wf9EjFqFFMgHi0p9Ua8WogCQx1PbayLT4KIABEZ4ddnB9XZ2MpJQbqceiZh5BbJ2KkpV4HFE3zprjJCjvYKUMqdzvG+fWXc1sRUttD/ZbImpKn1aRPdl5XqHxmfoTL0K42EvHrw848Ms2z+HcWWhL9xClaPWrS2LHvYlpDj4NXV7NKWGr/PzGdZDkQuXLRNMyMGr+ELryDMZaOOwFCVWytNADOBPfhK0t8N6CkE1PKKMsuOFi4MRdoMFTtsBbYYhBtk4FDnYF7yjjQ9etn5s/zNLE7kXee3VXchlAWEHfzOpvBNk7Apjh4eKS0HY0Qnm7sNZY5xr0e6fz0IZ2OHbbcLalu0fWYCOyIxB4dXUG8WBc7IMVyODdxtuu8PQ/fW6rwgX2KJfTb6XjGemM8IXR8creaKXXnvKe1IG8q1v0fmvWWDDQ87s2Rfh3M3H6V7zO0RaWOGhxVEot71+iSsv4ncsULg/zpxn3FadAGsJa8qIe7vlZGVcaCbDfWTR76Un7hYNWUEmqH6IjXr83GpkifmWqrAYTEmzr26k+5A9VRbGewU+yRrI8xn1mAsFKgRsx+btYxA41yFg1C+BqqumT1tQv8F+9mJKDmMwtHdzDGb/jxW53zvvIWbPMVI6+Y6dBAgrNjHieNn1fZY+a9f8Gq/ICShHI8B77Dd7mHEMb/SSMaH7SFQ";

            TokenData tokenData = testObject.Read();
            Assert.AreEqual(expectedKey, tokenData.Key);
        }

        [TestMethod]
        public void Read_FromTokenFileWithoutUserId_PopulatesTokenExpirationDate_Test()
        {
            testObject = new XmlFileTokenReader(tokenFileWithoutUserId);

            DateTime expectedExpirationDate = DateTime.Parse("2014-01-22 18:35:19");

            TokenData tokenData = testObject.Read();
            Assert.AreEqual(expectedExpirationDate, tokenData.ExpirationDate);
        }

        [TestMethod]
        public void Read_FromTokenFileWithoutUserId_DoesNotPopulateTokenUserId_Test()
        {
            testObject = new XmlFileTokenReader(tokenFileWithoutUserId);

            TokenData tokenData = testObject.Read();
            Assert.AreEqual(string.Empty, tokenData.UserId);
        }

        [TestMethod]
        public void Read_FromTokenFileWithUserId_PopulatesTokenKey_Test()
        {
            testObject = new XmlFileTokenReader(tokenFileWithUserId);

            string expectedKey = "AgAAAA**AQAAAA**aAAAAA**ZyUYUA**nY+sHZ2PrBmdj6wVnY+sEZ2PrA2dj6wFk4GhCZCCpAWdj6x9nY+seQ**mx0AAA**AAMAAA**/NP/FdHHa5sLL5UImXnWnhT1okn4npvrvkRmwEv9N6/3HYGzF1/CnD90UHw4T1u6wXGdJCW3YRoK7wf9EjFqFFMgHi0p9Ua8WogCQx1PbayLT4KIABEZ4ddnB9XZ2MpJQbqceiZh5BbJ2KkpV4HFE3zprjJCjvYKUMqdzvG+fWXc1sRUttD/ZbImpKn1aRPdl5XqHxmfoTL0K42EvHrw848Ms2z+HcWWhL9xClaPWrS2LHvYlpDj4NXV7NKWGr/PzGdZDkQuXLRNMyMGr+ELryDMZaOOwFCVWytNADOBPfhK0t8N6CkE1PKKMsuOFi4MRdoMFTtsBbYYhBtk4FDnYF7yjjQ9etn5s/zNLE7kXee3VXchlAWEHfzOpvBNk7Apjh4eKS0HY0Qnm7sNZY5xr0e6fz0IZ2OHbbcLalu0fWYCOyIxB4dXUG8WBc7IMVyODdxtuu8PQ/fW6rwgX2KJfTb6XjGemM8IXR8creaKXXnvKe1IG8q1v0fmvWWDDQ87s2Rfh3M3H6V7zO0RaWOGhxVEot71+iSsv4ncsULg/zpxn3FadAGsJa8qIe7vlZGVcaCbDfWTR76Un7hYNWUEmqH6IjXr83GpkifmWqrAYTEmzr26k+5A9VRbGewU+yRrI8xn1mAsFKgRsx+btYxA41yFg1C+BqqumT1tQv8F+9mJKDmMwtHdzDGb/jxW53zvvIWbPMVI6+Y6dBAgrNjHieNn1fZY+a9f8Gq/ICShHI8B77Dd7mHEMb/SSMaH7SFQ";

            TokenData tokenData = testObject.Read();
            Assert.AreEqual(expectedKey, tokenData.Key);
        }

        [TestMethod]
        public void Read_FromTokenFileWithUserId_PopulatesTokenExpirationDate_Test()
        {
            testObject = new XmlFileTokenReader(tokenFileWithUserId);

            DateTime expectedExpirationDate = DateTime.Parse("2014-01-22 18:35:19");

            TokenData tokenData = testObject.Read();
            Assert.AreEqual(expectedExpirationDate, tokenData.ExpirationDate);
        }

        [TestMethod]
        public void Read_FromTokenFileWithUserId_PopulatesTokenUserId_Test()
        {
            testObject = new XmlFileTokenReader(tokenFileWithUserId);

            TokenData tokenData = testObject.Read();
            Assert.AreEqual("testuser_shipworks", tokenData.UserId);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void Constructor_ThrowsEbayException_WhenUnableToOpenFile_Test()
        {
            // Send in a file info object with a non-existent file to generate the exception
            testObject = new XmlFileTokenReader(new FileInfo("NonExistentFile.txt"));
            testObject.Read();
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void Read_ThrowsEbayException_WhenUnableToReadFile_Test()
        {            
            testObject = new XmlFileTokenReader(tokenFileWithoutUserId);

            // Call read twice to generate the exception (the file stream will have
            // been closed)
            testObject.Read();
            testObject.Read();
        }
    }
}
