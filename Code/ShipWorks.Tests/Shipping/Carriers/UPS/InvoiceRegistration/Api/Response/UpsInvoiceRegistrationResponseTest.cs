﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using Xunit;
using Moq;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.InvoiceRegistration.Api.Response;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.WebServices.Registration;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.InvoiceRegistration.Api.Response
{
    public class UpsInvoiceRegistrationResponseTest
    {
        private UpsInvoiceRegistrationResponse testObject;

        private Mock<CarrierRequest> carrierRequest;

        private RegisterResponse nativeResponse;

        private List<Mock<ICarrierResponseManipulator>> manipulators;

        public UpsInvoiceRegistrationResponseTest()
        {
            carrierRequest = new Mock<CarrierRequest>();

            nativeResponse = new RegisterResponse()
            {
                Response = new ResponseType()
                {
                    ResponseStatus = new CodeDescriptionType()
                    {
                        Code = EnumHelper.GetApiValue(UpsInvoiceRegistrationResponseStatusCode.Success)
                    }
                }
            };

            var manipulator = new Mock<ICarrierResponseManipulator>();
            manipulator.Setup(m => m.Manipulate(It.IsAny<UpsInvoiceRegistrationResponse>()));

            manipulators = new List<Mock<ICarrierResponseManipulator>> { manipulator };

            testObject = new UpsInvoiceRegistrationResponse(
                nativeResponse,
                carrierRequest.Object, manipulators.Select(x => x.Object).ToList());
        }

        [Fact]
        public void Process_DelegatesToManipulators_WhenThereIsOneManipulator()
        {
            testObject.Process();

            foreach (Mock<ICarrierResponseManipulator> manipulator in manipulators)
            {
                manipulator.Verify(m => m.Manipulate(testObject), Times.Once());
            }
        }

        [Fact]
        public void Process_UpsApiException_WhenResponseStatusIsFailure()
        {
            nativeResponse.Response.ResponseStatus.Code = EnumHelper.GetApiValue(UpsInvoiceRegistrationResponseStatusCode.Failed);

            Assert.Throws<UpsApiException>(() => testObject.Process());
        }
    }
}
