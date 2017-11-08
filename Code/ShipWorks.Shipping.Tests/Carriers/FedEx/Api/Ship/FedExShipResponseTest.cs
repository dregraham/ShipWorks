using System.Collections.Generic;
using Autofac;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.Api.Ship;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Ship
{
    public class FedExShipResponseTest
    {
        private readonly Mock<IFedExLabelRepository> labelRepository;
        private FedExShipResponse testObject;
        private readonly ProcessShipmentReply reply;
        private readonly AutoMock mock;
        private Mock<IFedExShipResponseManipulator> manipulator;

        public FedExShipResponseTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            manipulator = mock.CreateMock<IFedExShipResponseManipulator>();
            manipulator.Setup(x => x.Manipulate(It.IsAny<ProcessShipmentReply>(), It.IsAny<ProcessShipmentRequest>(), AnyShipment))
                .Returns(new ShipmentEntity());
            mock.Provide<IEnumerable<IFedExShipResponseManipulator>>(new[] { manipulator.Object });

            reply = new ProcessShipmentReply { HighestSeverity = NotificationSeverityType.SUCCESS };
            reply.Ensure(x => x.CompletedShipmentDetail).EnsureAtLeastOne(x => x.CompletedPackageDetails);

            ShipmentEntity shipment = Create.Shipment().AsFedEx().Build();
            shipment.FedEx.Service = (int) FedExServiceType.FedExGround;

            labelRepository = mock.FromFactory<IFedExLabelRepositoryFactory>()
                .Mock(x => x.Create(AnyShipment));

            testObject = mock.Create<FedExShipResponse>(
                TypedParameter.From(reply),
                TypedParameter.From(shipment));
        }

        [Fact]
        public void ApplyManipulators_ReturnsError_WhenHighestSeverityIsFailure()
        {
            reply.HighestSeverity = NotificationSeverityType.FAILURE;
            reply.Notifications = new[] { new Notification { Message = "TestFailure" } };

            var result = testObject.ApplyManipulators(null);

            Assert.True(result.Failure);
            Assert.IsAssignableFrom<FedExApiCarrierException>(result.Exception);
        }

        [Fact]
        public void ApplyManipulators_AppliesResponseManipulators_WhenProcessShipmentReplyContainsNoErrors()
        {
            testObject.ApplyManipulators(null);

            manipulator.Verify(x => x.Manipulate(It.IsAny<ProcessShipmentReply>(), null, AnyShipment), Times.Once());
        }

        [Fact]
        public void Process_SaveLabelsCalled_WhenProcessShipmentReplyContainsNoErrors()
        {
            testObject.Process();

            labelRepository.Verify(x => x.SaveLabels(AnyShipment, It.IsAny<ProcessShipmentReply>()), Times.Once());
        }

        [Fact]
        public void ApplyManipulators_ReturnsFailure_WhenLtlFreightServiceAndMissingShipmentDocs()
        {
            ShipmentEntity shipment = Create.Shipment().AsFedEx().Build();
            shipment.FedEx.Service = (int) FedExServiceType.FedExFreightEconomy;

            reply.CompletedShipmentDetail.ShipmentDocuments = new ShippingDocument[0];

            testObject = mock.Create<FedExShipResponse>(
                TypedParameter.From(reply),
                TypedParameter.From(shipment));

            var result = testObject.ApplyManipulators(null);

            Assert.True(result.Failure);
        }

        [Fact]
        public void ApplyManipulators_ReturnsFailure_WhenNotLtlFreightServiceAndMissingCompletedPackageDetails()
        {
            ShipmentEntity shipment = Create.Shipment().AsFedEx().Build();
            shipment.FedEx.Service = (int) FedExServiceType.FedExGround;

            reply.CompletedShipmentDetail.CompletedPackageDetails = null;

            testObject = mock.Create<FedExShipResponse>(
                TypedParameter.From(reply),
                TypedParameter.From(shipment));

            var result = testObject.ApplyManipulators(null);

            Assert.True(result.Failure);
        }
    }
}
