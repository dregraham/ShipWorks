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

        public UpsInvoiceRegistrationAddressManipulatorTest()
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
        public void Manipulate_CustomerNameIsSet()
        {
            testObject.Manipulate(request);

            Assert.Equal(string.Format("{0} {1} {2}", upsAccount.FirstName, upsAccount.MiddleName, upsAccount.LastName), registerRequest.CustomerName);
        }

        [Fact]
        public void Manipulate_CompanyNameIsSet()
        {
            testObject.Manipulate(request);

            Assert.Equal(upsAccount.Company, registerRequest.CompanyName);
        }

        [Fact]
        public void Manipulate_AddressLine0IsSet()
        {
            testObject.Manipulate(request);

            Assert.Equal(upsAccount.Street1, registerRequest.Address.AddressLine[0]);
        }

        [Fact]
        public void Manipulate_AddressLine1IsSet()
        {
            testObject.Manipulate(request);

            Assert.Equal(upsAccount.Street2, registerRequest.Address.AddressLine[1]);
        }

        [Fact]
        public void Manipulate_CityIsSet()
        {
            testObject.Manipulate(request);

            Assert.Equal(upsAccount.City, registerRequest.Address.City);
        }

        [Fact]
        public void Manipulate_StateIsSet()
        {
            testObject.Manipulate(request);

            Assert.Equal(upsAccount.StateProvCode, registerRequest.Address.StateProvinceCode);
        }

        [Fact]
        public void Manipulate_ZipIsSet()
        {
            testObject.Manipulate(request);

            Assert.Equal(upsAccount.PostalCode, registerRequest.Address.PostalCode);
        }

        [Fact]
        public void Manipulate_PostalCodeIsSet()
        {
            testObject.Manipulate(request);

            Assert.Equal(upsAccount.PostalCode, registerRequest.Address.PostalCode);
        }

        [Fact]
        public void Manipulate_CountryCodeIsSet()
        {
            testObject.Manipulate(request);

            Assert.Equal(upsAccount.CountryCode, registerRequest.Address.CountryCode);
        }

        [Fact]
        public void Manipulate_PhoneIsSet()
        {
            testObject.Manipulate(request);

            Assert.Equal(upsAccount.Phone.Replace("-",string.Empty), registerRequest.PhoneNumber);
        }

        [Fact]
        public void Manipulate_EmailIsSet()
        {
            testObject.Manipulate(request);

            Assert.Equal(upsAccount.Email, registerRequest.EmailAddress);
        }
    }
}