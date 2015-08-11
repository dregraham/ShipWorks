using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Response
{
    public class FedExShipResponseTest
    {
        private FedExShipResponse testObject;
        
        private List<ICarrierResponseManipulator> manipulators;
        private Mock<ILabelRepository> mockLabelRepository;
        private Mock<ICarrierResponseManipulator> mockedShipmentManipulator;
        private Mock<CarrierRequest> carrierRequest;

        public FedExShipResponseTest()
        {
            mockedShipmentManipulator = new Mock<ICarrierResponseManipulator>();
            manipulators = new List<ICarrierResponseManipulator>
            {
                mockedShipmentManipulator.Object
            };

            mockLabelRepository = new Mock<ILabelRepository>();

            carrierRequest = new Mock<CarrierRequest>(null, null);

            ProcessShipmentReply processShipmentReply = BuildFedExProcessShipmentReply.BuildValidFedExProcessShipmentReply();
            ShipmentEntity setupShipmentEntity = BuildFedExShipmentEntity.SetupBaseShipmentEntity();

            testObject = new FedExShipResponse(processShipmentReply, carrierRequest.Object, setupShipmentEntity, mockLabelRepository.Object, manipulators);
        }

        [Fact]
        public void Process_ThrowsFedExApiException_WhenHighestSeverityIsFailure()
        {
            var processShipmentReply = new ProcessShipmentReply
            {
                HighestSeverity = NotificationSeverityType.FAILURE,
                Notifications = new[]
                {
                    new Notification
                    {
                        Message = "TestFailure"
                    }
                }
            };

            testObject = new FedExShipResponse(processShipmentReply, carrierRequest.Object, null, mockLabelRepository.Object, manipulators);

            try
            {
                testObject.Process();
            }
            catch (FedExApiCarrierException ex)
            {
                Assert.True(ex.Message.Contains("TestFailure"));
            }
        }

        [Fact]
        public void Process_ManipulatorsApplied_WhenProcessShipmentReplyContainsNoErrors()
        {
            testObject.Process();

           mockedShipmentManipulator.Verify(x => x.Manipulate(It.IsAny<ICarrierResponse>()),Times.Once()); 
        }

        [Fact]
        public void Process_SaveLabelsCalled_WhenProcessShipmentReplyContainsNoErrors()
        {
            testObject.Process();

            mockLabelRepository.Verify(x => x.SaveLabels(testObject), Times.Once());
        }

    }
}
