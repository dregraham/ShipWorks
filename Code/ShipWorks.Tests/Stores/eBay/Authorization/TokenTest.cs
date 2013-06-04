using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

using ShipWorks.Stores.Platforms.Ebay.Authorization;
using ShipWorks.Stores.Platforms.Ebay.Authorization.Readers;
using ShipWorks.Stores.Platforms.Ebay.Authorization.Writers;
using ShipWorks.Stores.Platforms.Ebay.Authorization.Repositories;
using Moq;

namespace ShipWorks.Tests.Stores.eBay.Authorization
{
    /// <summary>
    /// Summary description for TokenTest
    /// </summary>
    [TestClass]
    public class TokenTest
    {
        private const string SampleTokenText = "<Token><ExpirationDate>2030-8-1 12:03:48</ExpirationDate><Key>SomeKey</Key><UserId>SomeUserId</UserId></Token>";
        private TokenData sampleTokenData;

        private Token testObject;
        
        private Mock<ITokenFactory> mockedFactory;
        private Mock<ITokenReader> mockedReader;
        private Mock<ITokenReader> mockedFileReader;
        private Mock<ITokenWriter> mockedWriter;
        private Mock<ITokenRepository> mockedRepository;

        [TestInitialize]
        public void Initialize()
        {
            // The setup is here rather than the constructor because we always want to start with a fresh 
            // token (our test object) for each test
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


            // Create Token test object using the mocked factory
            testObject = new Token("my license", new TokenData(), mockedFactory.Object);
        }

        [TestMethod]
        public void Load_CreatesTokenRepository_FromFactory_Test()
        {
            testObject.Load();

            mockedFactory.Verify(f => f.CreateRepository(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void Load_DelegatesToTokenRepository_WhenLoadingFromRepository_Test()
        {
            testObject.Load();

            mockedRepository.Verify(r => r.GetTokenData(), Times.Once());
        }

        [TestMethod]
        public void Load_CreatesTokenReader_FromFactory_Test()
        {
            testObject.Load();

            mockedFactory.Verify(f => f.CreateReader(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void Load_DelegatesToReader_WhenLoadingFromRepository_Test()
        {
            testObject.Load();

            mockedReader.Verify(r => r.Read(), Times.Once());
        }

        [TestMethod]
        public void Load_PopulatesTokenData_WhenLoadingFromRepository_Test()
        {
            testObject.Load();

            // Since we're using mocked up objects, we can compare against our sample token data
            Assert.AreEqual(sampleTokenData.UserId, testObject.UserId);
            Assert.AreEqual(sampleTokenData.Key, testObject.Key);
            Assert.AreEqual(sampleTokenData.ExpirationDate, testObject.ExpirationDate);
        }

        [TestMethod]
        public void Load_DoesNotDelegateToTokenRepository_WhenLoadingFromFile_Test()
        {
            FileInfo file = new FileInfo("someFile.tkn");
            testObject.Load(file);

            mockedRepository.Verify(r => r.GetTokenData(), Times.Never());
        }

        [TestMethod]
        public void Load_DelegatesToFileReader_WhenLoadingFromFile_Test()
        {
            FileInfo file = new FileInfo("someFile.tkn");
            testObject.Load(file);

            // Tyring to load from a file so check our mocked file reader (rather than the normal reader)
            mockedFileReader.Verify(r => r.Read(), Times.Once());
        }

        [TestMethod]
        public void Load_PopulatesTokenData_WhenLoadingFromFile_Test()
        {
            FileInfo file = new FileInfo("someFile.tkn");
            testObject.Load(file);

            // Since we're using mocked up objects, we can compare against our sample token data            
            Assert.AreEqual(sampleTokenData.UserId, testObject.UserId);
            Assert.AreEqual(sampleTokenData.Key, testObject.Key);
            Assert.AreEqual(sampleTokenData.ExpirationDate, testObject.ExpirationDate);
        }

        [TestMethod]
        public void Save_DelegatesToFactory_ToCreateTokenWriter_Test()
        {
            FileInfo file = new FileInfo("someFile.tkn");
            testObject.Save(file);

            // Check that the factory created the writer and the correct file was used
            mockedFactory.Verify(f => f.CreateWriter(file), Times.Once());
        }

        [TestMethod]
        public void Save_DelegatesToTokenWriter_Test()
        {
            FileInfo file = new FileInfo("someFile.tkn");
            testObject.Save(file);

            mockedWriter.Verify(w => w.Write(It.IsAny<Token>()), Times.Once());
        }
    }
}
