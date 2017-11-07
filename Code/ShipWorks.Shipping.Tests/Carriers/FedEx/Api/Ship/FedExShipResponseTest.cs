using System.Collections.Generic;
using Autofac;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.Api.Ship;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Response
{
    public class FedExShipResponseTest
    {
        private FedExShipResponse testObject;

        private List<IFedExShipResponseManipulator> manipulators;
        private readonly ProcessShipmentReply reply;
        private Mock<IFedExLabelRepository> mockLabelRepository;
        private readonly AutoMock mock;
        private Mock<IFedExShipResponseManipulator> manipulator;
        private Mock<CarrierRequest> carrierRequest;

        public FedExShipResponseTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            manipulator = mock.CreateMock<IFedExShipResponseManipulator>();
            manipulator.Setup(x => x.Manipulate(It.IsAny<ProcessShipmentReply>(), AnyShipment))
                .Returns(new ShipmentEntity());
            mock.Provide<IEnumerable<IFedExShipResponseManipulator>>(new[] { manipulator.Object });


            //var manipulatorFactory = mock.MockFunc<IFedExSettingsRepository, IFedExShipResponseManipulator>(manipulator);

            //mock.Provide<IEnumerable<Func<IFedExSettingsRepository, IFedExShipResponseManipulator>>>(
            //    new List<Func<IFedExSettingsRepository, IFedExShipResponseManipulator>>
            //    {
            //        manipulatorFactory.Object,
            //    });

            reply = new ProcessShipmentReply { HighestSeverity = NotificationSeverityType.SUCCESS };
            reply.Ensure(x => x.CompletedShipmentDetail).EnsureAtLeastOne(x => x.CompletedPackageDetails);

            testObject = mock.Create<FedExShipResponse>(
                TypedParameter.From(reply),
                TypedParameter.From(Create.Shipment().Build()));
        }

        [Fact]
        public void ApplyManipulators_ReturnsError_WhenHighestSeverityIsFailure()
        {
            reply.HighestSeverity = NotificationSeverityType.FAILURE;
            reply.Notifications = new[] { new Notification { Message = "TestFailure" } };

            var result = testObject.ApplyManipulators();

            Assert.True(result.Failure);
            Assert.IsAssignableFrom<FedExApiCarrierException>(result.Exception);
        }

        [Fact]
        public void ApplyManipulators_AppliesResponseManipulators_WhenProcessShipmentReplyContainsNoErrors()
        {
            testObject.ApplyManipulators();

            manipulator.Verify(x => x.Manipulate(It.IsAny<ProcessShipmentReply>(), AnyShipment), Times.Once());
        }

        [Fact]
        public void Process_SaveLabelsCalled_WhenProcessShipmentReplyContainsNoErrors()
        {
            testObject.Process();

            mock.Mock<IFedExLabelRepository>()
                .Verify(x => x.SaveLabels(AnyShipment, It.IsAny<ProcessShipmentReply>()), Times.Once());
        }
    }
}
