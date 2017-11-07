using System.Collections.Generic;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response;
using ShipWorks.Shipping.Carriers.FedEx.Api.Ship;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Response
{
    public class FedExShipResponseTest
    {
        private FedExShipResponse testObject;

        private List<IFedExShipResponseManipulator> manipulators;
        private Mock<IFedExLabelRepository> mockLabelRepository;
        private readonly AutoMock mock;
        private Mock<IFedExShipResponseManipulator> mockedShipmentManipulator;
        private Mock<CarrierRequest> carrierRequest;

        public FedExShipResponseTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            mockedShipmentManipulator = new Mock<IFedExShipResponseManipulator>();
            manipulators = new List<IFedExShipResponseManipulator>
            {
                mockedShipmentManipulator.Object
            };

            mockLabelRepository = new Mock<IFedExLabelRepository>();

            carrierRequest = new Mock<CarrierRequest>(null, null);

            //ProcessShipmentReply processShipmentReply = BuildFedExProcessShipmentReply.BuildValidFedExProcessShipmentReply();
            //ShipmentEntity setupShipmentEntity = BuildFedExShipmentEntity.SetupBaseShipmentEntity();

            testObject = mock.Create<FedExShipResponse>();
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

            //testObject =  new FedExShipResponse(processShipmentReply, carrierRequest.Object, null, mockLabelRepository.Object, manipulators);

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
        public void ApplyResponseManipulators_AppliesResponseManipulators_WhenProcessShipmentReplyContainsNoErrors()
        {
            testObject.ApplyManipulators();

            mockedShipmentManipulator.Verify(x => x.Manipulate(It.IsAny<ProcessShipmentReply>(), AnyShipment), Times.Once());
        }

        [Fact]
        public void Process_SaveLabelsCalled_WhenProcessShipmentReplyContainsNoErrors()
        {
            testObject.Process();

            mockLabelRepository.Verify(x => x.SaveLabels(AnyShipment, It.IsAny<ProcessShipmentReply>()), Times.Once());
        }
    }
}
