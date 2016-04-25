using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Stores.Platforms.Odbc;
using System;
using System.Data;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc
{
    public class OdbcDataSourceTest
    {
        [Fact]
        public void ConnectionString_DelegatesToEncrypt_AndCallsEncrypt()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var encryptionProvider = mock.Mock<IEncryptionProvider>();

                OdbcDataSource testObject = mock.Create<OdbcDataSource>();

                string connectionString = testObject.ConnectionString;

                encryptionProvider.Verify(ep=>ep.Encrypt(It.IsAny<string>()), Times.Once);
            }
        }

        [Fact]
        public void ConnectionString_DelegatesToEncrypt_AndContainsDsn_WhenDsnProvided()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var encryptionProvider = mock.Mock<IEncryptionProvider>();

                OdbcDataSource testObject = mock.Create<OdbcDataSource>();
                testObject.Name = "blah";

                string connectionString = testObject.ConnectionString;

                encryptionProvider.Verify(ep => ep.Encrypt(
                    It.Is<string>(str=>str.Contains("DSN=blah;"))), Times.Once);
            }
        }

        [Fact]
        public void ConnectionString_DelegatesToEncrypt_AndContainsUsername_WhenUsernameProvided()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var encryptionProvider = mock.Mock<IEncryptionProvider>();

                OdbcDataSource testObject = mock.Create<OdbcDataSource>();
                testObject.Username = "blah";

                string connectionString = testObject.ConnectionString;

                encryptionProvider.Verify(ep => ep.Encrypt(
                    It.Is<string>(str => str.Contains("Uid=blah;"))), Times.Once);
            }
        }

        [Fact]
        public void ConnectionString_DelegatesToEncrypt_AndContainsPassword_WhenPasswordProvided()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var encryptionProvider = mock.Mock<IEncryptionProvider>();

                OdbcDataSource testObject = mock.Create<OdbcDataSource>();
                testObject.Password = "blah";

                string connectionString = testObject.ConnectionString;

                encryptionProvider.Verify(ep => ep.Encrypt(
                    It.Is<string>(str => str.Contains("Pwd=blah;"))), Times.Once);
            }
        }

        [Fact]
        public void ConnectionString_DelegatesToEncrypt_AndDoesNotContainPassword()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var encryptionProvider = mock.Mock<IEncryptionProvider>();

                OdbcDataSource testObject = mock.Create<OdbcDataSource>();
                
                string connectionString = testObject.ConnectionString;

                encryptionProvider.Verify(ep => ep.Encrypt(
                    It.Is<string>(str => !str.Contains("Pwd="))), Times.Once);
            }
        }

        [Fact]
        public void ConnectionString_DelegatesToEncrypt_AndDoesNotContainUsername()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var encryptionProvider = mock.Mock<IEncryptionProvider>();

                OdbcDataSource testObject = mock.Create<OdbcDataSource>();

                string connectionString = testObject.ConnectionString;

                encryptionProvider.Verify(ep => ep.Encrypt(
                    It.Is<string>(str => !str.Contains("Uid="))), Times.Once);
            }
        }

        [Fact]
        public void TestConnection_CallsOpenConnection()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var connection = mock.Mock<IDbConnection>();

                var odbcProvider = mock.Mock<IShipWorksOdbcProvider>();
                odbcProvider.Setup(p => p.CreateOdbcConnection())
                    .Returns(connection.Object);

                var testObject = mock.Create<OdbcDataSource>();

                var testResult = testObject.TestConnection();

                connection.Verify(con => con.Open(), Times.Once);
            }
        }

        [Fact]
        public void TestConnection_ConnectionStringHasDsnName()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var connection = mock.Mock<IDbConnection>();

                var odbcProvider = mock.Mock<IShipWorksOdbcProvider>();
                odbcProvider.Setup(p => p.CreateOdbcConnection())
                    .Returns(connection.Object);

                var testObject = mock.Create<OdbcDataSource>();
                testObject.Name = "blip";

                var testResult = testObject.TestConnection();

                connection.VerifySet(con => con.ConnectionString = "DSN=blip;", Times.Once());
            }
        }

        [Fact]
        public void TestConnection_ReturnsSuccess_WhenSuccessfullyConnects()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var connection = mock.Mock<IDbConnection>();

                var odbcProvider = mock.Mock<IShipWorksOdbcProvider>();
                odbcProvider.Setup(p => p.CreateOdbcConnection())
                    .Returns(connection.Object);

                var testObject = mock.Create<OdbcDataSource>();

                var testResult = testObject.TestConnection();

                Assert.Equal(true, testResult.Success);
            }
        }

        [Fact]
        public void TestConnection_ReturnsFailure_WhenConnectionOpenThrows()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var connection = mock.Mock<IDbConnection>();
                connection.Setup(con => con.Open())
                    .Throws<Exception>();

                var odbcProvider = mock.Mock<IShipWorksOdbcProvider>();
                odbcProvider.Setup(p => p.CreateOdbcConnection())
                    .Returns(connection.Object);

                var testObject = mock.Create<OdbcDataSource>();

                var testResult = testObject.TestConnection();

                Assert.Equal(false, testResult.Success);
            }
        }

        [Fact]
        public void TestConnection_ReturnsMessage_WhenConnectionOpenThrows()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var connection = mock.Mock<IDbConnection>();
                connection.Setup(con => con.Open())
                    .Throws(new Exception("bloop"));

                var odbcProvider = mock.Mock<IShipWorksOdbcProvider>();
                odbcProvider.Setup(p => p.CreateOdbcConnection())
                    .Returns(connection.Object);

                var testObject = mock.Create<OdbcDataSource>();

                var testResult = testObject.TestConnection();

                Assert.Equal("bloop", testResult.Message);
            }
        }
    }
}
