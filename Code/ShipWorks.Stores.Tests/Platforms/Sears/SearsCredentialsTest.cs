using Autofac.Extras.Moq;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using Moq;
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
                HttpVariable passwordVariable = request.Variables.First(v => v.Name == "password");

                Assert.Equal(password, passwordVariable.Value);
            }
        }

        [Fact]
        public void SearsCredentials_WithNullDateTimeProvider_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new SearsCredentials(null));
        }
    }
}
