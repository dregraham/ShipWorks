﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.InvoiceRegistration.Api.Request.Manipulators;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.WebServices.Registration;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.InvoiceRegistration.Api.Request.Manipulators
{
    public class UpsInvoiceRegistrationShipperInfoManipulatorTest
    {
        UpsInvoiceRegistrationShipperInfoManipulator testObject;

        private UpsAccountEntity upsAccount;

        private CarrierRequest request;

        private RegisterRequest registerRequest;

        public UpsInvoiceRegistrationShipperInfoManipulatorTest()
        {
            upsAccount = new UpsAccountEntity()
            {
                AccountNumber = "42",
                PostalCode = "90210",
                CountryCode = "USA"
            };

            registerRequest = new RegisterRequest();

            Mock<CarrierRequest> mockRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(),null,registerRequest);

            request = mockRequest.Object;

            testObject = new UpsInvoiceRegistrationShipperInfoManipulator(upsAccount);           
        }

        [Fact]
        public void Manipulate_AccountNameIsInterapptive()
        {
            testObject.Manipulate(request);

            Assert.Equal("Interapptive", registerRequest.ShipperAccount.AccountName);
        }

        [Fact]
        public void Manipulate_AccountNumberIsSet()
        {
            testObject.Manipulate(request);

            Assert.Equal(upsAccount.AccountNumber, registerRequest.ShipperAccount.AccountNumber);
        }

        [Fact]
        public void Manipulate_PostalCodeIsSet()
        {
            testObject.Manipulate(request);

            Assert.Equal(upsAccount.PostalCode, registerRequest.ShipperAccount.PostalCode);
        }

        [Fact]
        public void Manipulate_CountryCodeIsSet()
        {
            testObject.Manipulate(request);

            Assert.Equal(upsAccount.CountryCode, registerRequest.ShipperAccount.CountryCode);
        }
    }
}
