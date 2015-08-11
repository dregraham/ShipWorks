using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.iParcel;
using Moq;
using ShipWorks.Shipping.Carriers.iParcel.Net;

namespace ShipWorks.Tests.Shipping.Carriers.iParcel
{
    public class iParcelCredentialsTest
    {
        private iParcelCredentials testObject;

        private Mock<IiParcelServiceGateway> serviceGateway;

        [TestInitialize]
        public void Initialize()
        {
            serviceGateway = new Mock<IiParcelServiceGateway>();
            serviceGateway.Setup(g => g.IsValidUser(It.IsAny<iParcelCredentials>())).Returns(true);

            testObject = new iParcelCredentials(serviceGateway.Object);
        }

        [Fact]
        public void EncryptedPassword_WhenIsEncryptedIsTrue_Test()
        {
            testObject = new iParcelCredentials("someUser", "somePassword", true, serviceGateway.Object);

            // Since the credentials use a static class, we'll just check that we get back the same
            // value for the encrypted password property as the password that was provided (i.e.
            // the password was not encrypted again)
            Assert.AreEqual("somePassword", testObject.EncryptedPassword);
        }

        [Fact]
        public void EncryptedPassword_WhenIsEncryptedIsFalse_Test()
        {
            testObject = new iParcelCredentials("someUser", "somePassword", false, serviceGateway.Object);

            // Since the credentials use a static class, we'll just check that we get a different
            // value for the encrypted password property than the password that was provided (i.e.
            // the value is different)
            Assert.AreNotEqual("somePassword", testObject.EncryptedPassword);
        }

        [Fact]
        public void DecryptedPassword_WhenIsEncryptedIsTrue_Test()
        {
            testObject = new iParcelCredentials("someUser", "D4ljlev6Y5B+KN4tWMzYsw==", true, serviceGateway.Object);

            // Since the credentials use a static class, we'll just check that we get a different
            // value for the decrypted password property than the password that was provided
            Assert.AreNotEqual("D4ljlev6Y5B+KN4tWMzYsw==", testObject.DecryptedPassword);
        }

        [Fact]
        public void DecryptedPassword_WhenIsEncryptedIsFalse_Test()
        {
            testObject = new iParcelCredentials("someUser", "somePassword", false, serviceGateway.Object);

            // Since the credentials use a static class, we'll just check that we get back the same
            // value for the encrypted password property as the password that was provided (i.e.
            // the password was not encrypted again)
            Assert.AreEqual("somePassword", testObject.DecryptedPassword);
        }

        [Fact]
        [ExpectedException(typeof(iParcelException))]
        public void Validate_ThrowsiParcelException_WhenUsernameIsNull_Test()
        {
            testObject = new iParcelCredentials(null, "somePassword", false, serviceGateway.Object);

            testObject.Validate();
        }

        [Fact]
        [ExpectedException(typeof(iParcelException))]
        public void Validate_ThrowsiParcelException_WhenUsernameIsEmpty_Test()
        {
            testObject = new iParcelCredentials(string.Empty, "somePassword", false, serviceGateway.Object);

            testObject.Validate();
        }

        [Fact]
        [ExpectedException(typeof(iParcelException))]
        public void Validate_ThrowsiParcelException_WhenPasswordIsNull_Test()
        {
            testObject = new iParcelCredentials("someUser", null, false, serviceGateway.Object);

            testObject.Validate();
        }

        [Fact]
        [ExpectedException(typeof(iParcelException))]
        public void Validate_ThrowsiParcelException_WhenPasswordIsEmpty_Test()
        {
            testObject = new iParcelCredentials("someUser", string.Empty, false, serviceGateway.Object);

            testObject.Validate();
        }

        [Fact]
        public void Validate_DelegatesToServiceGateway_Test()
        {
            serviceGateway.Setup(g => g.IsValidUser(It.IsAny<iParcelCredentials>())).Returns(true);
            testObject = new iParcelCredentials("someUser", "somePassword", false, serviceGateway.Object);

            testObject.Validate();

            serviceGateway.Verify(g => g.IsValidUser(testObject), Times.Once());
        }

        [Fact]
        [ExpectedException(typeof(iParcelException))]
        public void Validate_ThrowsiParcelException_WhenCredentialsDoNotPassGatewayValidation_Test()
        {
            serviceGateway.Setup(g => g.IsValidUser(It.IsAny<iParcelCredentials>())).Returns(false);
            testObject = new iParcelCredentials("someUser", "somePassword", false, serviceGateway.Object);

            testObject.Validate();
        }

        [Fact]
        public void Validate_ValidCredentials_Test()
        {
            serviceGateway.Setup(g => g.IsValidUser(It.IsAny<iParcelCredentials>())).Returns(true);
            testObject = new iParcelCredentials("someUser", "somePassword", false, serviceGateway.Object);

            // This shouldn't throw an exception
            testObject.Validate();
        }

        [Fact]
        [ExpectedException(typeof(iParcelException))]
        public void SaveToEntity_ThrowsiParcelException_ForInvalidCredentials_Test()
        {
            // Setup our test object with an invalid username
            testObject = new iParcelCredentials(string.Empty, "somePassword", false, serviceGateway.Object);

            testObject.SaveToEntity(new IParcelAccountEntity());
        }

        [Fact]
        public void SaveToEntity_Username_Test()
        {
            IParcelAccountEntity account = new IParcelAccountEntity();
            testObject = new iParcelCredentials("someUser", "somePassword", false, serviceGateway.Object);

            testObject.SaveToEntity(account);

            Assert.AreEqual("someUser", account.Username);
        }

        [Fact]
        public void SaveToEntity_Password_Test()
        {
            IParcelAccountEntity account = new IParcelAccountEntity();
            testObject = new iParcelCredentials("someUser", "somePassword", false, serviceGateway.Object);

            testObject.SaveToEntity(account);

            // This should be the encrypted password
            Assert.AreEqual("D4ljlev6Y5B+KN4tWMzYsw==", account.Password);
        }
    }
}
