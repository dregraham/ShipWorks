using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.LinkNewAccount.Request.Manipulators;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.WebServices.Registration;
using System;
using System.Collections.Generic;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.LinkNewAccount.Request.Manipulators
{
    public class UpsLinkNewAccountInfoManipulatorTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly ManageAccountRequest manageAccountRequest;
        private readonly UpsAccountEntity upsAccount;
        private readonly UpsLinkNewAccountInfoManipulator testObject;
        private readonly Mock<CarrierRequest> carrierRequest;
        
        public UpsLinkNewAccountInfoManipulatorTest()
        {
            mock = AutoMock.GetLoose();

            manageAccountRequest = new ManageAccountRequest();

            carrierRequest = new Mock<CarrierRequest>(MockBehavior.Loose, new List<ICarrierRequestManipulator>(), null, manageAccountRequest);

            upsAccount = new UpsAccountEntity();

            testObject = new UpsLinkNewAccountInfoManipulator(upsAccount);
        }

        [Fact]
        public void Manipulate_UsernameIsUserID()
        {
            var testID = "testID";
            upsAccount.UserID = testID;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(testID, manageAccountRequest.Username);
        }

        [Fact]
        public void Manipulate_PasswordIsPassword()
        {
            var expected = "asdfg";
            upsAccount.Password = expected;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(expected, manageAccountRequest.Password);
        }

        [Fact]
        public void Manipulate_AccountNameIsAccountNumber()
        {
            var expected = "1234";
            upsAccount.AccountNumber = expected;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(expected, manageAccountRequest.ShipperAccount.AccountName);
        }

        [Fact]
        public void Manipulate_AccountNumberIsAccountNumber()
        {
            var expected = "1234";
            upsAccount.AccountNumber = expected;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(expected, manageAccountRequest.ShipperAccount.AccountNumber);
        }

        [Fact]
        public void Manipulate_CountryCodeIsCountryCode()
        {
            var expected = "AS";
            upsAccount.CountryCode = expected;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(expected, manageAccountRequest.ShipperAccount.CountryCode);
        }

        [Fact]
        public void Manipulate_PostalCodeIsPostalCode()
        {
            var expected = "90210";
            upsAccount.PostalCode = expected;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(expected, manageAccountRequest.ShipperAccount.PostalCode);
        }
        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
