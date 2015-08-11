using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.InvoiceRegistration.Api.Request.Manipulators;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.WebServices.Registration;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.InvoiceRegistration.Api.Request.Manipulators
{
    public class UpsInvoiceRegistrationAddressManipulatorTest
    {
        private RegisterRequest registerRequest;

        private CarrierRequest request;

        private UpsInvoiceRegistrationAddressManipulator testObject;

        private UpsAccountEntity upsAccount;

        [TestInitialize]
        public void Initialize()
        {
            registerRequest = new RegisterRequest();

            Mock<CarrierRequest> mockRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), null, registerRequest);

            request = mockRequest.Object;

            upsAccount = new UpsAccountEntity
            {
                Company = "MyCompany",
                FirstName = "Homer",
                MiddleName = "J",
                LastName = "Simpson",
                Street1 = "123 Elm St",
                Street2 = "Apt 42",
                City = "Springfield",
                StateProvCode = "IL",
                PostalCode = "60035",
                CountryCode = "US",
                Phone = "847-433-5532",
                Email = "HSimpson@aol.com"
            };

            testObject = new UpsInvoiceRegistrationAddressManipulator(upsAccount);
        }

        [Fact]
        public void Manipulate_CustomerNameIsSet_Test()
        {
            testObject.Manipulate(request);

            Assert.AreEqual(string.Format("{0} {1} {2}", upsAccount.FirstName, upsAccount.MiddleName, upsAccount.LastName), registerRequest.CustomerName);
        }

        [Fact]
        public void Manipulate_CompanyNameIsSet_Test()
        {
            testObject.Manipulate(request);

            Assert.AreEqual(upsAccount.Company, registerRequest.CompanyName);
        }

        [Fact]
        public void Manipulate_AddressLine0IsSet_Test()
        {
            testObject.Manipulate(request);

            Assert.AreEqual(upsAccount.Street1, registerRequest.Address.AddressLine[0]);
        }

        [Fact]
        public void Manipulate_AddressLine1IsSet_Test()
        {
            testObject.Manipulate(request);

            Assert.AreEqual(upsAccount.Street2, registerRequest.Address.AddressLine[1]);
        }

        [Fact]
        public void Manipulate_CityIsSet_Test()
        {
            testObject.Manipulate(request);

            Assert.AreEqual(upsAccount.City, registerRequest.Address.City);
        }

        [Fact]
        public void Manipulate_StateIsSet_Test()
        {
            testObject.Manipulate(request);

            Assert.AreEqual(upsAccount.StateProvCode, registerRequest.Address.StateProvinceCode);
        }

        [Fact]
        public void Manipulate_ZipIsSet_Test()
        {
            testObject.Manipulate(request);

            Assert.AreEqual(upsAccount.PostalCode, registerRequest.Address.PostalCode);
        }

        [Fact]
        public void Manipulate_PostalCodeIsSet_Test()
        {
            testObject.Manipulate(request);

            Assert.AreEqual(upsAccount.PostalCode, registerRequest.Address.PostalCode);
        }

        [Fact]
        public void Manipulate_CountryCodeIsSet_Test()
        {
            testObject.Manipulate(request);

            Assert.AreEqual(upsAccount.CountryCode, registerRequest.Address.CountryCode);
        }

        [Fact]
        public void Manipulate_PhoneIsSet_Test()
        {
            testObject.Manipulate(request);

            Assert.AreEqual(upsAccount.Phone.Replace("-",string.Empty), registerRequest.PhoneNumber);
        }

        [Fact]
        public void Manipulate_EmailIsSet_Test()
        {
            testObject.Manipulate(request);

            Assert.AreEqual(upsAccount.Email, registerRequest.EmailAddress);
        }
    }
}