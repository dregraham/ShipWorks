using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OpenAccount;
using ShipWorks.Shipping.Carriers.UPS.OpenAccount.Api;
using ShipWorks.Shipping.Carriers.UPS.OpenAccount.Api.Request;
using ShipWorks.Shipping.Carriers.UPS.OpenAccount.Api.Response;
using ShipWorks.Shipping.Carriers.UPS.WebServices.OpenAccount;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.OpenAccount.Api.Response
{
    public class UpsOpenAccountResponseTest
    {
        private UpsOpenAccountResponse testObject;

        private OpenAccountResponse nativeResponse;
        private Mock<CarrierRequest> carrierRequest;

        private List<Mock<ICarrierResponseManipulator>> manipulators;

        private UpsAccountEntity account;

        public UpsOpenAccountResponseTest()
        {
            nativeResponse = BuildOpenAccountResponse(true, "1323223");

            account = new UpsAccountEntity()
            {
                AccountNumber = "1323223"
            };

            Mock<ICarrierResponseManipulator> manipulator = new Mock<ICarrierResponseManipulator>();
            manipulator.Setup(m => m.Manipulate(It.IsAny<UpsOpenAccountResponse>()));

            manipulators = new List<Mock<ICarrierResponseManipulator>> { manipulator };

            carrierRequest = new Mock<CarrierRequest>(null, null);
            carrierRequest.Setup(r => r.CarrierAccountEntity).Returns(account);

            testObject = new UpsOpenAccountResponse(nativeResponse, carrierRequest.Object, manipulators.Select(m => m.Object));
        }

        private OpenAccountResponse BuildOpenAccountResponse(bool success, string shipperNumber)
        {
            return new OpenAccountResponse()
            {
                ShipperNumber = shipperNumber,
                Response = new ResponseType()
                {
                    ResponseStatus = new CodeDescriptionType()
                    {
                        Code = success ? "1" : "0"
                    },
                    Alert = new CodeDescriptionType[]
                        {
                            new CodeDescriptionType()
                                {
                                    Code = success ? "1" : "0"
                                }
                        }
                }
            };
        }

        [Fact]
        public void Process_UpsOpenAccountResponseException_WhenResponseStatusIsFailure()
        {
            nativeResponse = BuildOpenAccountResponse(false, "1323223");
            testObject = new UpsOpenAccountResponse(nativeResponse, carrierRequest.Object, manipulators.Select(m => m.Object));

            Assert.Throws<UpsOpenAccountResponseException>(() => testObject.Process());
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
        public void Process_DelegatesToManipulators_WhenThereAreMultipleManipulators()
        {
            // Add a second manipulator to the list
            Mock<ICarrierResponseManipulator> anotherManipulator = new Mock<ICarrierResponseManipulator>();
            anotherManipulator.Setup(m => m.Manipulate(It.IsAny<UpsOpenAccountResponse>()));

            manipulators.Add(anotherManipulator);

            testObject = new UpsOpenAccountResponse(nativeResponse, carrierRequest.Object, manipulators.Select(m => m.Object));
            testObject.Process();

            // Check that the manipulators in the list were called and that the anotherManipulator was called
            manipulators.ForEach(manipulator => manipulator.Verify(m => m.Manipulate(testObject), Times.Once()));
            anotherManipulator.Verify(m => m.Manipulate(testObject), Times.Once());
        }
    }
}
