#region

using Autofac.Extras.Moq;
using Autofac.Features.Indexed;
using Interapptive.Shared.Security;
using Moq;
using ShipWorks.ApplicationCore.Security;
using ShipWorks.Data.Administration;
using System;
using Xunit;

#endregion

namespace ShipWorks.Tests.ApplicationCore.Security
{
    public class EncryptionProviderFactoryTest
    {
        [Fact]
        public void CreateLicenseEncryptionProvider_UsesLicenseCipher()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var cipherProvider = MockCipherProvider(mock, CipherContext.License);

                mock.Create<EncryptionProviderFactory>().CreateLicenseEncryptionProvider();

                cipherProvider.Verify(i => i[It.Is<CipherContext>(c => c == CipherContext.License)], Times.Once);
            }
        }

        [Fact]
        public void CreateLicenseEncryptionProvider_CallsIsCustomerLicenseSupported()
        {
            using (var mock = AutoMock.GetLoose())
            {
                MockCipherProvider(mock, CipherContext.License);
                var sqlSchemaVersion = mock.Mock<ISqlSchemaVersion>();

                mock.Create<EncryptionProviderFactory>().CreateLicenseEncryptionProvider();

                sqlSchemaVersion.Verify(v => v.IsCustomerLicenseSupported(), Times.Once);
            }
        }

        [Fact]
        public void CreateLicenseEncryptionProvider_ReturnsLicenseEncryptionProvider()
        {
            using (var mock = AutoMock.GetLoose())
            {
                MockCipherProvider(mock, CipherContext.License);

                IEncryptionProvider licenseEncryptionProvider =
                    mock.Create<EncryptionProviderFactory>().CreateLicenseEncryptionProvider();

                Assert.IsType<LicenseEncryptionProvider>(licenseEncryptionProvider);
            }
        }

        [Fact]
        public void CreateSearsEncryptionProvider_UsesSearsCipher()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var cipherProvider = MockCipherProvider(mock, CipherContext.Sears);

                mock.Create<EncryptionProviderFactory>().CreateSearsEncryptionProvider();

                cipherProvider.Verify(i => i[It.Is<CipherContext>(c => c == CipherContext.Sears)], Times.Once);
            }
        }

        [Fact]
        public void CreateSearsEncryptionProvider_ReturnsAesEncryptionProvider()
        {
            using (var mock = AutoMock.GetLoose())
            {
                MockCipherProvider(mock, CipherContext.Sears);

                IEncryptionProvider searsEncryptionProvider =
                    mock.Create<EncryptionProviderFactory>().CreateSearsEncryptionProvider();

                Assert.IsType<AesEncryptionProvider>(searsEncryptionProvider);
            }
        }

        [Fact]
        public void CreateSecureTextEncryptionProvider_ReturnsSecureTextEncryptionProvider()
        {
            using (var mock = AutoMock.GetLoose())
            {
                IEncryptionProvider secureTextEncryptionProvider =
                    mock.Create<EncryptionProviderFactory>().CreateSecureTextEncryptionProvider("blah");

                Assert.IsType<SecureTextEncryptionProvider>(secureTextEncryptionProvider);
            }
        }

        [Fact]
        public void CreateAesStreamEncryptionProvider_UsesStreamCipher()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var cipherProvider = MockCipherProvider(mock, CipherContext.Stream);

                mock.Create<EncryptionProviderFactory>().CreateAesStreamEncryptionProvider();

                cipherProvider.Verify(i => i[It.Is<CipherContext>(c => c == CipherContext.Stream)], Times.Once);
            }
        }

        [Fact]
        public void CreateAesStreamEncryptionProvider_ReturnsAesStreamEncryptionProvider()
        {
            using (var mock = AutoMock.GetLoose())
            {
                MockCipherProvider(mock, CipherContext.Stream);

                IEncryptionProvider provider = mock.Create<EncryptionProviderFactory>().CreateAesStreamEncryptionProvider();

                Assert.IsType<AesStreamEncryptionProvider>(provider);
            }
        }

        private static Mock<IIndex<CipherContext, ICipherKey>> MockCipherProvider(AutoMock mock, CipherContext license)
        {
            var cipherKey = mock.Mock<ICipherKey>();
            var cipherProvider = mock.MockRepository.Create<IIndex<CipherContext, ICipherKey>>();

            cipherProvider.Setup(i => i[It.Is<CipherContext>(c => c == license)])
                .Returns(cipherKey.Object);
            // throw if a license not specified is requested
            cipherProvider.Setup(i => i[It.Is<CipherContext>(c => c != license)])
                .Throws(new Exception($"Invalid License Requested: {license}"));

            mock.Provide(cipherProvider.Object);
            return cipherProvider;
        }
    }
}