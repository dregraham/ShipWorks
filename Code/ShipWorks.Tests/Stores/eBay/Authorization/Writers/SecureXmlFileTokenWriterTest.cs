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
using Moq;
using ShipWorks.Stores.Platforms.Ebay.Authorization.Readers;
using ShipWorks.Stores.Platforms.Ebay.Authorization.Repositories;

namespace ShipWorks.Tests.Stores.eBay.Authorization.Writers
{
    [TestClass]
    public class SecureXmlFileTokenWriterTest
    {
        const string TestFileName = "secureXmlFileTokenWriterTestFile.tkn";
        private const string SampleTokenText = "<Token><ExpirationDate>2030-8-1 12:03:48</ExpirationDate><Key>SomeKey</Key><UserId>SomeUserId</UserId></Token>";
        private TokenData sampleTokenData;
        
        private FileInfo tokenFile;
        private Token token;

        private Mock<ITokenFactory> mockedFactory;
        private Mock<ITokenReader> mockedReader;
        private Mock<ITokenReader> mockedFileReader;
        private Mock<ITokenWriter> mockedWriter;
        private Mock<ITokenRepository> mockedRepository;

        private SecureXmlFileTokenWriter testObject;
        
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

            if (File.Exists(TestFileName))
            {
                File.Delete(TestFileName);
            }
          
            tokenFile = new FileInfo(TestFileName);
            testObject = new SecureXmlFileTokenWriter(tokenFile, "token");
        }

        [TestCleanup]
        public void Cleanup()
        {
            File.Delete(TestFileName);
        }

        [TestMethod]
        public void Write_WritesDataToFile_Test()
        {
            testObject.Write(token);
            Assert.IsTrue(tokenFile.Length > 0);
        }

        [TestMethod]
        public void Write_DoesNotWriteRawXmlToFile_Test()
        {
            testObject.Write(token);
            
            // Read the data that was written and check to see that it's XML
            string data = GetStreamData();
            Assert.IsFalse(IsValidXml(data));
        }

        [TestMethod]
        public void Write_EncryptsTokenXmlToFile_Test()
        {
            testObject.Write(token);

            string data = GetStreamData();
            string decryptedData = SecureText.Decrypt(data, "token");

            Assert.IsTrue(IsValidXml(decryptedData));
        }


        /// <summary>
        /// Helper method to read any data written to our stream
        /// </summary>
        /// <returns>The string written to the stream</returns>
        private string GetStreamData()
        {
            using (StreamReader reader = new StreamReader(tokenFile.OpenRead()))
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
