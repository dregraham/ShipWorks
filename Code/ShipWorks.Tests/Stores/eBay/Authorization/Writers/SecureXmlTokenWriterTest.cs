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
using ShipWorks.Stores.Platforms.Ebay.Authorization.Writers;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Platforms.Ebay.Authorization.Readers;
using ShipWorks.Stores.Platforms.Ebay.Authorization.Repositories;
using Moq;

namespace ShipWorks.Tests.Stores.eBay.Authorization.Writers
{
    [TestClass]
    public class SecureXmlTokenWriterTest
    {
        SecureXmlTokenWriter testObject;
        MemoryStream stream;

        private const string SampleTokenText = "<Token><ExpirationDate>2030-8-1 12:03:48</ExpirationDate><Key>SomeKey</Key><UserId>SomeUserId</UserId></Token>";
        private TokenData sampleTokenData;

        private Token token;

        private Mock<ITokenFactory> mockedFactory;
        private Mock<ITokenReader> mockedReader;
        private Mock<ITokenReader> mockedFileReader;
        private Mock<ITokenWriter> mockedWriter;
        private Mock<ITokenRepository> mockedRepository;


        [TestInitialize]
        public void Initialize()
        {
            sampleTokenData = new TokenData() { ExpirationDate = DateTime.Parse("8/1/2030 12:03:48 PM"), Key = "SomeKey", UserId = "SomeUserId" };

            // Setup a mocked token reader
            mockedReader = new Mock<ITokenReader>();
            mockedReader.Setup(r => r.Read()).Returns(sampleTokenData);

            // Setup another token reader to use to read files (to determine whether a file reader was used in our tests)
            mockedFileReader = new Mock<ITokenReader>();
            mockedFileReader.Setup(r => r.Read()).Returns(sampleTokenData);

            // Setup a mocked token writer
            mockedWriter = new Mock<ITokenWriter>();
            mockedWriter.Setup(w => w.Write(It.IsAny<Token>()));

            // Setup a mocked token repository
            mockedRepository = new Mock<ITokenRepository>();
            mockedRepository.Setup(r => r.GetTokenData()).Returns(SampleTokenText);

            // Setup the mocked token factory to return the mocked reader, writer, and repository created above
            mockedFactory = new Mock<ITokenFactory>();
            mockedFactory.Setup(f => f.CreateReader(It.IsAny<string>())).Returns(mockedReader.Object);
            mockedFactory.Setup(f => f.CreateReader(It.IsAny<FileInfo>())).Returns(mockedFileReader.Object);
            mockedFactory.Setup(f => f.CreateRepository(It.IsAny<string>())).Returns(mockedRepository.Object);
            mockedFactory.Setup(f => f.CreateWriter(It.IsAny<FileInfo>())).Returns(mockedWriter.Object);


            // Populate our token with mocked token data
            token = new Token("my license", new TokenData(), mockedFactory.Object);
            token.Load();


            stream = new MemoryStream();
            testObject = new SecureXmlTokenWriter(stream, "token");
        }

        [TestCleanup]
        public void Cleanup()
        {
            stream.Close();
        }

        [TestMethod]
        public void Write_WritesDataToStream_Test()
        {
            testObject.Write(token);

            // Just checking that the stream was written to, so check the 
            // current position of the stream
            Assert.IsTrue(stream.Position > 0);
        }

        [TestMethod]
        public void Write_LeavesStreamOpen_Test()
        {
            testObject.Write(token);

            Assert.IsTrue(stream.CanRead);
            Assert.IsTrue(stream.CanWrite);
        }

        [TestMethod]
        public void Write_DoesNotWriteRawXmlToStream_Test()
        {
            testObject.Write(token);

            string dataWrittenToStream = GetStreamData();
            Assert.IsFalse(IsValidXml(dataWrittenToStream));
        }

        [TestMethod]
        public void Write_EncryptsXml_Test()
        {
            testObject.Write(token);

            string streamData = GetStreamData();
            string decryptedData = SecureText.Decrypt(streamData, "token");

            Assert.IsTrue(IsValidXml(decryptedData));
        }

        [TestMethod]
        public void Write_WritesTokenDataToStream_Test()
        {
            testObject.Write(token);

            string streamData = GetStreamData();
            string tokenXml = SecureText.Decrypt(streamData, "token");

            // Load the xml string into a document and query for the expected values
            XmlDocument tokenDoc = new XmlDocument();
            tokenDoc.LoadXml(tokenXml);

            Assert.AreEqual(token.Key, tokenDoc.SelectSingleNode("//Token").InnerText);
            Assert.AreEqual(token.ExpirationDate, DateTime.Parse(tokenDoc.SelectSingleNode("//Expiration").InnerText));
            Assert.AreEqual(token.UserId, tokenDoc.SelectSingleNode("//UserID").InnerText);
        }

        /// <summary>
        /// Helper method to read any data written to our stream
        /// </summary>
        /// <returns>The string written to the stream</returns>
        private string GetStreamData()
        {
            stream.Seek(0, SeekOrigin.Begin);
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Helper method to determines whether [the specified data] [is valid XML] .
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>
        ///   <c>true</c> if [the specified data][is valid XML]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsValidXml(string data)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(data);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
