using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Carriers.FedEx;
using ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    public class FedExServiceTypeManipulatorTest
    {
        private readonly ProcessShipmentRequest processShipmentRequest;
        private readonly ShipmentEntity shipment;
        private readonly FedExServiceTypeManipulator testObject;

        public const string DeprecatedCode = "FedEx is no longer going directly to FedEx servers.  It goes through ShipEngine now, so this is deprecated code.";

        public FedExServiceTypeManipulatorTest()
        {
            AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            shipment = BuildFedExShipmentEntity.SetupRequestShipmentEntity();
            shipment.FedEx.Service = (int) FedExServiceType.PriorityOvernight;

            processShipmentRequest = new ProcessShipmentRequest();

            testObject = mock.Create<FedExServiceTypeManipulator>();
        }

        [Fact(Skip = DeprecatedCode)]
        public void ShouldApply_ReturnsTrue()
        {
            // Make sure we got a the same values back
            Assert.True(testObject.ShouldApply(shipment, 0));
        }

        [Fact(Skip = DeprecatedCode)]
        public void Manipulate_FedExServiceTypeManipulator_ReturnsServiceType()
        {
            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Make sure we got a the same values back
            Assert.Equal(processShipmentRequest.RequestedShipment.ServiceType, ServiceType.PRIORITY_OVERNIGHT);
        }
    }
}
