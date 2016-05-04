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
        [Fact]
        public void AddCredentials_WithLegacyStore_AddsEmailRequestVariable()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string email = "abcfake@email.com";
                string password = "fakePassword123";

                SearsStoreEntity store = new SearsStoreEntity
                {
                    Email = email,
                    Password = SecureText.Encrypt(password, email)
                };

                HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
                TypedParameter dateTimeProvider = new TypedParameter(typeof(IDateTimeProvider), new DateTimeProvider());
                TypedParameter storeParameter = new TypedParameter(typeof(SearsStoreEntity), store);
                TypedParameter requestParameter = new TypedParameter(typeof(HttpVariableRequestSubmitter), request);

                Mock<IDateTimeProvider> dateTime = mock.Mock<IDateTimeProvider>();
                var testObject = mock.Create<SearsCredentials>(new[] { storeParameter, requestParameter, dateTimeProvider});

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
                    Email = email,
                    Password = SecureText.Encrypt(password, email)
                };
                

                var secureTextEncryptionProvider = mock.Mock<IEncryptionProvider>();
                secureTextEncryptionProvider
                    .Setup(p => p.Decrypt(It.Is<string>(s => s == store.Password)))
                    .Returns(password);
                Func<string, IEncryptionProvider> provideFunc = s => secureTextEncryptionProvider.Object;

                HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
                TypedParameter storeParameter = new TypedParameter(typeof(SearsStoreEntity), store);
                TypedParameter requestParameter = new TypedParameter(typeof(HttpVariableRequestSubmitter), request);
                TypedParameter secureTextEncryptionProviderParameter = new TypedParameter(typeof(Func<string, IEncryptionProvider>), provideFunc);

                SearsCredentials testObject = mock.Create<SearsCredentials>(new[] { storeParameter, requestParameter, secureTextEncryptionProviderParameter });
                
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
                    Email = email,
                    Password = SecureText.Encrypt(password, email)
                };

                HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
                TypedParameter storeParameter = new TypedParameter(typeof(SearsStoreEntity), store);
                TypedParameter requestParameter = new TypedParameter(typeof(HttpVariableRequestSubmitter), request);

                SearsCredentials testObject = mock.Create<SearsCredentials>(new[] { storeParameter, requestParameter });

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
                mock.Mock<IEncryptionProvider>()
                    .Setup(e => e.Decrypt(It.IsAny<string>())).Returns("blah");

                string sellerId = "mySellerId";

                var store = new SearsStoreEntity
                {
                    SellerID = sellerId
                };

                HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
                TypedParameter dateTimeProvider = new TypedParameter(typeof(IDateTimeProvider), new DateTimeProvider());
                TypedParameter storeParameter = new TypedParameter(typeof(SearsStoreEntity), store);
                TypedParameter requestParameter = new TypedParameter(typeof(HttpVariableRequestSubmitter), request);

                SearsCredentials testObject = mock.Create<SearsCredentials>(new[] { storeParameter, requestParameter, dateTimeProvider });

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
                mock.Mock<IEncryptionProvider>()
                    .Setup(e => e.Decrypt(It.IsAny<string>())).Returns("blah");

                string sellerId = "mySellerId";

                var store = new SearsStoreEntity
                {
                    SellerID = sellerId
                };

                HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
                TypedParameter dateTimeProvider = new TypedParameter(typeof(IDateTimeProvider), new DateTimeProvider());
                TypedParameter storeParameter = new TypedParameter(typeof(SearsStoreEntity), store);
                TypedParameter requestParameter = new TypedParameter(typeof(HttpVariableRequestSubmitter), request);

                SearsCredentials testObject = mock.Create<SearsCredentials>(new[] { storeParameter, requestParameter, dateTimeProvider });

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

                var store = new SearsStoreEntity
                {
                    SellerID = sellerId
                };

                mock.Mock<IEncryptionProvider>()
                    .Setup(e => e.Decrypt(It.IsAny<string>())).Returns("blah");

                HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
                TypedParameter dateTimeProvider = new TypedParameter(typeof(IDateTimeProvider), new DateTimeProvider());
                TypedParameter storeParameter = new TypedParameter(typeof(SearsStoreEntity), store);
                TypedParameter requestParameter = new TypedParameter(typeof(HttpVariableRequestSubmitter), request);

                SearsCredentials testObject = mock.Create<SearsCredentials>(new[] { storeParameter, requestParameter, dateTimeProvider });

                testObject.AddCredentials();

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
                string expectedHash = "c3e49bac14d3a39819aa9eae7f72f66d0438e0d539989415bf9db7e6933b7de1";
                var now = new DateTime(2016, 5, 2, 16, 20, 00, DateTimeKind.Utc);

                mock.Mock<IDateTimeProvider>()
                    .Setup(d => d.UtcNow)
                    .Returns(now);

                mock.Mock<IEncryptionProvider>()
                    .Setup(e => e.Decrypt(It.IsAny<string>())).Returns(secretKey);

                var store = new SearsStoreEntity
                {
                    SellerID = sellerId,
                    SecretKey = secretKey,
                    Email = emailAddress
                };

                HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
                TypedParameter storeParameter = new TypedParameter(typeof(SearsStoreEntity), store);
                TypedParameter requestParameter = new TypedParameter(typeof(HttpVariableRequestSubmitter), request);

                SearsCredentials testObject = mock.Create<SearsCredentials>(new[] { storeParameter, requestParameter });

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
                mock.Mock<IEncryptionProvider>()
                    .Setup(e => e.Decrypt(It.IsAny<string>())).Returns("blah");

                string sellerId = "mySellerId";

                var store = new SearsStoreEntity
                {
                    SellerID = sellerId
                };

                HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
                TypedParameter dateTimeProvider = new TypedParameter(typeof(IDateTimeProvider), new DateTimeProvider());
                TypedParameter storeParameter = new TypedParameter(typeof(SearsStoreEntity), store);
                TypedParameter requestParameter = new TypedParameter(typeof(HttpVariableRequestSubmitter), request);

                SearsCredentials testObject = mock.Create<SearsCredentials>(new[] { storeParameter, requestParameter, dateTimeProvider });

                testObject.AddCredentials();

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

                string sellerId = "mySellerId";

                var store = new SearsStoreEntity
                {
                    SellerID = sellerId,
                    Email = storeEmail
                };

                HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
                TypedParameter dateTimeProvider = new TypedParameter(typeof(IDateTimeProvider), new DateTimeProvider());
                TypedParameter storeParameter = new TypedParameter(typeof(SearsStoreEntity), store);
                TypedParameter requestParameter = new TypedParameter(typeof(HttpVariableRequestSubmitter), request);

                SearsCredentials testObject = mock.Create<SearsCredentials>(new[] { storeParameter, requestParameter, dateTimeProvider });

                testObject.AddCredentials();

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

                string sellerId = "mySellerId";

                var store = new SearsStoreEntity
                {
                    SellerID = sellerId
                };

                HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
                TypedParameter storeParameter = new TypedParameter(typeof(SearsStoreEntity), store);
                TypedParameter requestParameter = new TypedParameter(typeof(HttpVariableRequestSubmitter), request);

                SearsCredentials testObject = mock.Create<SearsCredentials>(new[] { storeParameter, requestParameter });

                testObject.AddCredentials();

                string headerValue = request.Headers["authorization"];
                string timeStamp = headerValue.Split(',')[1].Split('=')[1];

                Assert.Equal(now.AddMinutes(-15), DateTime.Parse(timeStamp).ToUniversalTime());
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

                string sellerId = "mySellerId";
                string secretKey = "mySecretKey";
                var store = new SearsStoreEntity
                {
                    SellerID = sellerId,
                    SecretKey = secretKey
                };

                HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
                TypedParameter storeParameter = new TypedParameter(typeof(SearsStoreEntity), store);
                TypedParameter requestParameter = new TypedParameter(typeof(HttpVariableRequestSubmitter), request);

                SearsCredentials testObject = mock.Create<SearsCredentials>(new[] { storeParameter, requestParameter });

                testObject.AddCredentials();

                encryptionProvider.Verify(p => p.Decrypt(It.Is<string>(s => s == secretKey)));
            }
        }

        [Fact]
        public void AddCredentials_ThrowsSearsException_WhenEncryptionProviderThrowsEncryptionException()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IEncryptionProvider>()
                    .Setup(e => e.Decrypt(It.IsAny<string>()))
                    .Throws<EncryptionException>();

                string sellerId = "mySellerId";

                var store = new SearsStoreEntity
                {
                    SellerID = sellerId
                };

                HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
                TypedParameter dateTimeProvider = new TypedParameter(typeof(IDateTimeProvider), new DateTimeProvider());
                TypedParameter storeParameter = new TypedParameter(typeof(SearsStoreEntity), store);
                TypedParameter requestParameter = new TypedParameter(typeof(HttpVariableRequestSubmitter), request);

                SearsCredentials testObject = mock.Create<SearsCredentials>(new[] { storeParameter, requestParameter, dateTimeProvider });

                Assert.Throws<SearsException>(() => testObject.AddCredentials());
            }
        }

        [Fact]
        public void SearsCredentials_WithNullDateTimeProvider_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new SearsCredentials(null, null, null, null, null));
        }
    }
}
