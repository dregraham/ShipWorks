﻿using System;
using System.Collections.Generic;
using CultureAttribute;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.InvoiceRegistration.Api.Request.Manipulators;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.WebServices.Registration;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.InvoiceRegistration.Api.Request.Manipulators
{
	[UseCulture("en-US")]
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

			registerRequest = new RegisterRequest();

			mockRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), null, registerRequest);
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
		public void Manipulate_CurrencyCodeIsSetToUsd()
		{
			testObject.Manipulate(request);

			Assert.Equal("USD", registerRequest.ShipperAccount.InvoiceInfo.CurrencyCode);
		}

		[Fact]
		public void Manipulate_AccountCountryIsCanada_CurrencyCodeIsSetToCad()
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
		public void Manipulate_InvoiceNumberIsSet()
		{
			testObject.Manipulate(request);

			Assert.Equal(invoiceAuthorization.InvoiceNumber, registerRequest.ShipperAccount.InvoiceInfo.InvoiceNumber);
		}

		[Fact]
		public void Manipulate_InvoiceDateIsSet()
		{
			testObject.Manipulate(request);

			Assert.Equal("20090927", registerRequest.ShipperAccount.InvoiceInfo.InvoiceDate);
		}

		[Fact]
		public void Manipulate_InvoiceAmount()
		{
			testObject.Manipulate(request);

			Assert.Equal("42.42", registerRequest.ShipperAccount.InvoiceInfo.InvoiceAmount);
		}

		[Fact]
		public void Manipulate_ControlIdIsSet()
		{
			testObject.Manipulate(request);

			Assert.Equal(invoiceAuthorization.ControlID, registerRequest.ShipperAccount.InvoiceInfo.ControlID);
		}
	}
}
