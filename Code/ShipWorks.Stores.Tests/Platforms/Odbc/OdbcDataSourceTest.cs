using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Stores.Platforms.Odbc;
using System;
using System.Data;
using Interapptive.Shared.Security;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc
{
    public class OdbcDataSourceTest
    {
        [Fact]
        public void Restore_SetsUsername()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string prviousState = "{\"Name\":\"Custom...\",\"Username\":\"Foo\",\"Password\":\"Bar\",\"IsCustom\":true,\"ConnectionString\":\"CustomConnectionString\"}";

                Mock<IEncryptionProvider> encryptionProvider = mock.Mock<IEncryptionProvider>();
                encryptionProvider.Setup(e => e.Encrypt(It.IsAny<string>())).Returns<string>(x => x);
                encryptionProvider.Setup(e => e.Decrypt(It.IsAny<string>())).Returns<string>(x => x);

                Mock<IEncryptionProviderFactory> encryptionProviderFactory = mock.Mock<IEncryptionProviderFactory>();
                encryptionProviderFactory.Setup(e => e.CreateOdbcEncryptionProvider()).Returns(encryptionProvider.Object);

                OdbcDataSource dataSource = mock.Create<OdbcDataSource>();

                dataSource.Restore(prviousState);

                Assert.Equal("Foo", dataSource.Username);
            }
        }

        [Fact]
        public void Restore_SetsPassword()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string prviousState = "{\"Name\":\"Custom...\",\"Username\":\"Foo\",\"Password\":\"Bar\",\"IsCustom\":true,\"ConnectionString\":\"CustomConnectionString\"}";

                Mock<IEncryptionProvider> encryptionProvider = mock.Mock<IEncryptionProvider>();
                encryptionProvider.Setup(e => e.Encrypt(It.IsAny<string>())).Returns<string>(x => x);
                encryptionProvider.Setup(e => e.Decrypt(It.IsAny<string>())).Returns<string>(x => x);

                Mock<IEncryptionProviderFactory> encryptionProviderFactory = mock.Mock<IEncryptionProviderFactory>();
                encryptionProviderFactory.Setup(e => e.CreateOdbcEncryptionProvider()).Returns(encryptionProvider.Object);

                OdbcDataSource dataSource = mock.Create<OdbcDataSource>();

                dataSource.Restore(prviousState);

                Assert.Equal("Bar", dataSource.Password);
            }
        }

        [Fact]
        public void Restore_SetsName()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string prviousState = "{\"Name\":\"Custom...\",\"Username\":null,\"Password\":null,\"IsCustom\":true,\"ConnectionString\":\"CustomConnectionString\"}";

                Mock<IEncryptionProvider> encryptionProvider = mock.Mock<IEncryptionProvider>();
                encryptionProvider.Setup(e => e.Encrypt(It.IsAny<string>())).Returns<string>(x => x);
                encryptionProvider.Setup(e => e.Decrypt(It.IsAny<string>())).Returns<string>(x => x);

                Mock<IEncryptionProviderFactory> encryptionProviderFactory = mock.Mock<IEncryptionProviderFactory>();
                encryptionProviderFactory.Setup(e => e.CreateOdbcEncryptionProvider()).Returns(encryptionProvider.Object);

                OdbcDataSource dataSource = mock.Create<OdbcDataSource>();

                dataSource.Restore(prviousState);

                Assert.Equal("Custom...", dataSource.Name);
            }
        }

        [Fact]
        public void Restore_SetsIsCustom()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string prviousState = "{\"Name\":\"Custom...\",\"Username\":null,\"Password\":null,\"IsCustom\":true,\"ConnectionString\":\"CustomConnectionString\"}";

                Mock<IEncryptionProvider> encryptionProvider = mock.Mock<IEncryptionProvider>();
                encryptionProvider.Setup(e => e.Encrypt(It.IsAny<string>())).Returns<string>(x => x);
                encryptionProvider.Setup(e => e.Decrypt(It.IsAny<string>())).Returns<string>(x => x);

                Mock<IEncryptionProviderFactory> encryptionProviderFactory = mock.Mock<IEncryptionProviderFactory>();
                encryptionProviderFactory.Setup(e => e.CreateOdbcEncryptionProvider()).Returns(encryptionProvider.Object);

                OdbcDataSource dataSource = mock.Create<OdbcDataSource>();

                dataSource.Restore(prviousState);

                Assert.True(dataSource.IsCustom);
            }
        }

        [Fact]
        public void Restore_SetsConnectionString()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string prviousState = "{\"Name\":\"Custom...\",\"Username\":null,\"Password\":null,\"IsCustom\":true,\"ConnectionString\":\"CustomConnectionString\"}";

                Mock<IEncryptionProvider> encryptionProvider = mock.Mock<IEncryptionProvider>();
                encryptionProvider.Setup(e => e.Encrypt(It.IsAny<string>())).Returns<string>(x => x);
                encryptionProvider.Setup(e => e.Decrypt(It.IsAny<string>())).Returns<string>(x => x);

                Mock<IEncryptionProviderFactory> encryptionProviderFactory = mock.Mock<IEncryptionProviderFactory>();
                encryptionProviderFactory.Setup(e => e.CreateOdbcEncryptionProvider()).Returns(encryptionProvider.Object);

                OdbcDataSource dataSource = mock.Create<OdbcDataSource>();

                dataSource.Restore(prviousState);

                Assert.Equal("CustomConnectionString", dataSource.ConnectionString);
            }
        }

        [Fact]
        public void Name_WhenDataSourceIsNotCustom_IsDsn()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IEncryptionProvider> encryptionProvider = mock.Mock<IEncryptionProvider>();
                Mock<IEncryptionProviderFactory> encryptionProviderFactory = mock.Mock<IEncryptionProviderFactory>();
                encryptionProviderFactory.Setup(e => e.CreateOdbcEncryptionProvider()).Returns(encryptionProvider.Object);

                OdbcDataSource testObject = mock.Create<OdbcDataSource>();

                testObject.ChangeConnection("Some Dsn", string.Empty, string.Empty);

                Assert.Equal("Some Dsn", testObject.Name);
            }
        }

        [Fact]
        public void Name_WhenDataSourceIsCustom_IsCustom()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IEncryptionProvider> encryptionProvider = mock.Mock<IEncryptionProvider>();
                Mock<IEncryptionProviderFactory> encryptionProviderFactory = mock.Mock<IEncryptionProviderFactory>();
                encryptionProviderFactory.Setup(e => e.CreateOdbcEncryptionProvider()).Returns(encryptionProvider.Object);

                OdbcDataSource testObject = mock.Create<OdbcDataSource>();

                testObject.ChangeConnection("Custom connection string");

                Assert.Equal("Custom...", testObject.Name);
            }
        }

        [Fact]
        public void ConnectionString_DoesNotContainPasswordWhenUsernameIsBlank()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IEncryptionProvider> encryptionProvider = mock.Mock<IEncryptionProvider>();
                Mock<IEncryptionProviderFactory> encryptionProviderFactory = mock.Mock<IEncryptionProviderFactory>();
                encryptionProviderFactory.Setup(e => e.CreateOdbcEncryptionProvider()).Returns(encryptionProvider.Object);

                OdbcDataSource testObject = mock.Create<OdbcDataSource>();

                testObject.ChangeConnection("dsn", "username", string.Empty);

                Assert.DoesNotContain("Pwd", testObject.ConnectionString);
            }
        }

        [Fact]
        public void ConnectionString_DoesNotContainDsnWhenUsernameIsBlank()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IEncryptionProvider> encryptionProvider = mock.Mock<IEncryptionProvider>();
                Mock<IEncryptionProviderFactory> encryptionProviderFactory = mock.Mock<IEncryptionProviderFactory>();
                encryptionProviderFactory.Setup(e => e.CreateOdbcEncryptionProvider()).Returns(encryptionProvider.Object);

                OdbcDataSource testObject = mock.Create<OdbcDataSource>();

                testObject.ChangeConnection(string.Empty, "username", "password");

                Assert.DoesNotContain("DSN", testObject.ConnectionString);
            }
        }

        [Fact]
        public void ConnectionString_DoesNotContainUsernameWhenUsernameIsBlank()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IEncryptionProvider> encryptionProvider = mock.Mock<IEncryptionProvider>();
                Mock<IEncryptionProviderFactory> encryptionProviderFactory = mock.Mock<IEncryptionProviderFactory>();
                encryptionProviderFactory.Setup(e => e.CreateOdbcEncryptionProvider()).Returns(encryptionProvider.Object);

                OdbcDataSource testObject = mock.Create<OdbcDataSource>();

                testObject.ChangeConnection("dsn", string.Empty, "password");

                Assert.DoesNotContain("Uid", testObject.ConnectionString);
            }
        }

        [Fact]
        public void ChangeConnection_WithConnectionString_SetsIsCustomToTrue()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IEncryptionProvider> encryptionProvider = mock.Mock<IEncryptionProvider>();
                Mock<IEncryptionProviderFactory> encryptionProviderFactory = mock.Mock<IEncryptionProviderFactory>();
                encryptionProviderFactory.Setup(e => e.CreateOdbcEncryptionProvider()).Returns(encryptionProvider.Object);

                OdbcDataSource testObject = mock.Create<OdbcDataSource>();

                testObject.ChangeConnection("CustomConnectionString");

                Assert.True(testObject.IsCustom);
            }
        }

        [Fact]
        public void ChangeConnection_WithDsnUsernamePassword_SetsIsCustomToFalse()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IEncryptionProvider> encryptionProvider = mock.Mock<IEncryptionProvider>();
                Mock<IEncryptionProviderFactory> encryptionProviderFactory = mock.Mock<IEncryptionProviderFactory>();
                encryptionProviderFactory.Setup(e => e.CreateOdbcEncryptionProvider()).Returns(encryptionProvider.Object);

                OdbcDataSource testObject = mock.Create<OdbcDataSource>();

                testObject.ChangeConnection("dsn","username","password");

                Assert.False(testObject.IsCustom);
            }
        }

        [Fact]
        public void Seralize_DelegatesToEncrypt_AndCallsEncrypt()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IEncryptionProvider> encryptionProvider = mock.Mock<IEncryptionProvider>();
                Mock<IEncryptionProviderFactory> encryptionProviderFactory = mock.Mock<IEncryptionProviderFactory>();
                encryptionProviderFactory.Setup(e => e.CreateOdbcEncryptionProvider()).Returns(encryptionProvider.Object);

                OdbcDataSource testObject = mock.Create<OdbcDataSource>();

                string connectionString = testObject.Serialize();

                encryptionProvider.Verify(ep=>ep.Encrypt(It.IsAny<string>()), Times.Once);
            }
        }

        [Fact]
        public void Seralize_DelegatesToEncrypt_AndContainsDsn_WhenDsnProvided()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IEncryptionProvider> encryptionProvider = mock.Mock<IEncryptionProvider>();
                Mock<IEncryptionProviderFactory> encryptionProviderFactory = mock.Mock<IEncryptionProviderFactory>();
                encryptionProviderFactory.Setup(e => e.CreateOdbcEncryptionProvider()).Returns(encryptionProvider.Object);

                OdbcDataSource testObject = mock.Create<OdbcDataSource>();
                testObject.ChangeConnection("blah", string.Empty, string.Empty);

                string connectionString = testObject.Serialize();

                encryptionProvider.Verify(ep => ep.Encrypt(
                    It.Is<string>(str=>str.Contains("\"Name\":\"blah\""))), Times.Once);
            }
        }

        [Fact]
        public void Seralize_DelegatesToEncrypt_AndContainsUsername_WhenUsernameProvided()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IEncryptionProvider> encryptionProvider = mock.Mock<IEncryptionProvider>();
                Mock<IEncryptionProviderFactory> encryptionProviderFactory = mock.Mock<IEncryptionProviderFactory>();
                encryptionProviderFactory.Setup(e => e.CreateOdbcEncryptionProvider()).Returns(encryptionProvider.Object);

                OdbcDataSource testObject = mock.Create<OdbcDataSource>();
                testObject.ChangeConnection(string.Empty, "blah", string.Empty);

                string connectionString = testObject.Serialize();

                encryptionProvider.Verify(ep => ep.Encrypt(
                    It.Is<string>(str => str.Contains("\"Username\":\"blah\""))), Times.Once);
            }
        }

        [Fact]
        public void Seralize_DelegatesToEncrypt_AndContainsPassword_WhenPasswordProvided()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IEncryptionProvider> encryptionProvider = mock.Mock<IEncryptionProvider>();
                Mock<IEncryptionProviderFactory> encryptionProviderFactory = mock.Mock<IEncryptionProviderFactory>();
                encryptionProviderFactory.Setup(e => e.CreateOdbcEncryptionProvider()).Returns(encryptionProvider.Object);

                OdbcDataSource testObject = mock.Create<OdbcDataSource>();
                testObject.ChangeConnection(string.Empty, string.Empty, "blah");

                string connectionString = testObject.Serialize();

                encryptionProvider.Verify(ep => ep.Encrypt(
                    It.Is<string>(str => str.Contains("\"Password\":\"blah\""))), Times.Once);
            }
        }

        [Fact]
        public void Seralize_DelegatesToEncrypt_AndDoesNotContainPassword()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IEncryptionProvider> encryptionProvider = mock.Mock<IEncryptionProvider>();
                Mock<IEncryptionProviderFactory> encryptionProviderFactory = mock.Mock<IEncryptionProviderFactory>();
                encryptionProviderFactory.Setup(e => e.CreateOdbcEncryptionProvider()).Returns(encryptionProvider.Object);

                OdbcDataSource testObject = mock.Create<OdbcDataSource>();

                string connectionString = testObject.Serialize();

                encryptionProvider.Verify(ep => ep.Encrypt(
                    It.Is<string>(str => !str.Contains("Pwd="))), Times.Once);
            }
        }

        [Fact]
        public void Seralize_DelegatesToEncrypt_AndDoesNotContainUsername()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IEncryptionProvider> encryptionProvider = mock.Mock<IEncryptionProvider>();
                Mock<IEncryptionProviderFactory> encryptionProviderFactory = mock.Mock<IEncryptionProviderFactory>();
                encryptionProviderFactory.Setup(e => e.CreateOdbcEncryptionProvider()).Returns(encryptionProvider.Object);

                OdbcDataSource testObject = mock.Create<OdbcDataSource>();

                string connectionString = testObject.Serialize();

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

                var odbcProvider = mock.Mock<IShipWorksDbProviderFactory>();
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

                var odbcProvider = mock.Mock<IShipWorksDbProviderFactory>();
                odbcProvider.Setup(p => p.CreateOdbcConnection())
                    .Returns(connection.Object);

                var testObject = mock.Create<OdbcDataSource>();
                testObject.ChangeConnection("blip", string.Empty, string.Empty);

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

                var odbcProvider = mock.Mock<IShipWorksDbProviderFactory>();
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

                var odbcProvider = mock.Mock<IShipWorksDbProviderFactory>();
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

                var odbcProvider = mock.Mock<IShipWorksDbProviderFactory>();
                odbcProvider.Setup(p => p.CreateOdbcConnection())
                    .Returns(connection.Object);

                var testObject = mock.Create<OdbcDataSource>();

                var testResult = testObject.TestConnection();

                Assert.Equal("bloop", testResult.Message);
            }
        }
    }
}
