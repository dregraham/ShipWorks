using System;
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
    public class UpsInvoiceRegistrationInvoiceInfoManipulatorTest
    {
        UpsInvoiceRegistrationInvoiceInfoManipulator testObject;

        private UpsOltInvoiceAuthorizationData invoiceAuthorization;

        private CarrierRequest request;

        private RegisterRequest registerRequest;

        private Mock<CarrierRequest> mockRequest;

        public UpsInvoiceRegistrationInvoiceInfoManipulatorTest()
        {
            invoiceAuthorization = new UpsOltInvoiceAuthorizationData()
            {
                ControlID = "control",
                InvoiceAmount = 42.42m,
                InvoiceDate = new DateTime(2009, 9, 27),
                InvoiceNumber = "invoiceNumber"
            };

            registerRequest=new RegisterRequest();

            mockRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(),null,registerRequest);
            mockRequest
                .Setup(r => r.CarrierAccountEntity)
                .Returns(new UpsAccountEntity()
                {
                    CountryCode = "US"
                });

            request = mockRequest.Object;

            testObject = new UpsInvoiceRegistrationInvoiceInfoManipulator(invoiceAuthorization);           
        }

        [Fact]
        public void Manipulate_CurrencyCodeIsSetToUsd_Test()
        {
            testObject.Manipulate(request);

            Assert.Equal("USD", registerRequest.ShipperAccount.InvoiceInfo.CurrencyCode);
        }

        [Fact]
        public void Manipulate_AccountCountryIsCanada_CurrencyCodeIsSetToCad_Test()
        {
            mockRequest
                .Setup(r => r.CarrierAccountEntity)
                .Returns(new UpsAccountEntity()
                {
                    CountryCode = "CA"
                });

            testObject.Manipulate(request);

            Assert.Equal("CAD", registerRequest.ShipperAccount.InvoiceInfo.CurrencyCode);
        }

        [Fact]
        public void Manipulate_InvoiceNumberIsSet_Test()
        {
            testObject.Manipulate(request);

            Assert.Equal(invoiceAuthorization.InvoiceNumber, registerRequest.ShipperAccount.InvoiceInfo.InvoiceNumber);
        }

        [Fact]
        public void Manipulate_InvoiceDateIsSet_Test()
        {
            testObject.Manipulate(request);

            Assert.Equal("20090927", registerRequest.ShipperAccount.InvoiceInfo.InvoiceDate);
        }

        [Fact]
        public void Manipulate_InvoiceAmount_Test()
        {
            testObject.Manipulate(request);

            Assert.Equal("42.42", registerRequest.ShipperAccount.InvoiceInfo.InvoiceAmount);
        }

        [Fact]
        public void Manipulate_ControlIdIsSet_Test()
        {
            testObject.Manipulate(request);

            Assert.Equal(invoiceAuthorization.ControlID, registerRequest.ShipperAccount.InvoiceInfo.ControlID);
        }
    }
}
