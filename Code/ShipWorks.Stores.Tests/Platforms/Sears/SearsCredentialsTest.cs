using Autofac.Extras.Moq;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Sears;
using System;
using System.Linq;
using Interapptive.Shared.Security;
using Xunit;
namespace ShipWorks.Stores.Tests.Platforms.Sears
{
    public class SearsCredentialsTest
    {
        [Fact]
        public void AddCredentials_WithLegacyStore_AddsEmailRequestVariable()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IDateTimeProvider> dateTime = mock.Mock<IDateTimeProvider>();
                var testObject = mock.Create<SearsCredentials>();

                string email = "abcfake@email.com";
                string password = "fakePassword123";

                var store = new SearsStoreEntity
                {
                    Email = email,
                    Password = SecureText.Encrypt(password, email)
                };

                var request = new HttpVariableRequestSubmitter();
                testObject.AddCredentials(store, request);
                HttpVariable emailVariable = request.Variables.First(v => v.Name == "email");

                Assert.Equal(email, emailVariable.Value);
            }
        }

        [Fact]
        public void AddCredentials_WithLegacyStore_AddsPasswordRequestVariable()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = mock.Create<SearsCredentials>();

                string email = "abcfake@email.com";
                string password = "fakePassword123";

                var store = new SearsStoreEntity
                {
                    Email = email,
                    Password = SecureText.Encrypt(password, email)
                };

                var request = new HttpVariableRequestSubmitter();
                testObject.AddCredentials(store, request);
                HttpVariable passwordVariable = request.Variables.First(v => v.Name == "password");

                Assert.Equal(password, passwordVariable.Value);
            }
        }

        [Fact]
        public void AddCredentials_DoesNotAddSellerIdVariable_WhenLegacyStore()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = mock.Create<SearsCredentials>();

                string email = "abcfake@email.com";
                string password = "fakePassword123";

                var store = new SearsStoreEntity
                {
                    Email = email,
                    Password = SecureText.Encrypt(password, email)
                };

                var request = new HttpVariableRequestSubmitter();
                testObject.AddCredentials(store, request);
                HttpVariable sellerIdVariable = request.Variables.FirstOrDefault(v => v.Name == "sellerId");

                Assert.Null(sellerIdVariable);
            }
        }

        [Fact]
        public void AddCredentials_AddsSellerIdToVariable_WhenNewStore()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IEncryptionProvider>()
                    .Setup(e => e.Decrypt(It.IsAny<string>())).Returns("blah");

                var testObject = mock.Create<SearsCredentials>();

                string sellerId = "mySellerId";

                var store = new SearsStoreEntity
                {
                    SellerID = sellerId
                };

                var request = new HttpVariableRequestSubmitter();
                testObject.AddCredentials(store, request);
                HttpVariable sellerIdVariable = request.Variables.Single(v => v.Name == "sellerId");

                Assert.Equal(sellerId, sellerIdVariable.Value);
            }
        }

        [Fact]
        public void AddCredentials_DoesNotAddEmailRequestVariable_WhenNewStore()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IEncryptionProvider>()
                    .Setup(e => e.Decrypt(It.IsAny<string>())).Returns("blah");

                var testObject = mock.Create<SearsCredentials>();

                string sellerId = "mySellerId";

                var store = new SearsStoreEntity
                {
                    SellerID = sellerId
                };

                var request = new HttpVariableRequestSubmitter();
                testObject.AddCredentials(store, request);
                HttpVariable emailVariable = request.Variables.SingleOrDefault(v => v.Name == "email");

                Assert.Null(emailVariable);
            }
        }

        [Fact]
        public void AddCredentials_AddsAuthorizationHeader_WhenNewStore()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IEncryptionProvider>()
                    .Setup(e => e.Decrypt(It.IsAny<string>())).Returns("blah");

                var testObject = mock.Create<SearsCredentials>();

                string sellerId = "mySellerId";

                var store = new SearsStoreEntity
                {
                    SellerID = sellerId
                };

                var request = new HttpVariableRequestSubmitter();
                testObject.AddCredentials(store, request);

                Assert.NotNull(request.Headers["authorization"]);
            }
        }

        [Fact]
        public void AddCredentials_AuthHeaderSignatureIsCorrect_WhenNewStore()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string secretKey = "syZJ+ak2Qhta3HvYig+3vCjg1Al0b8cqdVqaZr9EbFU=";
                string sellerId = "10173601";
                string emailAddress = "wes@shipworks.com";
                string expectedHash = "d847e945dda8574cdfeaedb69670b3357123af4078fa9bda64651571b4055d9a";
                var now = new DateTime(2016, 5, 2, 16, 20, 00, DateTimeKind.Utc);

                mock.Mock<IDateTimeProvider>()
                    .Setup(d => d.UtcNow)
                    .Returns(now);

                mock.Mock<IEncryptionProvider>()
                    .Setup(e => e.Decrypt(It.IsAny<string>())).Returns(secretKey);

                var testObject = mock.Create<SearsCredentials>();

                var store = new SearsStoreEntity
                {
                    SellerID = sellerId,
                    SecretKey = secretKey,
                    Email = emailAddress
                };

                var request = new HttpVariableRequestSubmitter();
                testObject.AddCredentials(store, request);

                string headerText = request.Headers["authorization"];
                string actualHash = headerText.Split(',')[2].Split('=')[1];

                Assert.Equal(expectedHash, actualHash);
            }
        }

        [Fact]
        public void AddCredentials_AuthHeaderStartsWithHashType()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IEncryptionProvider>()
                    .Setup(e => e.Decrypt(It.IsAny<string>())).Returns("blah");

                var testObject = mock.Create<SearsCredentials>();

                string sellerId = "mySellerId";

                var store = new SearsStoreEntity
                {
                    SellerID = sellerId
                };

                var request = new HttpVariableRequestSubmitter();
                testObject.AddCredentials(store, request);

                Assert.StartsWith("HMAC-SHA256 ", request.Headers["authorization"]);
            }
        }

        [Fact]
        public void AddCredentials_AuthHeaderEmailAddressIsStoreEmailAddress()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string storeEmail = "blah@shipworks.com";

                mock.Mock<IEncryptionProvider>()
                    .Setup(e => e.Decrypt(It.IsAny<string>())).Returns("blah");

                var testObject = mock.Create<SearsCredentials>();

                string sellerId = "mySellerId";

                var store = new SearsStoreEntity
                {
                    SellerID = sellerId,
                    Email = storeEmail
                };

                var request = new HttpVariableRequestSubmitter();
                testObject.AddCredentials(store, request);

                string headerValue = request.Headers["authorization"];
                string headerEmail = headerValue
                    .Split(' ')[1] // everything after the space
                    .Split(',')[0] // everything before the comma
                    .Split('=')[1]; // the email address
                
                Assert.Equal(storeEmail, headerEmail);
            }
        }

        [Fact]
        public void AddCredentials_AuthHeaderContainsTimestampsFromProvider()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var now = new DateTime(2016, 5, 2, 16, 20, 00, DateTimeKind.Utc);

                mock.Mock<IDateTimeProvider>()
                    .Setup(d => d.UtcNow)
                    .Returns(now);

                mock.Mock<IEncryptionProvider>()
                    .Setup(e => e.Decrypt(It.IsAny<string>())).Returns("blah");

                var testObject = mock.Create<SearsCredentials>();

                string sellerId = "mySellerId";

                var store = new SearsStoreEntity
                {
                    SellerID = sellerId
                };

                var request = new HttpVariableRequestSubmitter();
                testObject.AddCredentials(store, request);

                string headerValue = request.Headers["authorization"];
                string timeStamp = headerValue.Split(',')[1].Split('=')[1];

                Assert.Equal(now, DateTime.Parse(timeStamp).ToUniversalTime());
            }
        }


        [Fact]
        public void AddCredentials_DelegatesToIEncryptionProvider()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var now = new DateTime(2016, 5, 2, 16, 20, 00, DateTimeKind.Utc);

                mock.Mock<IDateTimeProvider>()
                    .Setup(d => d.UtcNow)
                    .Returns(now);

                var encryptionProvider = mock.Mock<IEncryptionProvider>();
                encryptionProvider.Setup(e => e.Decrypt(It.IsAny<string>())).Returns("blah");

                var testObject = mock.Create<SearsCredentials>();

                string sellerId = "mySellerId";
                string secretKey = "mySecretKey";
                var store = new SearsStoreEntity
                {
                    SellerID = sellerId,
                    SecretKey = secretKey
                };

                var request = new HttpVariableRequestSubmitter();
                testObject.AddCredentials(store, request);

                encryptionProvider.Verify(p => p.Decrypt(It.Is<string>(s => s == secretKey)));
            }
        }

        [Fact]
        public void SearsCredentials_WithNullDateTimeProvider_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new SearsCredentials(null, null));
        }
    }
}
