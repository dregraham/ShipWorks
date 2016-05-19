using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Common;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Sears;
using System;
using System.Linq;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Sears
{
    public class SearsCredentialsTest
    {
        DateTime testDateTime = new DateTime(2016, 5, 4, 16, 20, 32, DateTimeKind.Utc);

        [Fact]
        public void AddCredentials_AddsEmailRequestVariable_WhenLegacyStore()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string email = "abcfake@email.com";
                string password = "fakePassword123";

                SearsStoreEntity store = new SearsStoreEntity
                {
                    SearsEmail = email,
                    Password = SecureText.Encrypt(password, email)
                };

                mock.Mock<IDateTimeProvider>().SetupGet(d => d.UtcNow).Returns(testDateTime);

                SetupSecureTextProvider(mock, email, password, store);

                HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
                TypedParameter storeParameter = new TypedParameter(typeof(SearsStoreEntity), store);
                TypedParameter requestParameter = new TypedParameter(typeof(HttpVariableRequestSubmitter), request);

                var testObject = mock.Create<SearsCredentials>(storeParameter, requestParameter);

                testObject.AddCredentials();
                HttpVariable emailVariable = request.Variables.First(v => v.Name == "email");

                Assert.Equal(email, emailVariable.Value);
            }
        }

        [Fact]
        public void AddCredentials_AddsPasswordRequestVariable_WhenLegacyStore()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string email = "abcfake@email.com";
                string password = "fakePassword123";

                var store = new SearsStoreEntity
                {
                    SearsEmail = email,
                    Password = SecureText.Encrypt(password, email)
                };

                SetupSecureTextProvider(mock, email, password, store);

                HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
                TypedParameter storeParameter = new TypedParameter(typeof(SearsStoreEntity), store);
                TypedParameter requestParameter = new TypedParameter(typeof(HttpVariableRequestSubmitter), request);

                SearsCredentials testObject = mock.Create<SearsCredentials>(storeParameter, requestParameter);

                testObject.AddCredentials();
                HttpVariable passwordVariable = request.Variables.First(v => v.Name == "password");

                Assert.Equal(password, passwordVariable.Value);
            }
        }

        [Fact]
        public void AddCredentials_DoesNotAddSellerIdVariable_WhenLegacyStore()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string email = "abcfake@email.com";
                string password = "fakePassword123";

                var store = new SearsStoreEntity
                {
                    SearsEmail = email,
                    Password = SecureText.Encrypt(password, email)
                };

                SetupSecureTextProvider(mock, email, password, store);

                HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
                TypedParameter storeParameter = new TypedParameter(typeof(SearsStoreEntity), store);
                TypedParameter requestParameter = new TypedParameter(typeof(HttpVariableRequestSubmitter), request);

                SearsCredentials testObject = mock.Create<SearsCredentials>(storeParameter, requestParameter);

                testObject.AddCredentials();
                HttpVariable sellerIdVariable = request.Variables.FirstOrDefault(v => v.Name == "sellerId");

                Assert.Null(sellerIdVariable);
            }
        }

        [Fact]
        public void AddCredentials_AddsSellerIdToVariable_WhenNewStore()
        {
            using (var mock = AutoMock.GetLoose())
            {

                string sellerId = "mySellerId";

                var store = new SearsStoreEntity
                {
                    SellerID = sellerId,
                    SecretKey = "encrypted secret key"
                };

                SetupSearsEncryptionProvider(mock, store.SecretKey);
                SetupDateTime(mock);

                HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
                TypedParameter storeParameter = new TypedParameter(typeof(SearsStoreEntity), store);
                TypedParameter requestParameter = new TypedParameter(typeof(HttpVariableRequestSubmitter), request);

                SearsCredentials testObject = mock.Create<SearsCredentials>(storeParameter, requestParameter);

                testObject.AddCredentials();
                HttpVariable sellerIdVariable = request.Variables.Single(v => v.Name == "sellerId");

                Assert.Equal(sellerId, sellerIdVariable.Value);
            }
        }

        [Fact]
        public void AddCredentials_DoesNotAddEmailRequestVariable_WhenNewStore()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string sellerId = "mySellerId";

                var store = new SearsStoreEntity
                {
                    SellerID = sellerId,
                    SecretKey = "encrypted key"
                };

                SetupSearsEncryptionProvider(mock, store.SecretKey);
                SetupDateTime(mock);

                HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
                TypedParameter storeParameter = new TypedParameter(typeof(SearsStoreEntity), store);
                TypedParameter requestParameter = new TypedParameter(typeof(HttpVariableRequestSubmitter), request);

                SearsCredentials testObject = mock.Create<SearsCredentials>(storeParameter, requestParameter);

                testObject.AddCredentials();
                HttpVariable emailVariable = request.Variables.SingleOrDefault(v => v.Name == "email");

                Assert.Null(emailVariable);
            }
        }

        [Fact]
        public void AddCredentials_AddsAuthorizationHeader_WhenNewStore()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string sellerId = "mySellerId";
                string encryptedSecretKey = "encrypted secret key";

                var store = new SearsStoreEntity
                {
                    SellerID = sellerId,
                    SecretKey = encryptedSecretKey
                };

                SetupSearsEncryptionProvider(mock, encryptedSecretKey);
                SetupDateTime(mock);

                HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();

                TypedParameter storeParameter = new TypedParameter(typeof(SearsStoreEntity), store);
                TypedParameter requestParameter = new TypedParameter(typeof(HttpVariableRequestSubmitter), request);
                SearsCredentials testObject = mock.Create<SearsCredentials>(storeParameter, requestParameter);

                testObject.AddCredentials();

                Assert.NotNull(request.Headers["authorization"]);
            }
        }

        private static void SetupSearsEncryptionProvider(AutoMock mock, string encryptedSecretKey)
        {
            var encryptor = mock.Mock<IEncryptionProvider>();
            encryptor.Setup(e => e.Decrypt(It.Is<string>(s => s == encryptedSecretKey))).Returns("blah");
            mock.Mock<IEncryptionProviderFactory>()
                .Setup(f => f.CreateSearsEncryptionProvider())
                .Returns(encryptor.Object);
        }

        [Fact]
        public void AddCredentials_AuthHeaderSignatureIsCorrect_WhenNewStore()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string secretKey = "syZJ+ak2Qhta3HvYig+3vCjg1Al0b8cqdVqaZr9EbFU=";
                string sellerId = "10173601";
                string emailAddress = "wes@shipworks.com";
                string expectedHash = "1b2a4a430940b1c425f200812dfc6d42d5ace5fd533920565a7eac533761c8ac";

                var store = new SearsStoreEntity
                {
                    SellerID = sellerId,
                    SecretKey = secretKey,
                    SearsEmail = emailAddress
                };

                SetupSearsEncryptionProvider(mock, store.SecretKey);
                SetupDateTime(mock);

                HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
                TypedParameter storeParameter = new TypedParameter(typeof(SearsStoreEntity), store);
                TypedParameter requestParameter = new TypedParameter(typeof(HttpVariableRequestSubmitter), request);

                SearsCredentials testObject = mock.Create<SearsCredentials>(storeParameter, requestParameter);

                testObject.AddCredentials();

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
                string sellerId = "mySellerId";

                var store = new SearsStoreEntity
                {
                    SellerID = sellerId,
                    SecretKey = "its a secret"
                };

                SetupSearsEncryptionProvider(mock, store.SecretKey);
                SetupDateTime(mock);

                HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
                TypedParameter storeParameter = new TypedParameter(typeof(SearsStoreEntity), store);
                TypedParameter requestParameter = new TypedParameter(typeof(HttpVariableRequestSubmitter), request);

                SearsCredentials testObject = mock.Create<SearsCredentials>(storeParameter, requestParameter);

                testObject.AddCredentials();

                Assert.StartsWith("HMAC-SHA256 ", request.Headers["authorization"]);
            }
        }

        [Fact]
        public void AddCredentials_AuthHeaderEmailAddressIsStoreEmailAddress()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var store = new SearsStoreEntity
                {
                    SellerID = "mySellerId",
                    SearsEmail = "blah@shipworks.com",
                    SecretKey = "funky encrypted key"
                };

                SetupSearsEncryptionProvider(mock, store.SecretKey);
                SetupDateTime(mock);

                HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
                TypedParameter storeParameter = new TypedParameter(typeof(SearsStoreEntity), store);
                TypedParameter requestParameter = new TypedParameter(typeof(HttpVariableRequestSubmitter), request);

                SearsCredentials testObject = mock.Create<SearsCredentials>(storeParameter, requestParameter);

                testObject.AddCredentials();

                string headerValue = request.Headers["authorization"];
                string headerEmail = headerValue
                    .Split(' ')[1] // everything after the space
                    .Split(',')[0] // everything before the comma
                    .Split('=')[1]; // the email address

                Assert.Equal(store.SearsEmail, headerEmail);
            }
        }

        [Fact]
        public void AddCredentials_AuthHeaderContainsTimestampsFromProvider()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string sellerId = "mySellerId";

                var store = new SearsStoreEntity
                {
                    SellerID = sellerId,
                    SecretKey = "blahblah"
                };

                SetupSearsEncryptionProvider(mock, store.SecretKey);
                SetupDateTime(mock);

                HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
                TypedParameter storeParameter = new TypedParameter(typeof(SearsStoreEntity), store);
                TypedParameter requestParameter = new TypedParameter(typeof(HttpVariableRequestSubmitter), request);

                SearsCredentials testObject = mock.Create<SearsCredentials>(storeParameter, requestParameter);

                testObject.AddCredentials();

                string headerValue = request.Headers["authorization"];
                string timeStamp = headerValue.Split(',')[1].Split('=')[1];

                Assert.Equal(testDateTime.AddMinutes(-15), DateTime.Parse(timeStamp).ToUniversalTime());
            }
        }


        [Fact]
        public void AddCredentials_DelegatesToIEncryptionProvider()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string sellerId = "mySellerId";
                string secretKey = "mySecretKey";
                var store = new SearsStoreEntity
                {
                    SellerID = sellerId,
                    SecretKey = secretKey
                };

                var encryptionProvider = mock.Mock<IEncryptionProvider>();
                encryptionProvider.Setup(e => e.Decrypt(It.Is<string>(s => s == secretKey))).Returns("blah");

                mock.Mock<IEncryptionProviderFactory>()
                    .Setup(f => f.CreateSearsEncryptionProvider())
                    .Returns(encryptionProvider.Object);

                SetupDateTime(mock);

                HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
                TypedParameter storeParameter = new TypedParameter(typeof(SearsStoreEntity), store);
                TypedParameter requestParameter = new TypedParameter(typeof(HttpVariableRequestSubmitter), request);

                SearsCredentials testObject = mock.Create<SearsCredentials>(storeParameter, requestParameter);

                testObject.AddCredentials();

                encryptionProvider.Verify(p => p.Decrypt(It.Is<string>(s => s == secretKey)));
            }
        }

        [Fact]
        public void AddCredentials_ThrowsSearsException_WhenEncryptionProviderThrowsEncryptionException()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var store = new SearsStoreEntity
                {
                    SellerID = "mySellerId",
                    SecretKey = "encrypted"
                };

                var encryptor = mock.Mock<IEncryptionProvider>();
                encryptor.Setup(e => e.Decrypt(It.Is<string>(s => s == store.SecretKey))).Throws<EncryptionException>();

                mock.Mock<IEncryptionProviderFactory>()
                    .Setup(f => f.CreateSearsEncryptionProvider())
                    .Returns(encryptor.Object);

                HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
                TypedParameter dateTimeProvider = new TypedParameter(typeof(IDateTimeProvider), new DateTimeProvider());
                TypedParameter storeParameter = new TypedParameter(typeof(SearsStoreEntity), store);
                TypedParameter requestParameter = new TypedParameter(typeof(HttpVariableRequestSubmitter), request);

                SearsCredentials testObject = mock.Create<SearsCredentials>(storeParameter, requestParameter, dateTimeProvider);

                Assert.Throws<SearsException>(() => testObject.AddCredentials());
            }
        }

        [Fact]
        public void SearsCredentials_WithNullDateTimeProvider_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new SearsCredentials(null, null, null, null));
        }

        private static void SetupSecureTextProvider(AutoMock mock, string email, string password, SearsStoreEntity store)
        {
            var secureTextEncryptionProvider = mock.Mock<IEncryptionProvider>();
            secureTextEncryptionProvider
                .Setup(p => p.Decrypt(It.Is<string>(s => s == store.Password)))
                .Returns(password);
            mock.Mock<IEncryptionProviderFactory>()
                .Setup(f => f.CreateSecureTextEncryptionProvider(It.Is<string>(s => s == email)))
                .Returns(secureTextEncryptionProvider.Object);
        }

        private void SetupDateTime(AutoMock mock)
        {
            mock.Mock<IDateTimeProvider>().SetupGet(d => d.UtcNow).Returns(testDateTime);
        }
    }
}
