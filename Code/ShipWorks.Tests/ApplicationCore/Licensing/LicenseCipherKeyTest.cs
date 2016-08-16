using System;
using Autofac.Extras.Moq;
using Interapptive.Shared.Security;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing
{
    public class LicenseCipherKeyTest
    {
        [Fact]
        public void IV_ReturnsCorrectValue()
        {
            byte[] expectedIV = { 125, 42, 69, 178, 253, 78, 1, 17, 77, 56, 129, 11, 25, 225, 201, 14 };
            using (var mock = AutoMock.GetLoose())
            {
                IDatabaseIdentifier databaseIdentifier = mock.Create<IDatabaseIdentifier>();

                LicenseCipherKey testObject = new LicenseCipherKey(databaseIdentifier);
                Assert.Equal(expectedIV, testObject.InitializationVector);
            }
        }

        [Fact]
        public void Key_ReturnsCorrectValue()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Guid dbGuid = Guid.NewGuid();

                mock.Mock<IDatabaseIdentifier>().Setup(id => id.Get()).Returns(dbGuid);
                IDatabaseIdentifier databaseIdentifier = mock.Create<IDatabaseIdentifier>();

                LicenseCipherKey testObject = new LicenseCipherKey(databaseIdentifier);
                Assert.Equal(dbGuid.ToByteArray(), testObject.Key);
            }
        }

        [Fact]
        public void Key_ThrowsEncryptionException_WhenDatabaseIdentifierExceptionIsThrown()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IDatabaseIdentifier>().Setup(id => id.Get()).Throws(new DatabaseIdentifierException());
                IDatabaseIdentifier databaseIdentifier = mock.Create<IDatabaseIdentifier>();

                LicenseCipherKey testObject = new LicenseCipherKey(databaseIdentifier);
                Assert.Throws<EncryptionException>(() => testObject.Key);
            }
        }

        [Fact]
        public void Key_GetsDatabaseIdentifier()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var id = mock.Mock<IDatabaseIdentifier>();

                var key = new LicenseCipherKey(id.Object).Key;

                id.Verify(d => d.Get(), Times.Once);
            }
        }
    }
}
