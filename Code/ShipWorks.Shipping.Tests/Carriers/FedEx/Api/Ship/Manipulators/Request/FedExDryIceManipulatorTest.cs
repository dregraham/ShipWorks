using System.Linq;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Carriers.FedEx;
using ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    public class FedExDryIceManipulatorTest
    {
        private FedExDryIceManipulator testObject;
        private ShipmentEntity shipment;
        private readonly AutoMock mock;

        public FedExDryIceManipulatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            shipment = BuildFedExShipmentEntity.SetupRequestShipmentEntity();

            // All dry ice shipments need to use custom packaging
            shipment.FedEx.PackagingType = (int) FedExPackagingType.Custom;
            shipment.FedEx.Packages[0].DryIceWeight = 2.2046; //1KG
            shipment.FedEx.Packages[1].DryIceWeight = 4.4092; //2KG
            shipment.FedEx.Service = (int) FedExServiceType.FedExGround;

            testObject = new FedExDryIceManipulator();
        }

        [Fact]
        public void Manipulate_DryIceInPackageSpecialServiceType_WhenNotUsingGroundService_ShipmentEntityHasThreeKgOfDryIce()
        {
            shipment.FedEx.Service = (int) FedExServiceType.PriorityOvernight;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Equal(1, result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes.Count(t => t == PackageSpecialServiceType.DRY_ICE));
        }

        [Fact]
        public void Manipulate_HasOnePackageSpecialServiceType_WhenNotUsingGroundService_ShipmentEntityHasThreeKgOfDryIce()
        {
            shipment.FedEx.Service = (int) FedExServiceType.PriorityOvernight;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Equal(1, result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes.Count());
        }

        [Fact]
        public void Manipulate_WeightOfPackageIsOne_WhenNotUsingGroundService_ShipmentEntityHasThreeKgOfDryIce()
        {
            shipment.FedEx.Service = (int) FedExServiceType.PriorityOvernight;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Equal(1, result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DryIceWeight.Value);
        }

        [Fact]
        public void Manipulate_WeightOfPackageIsInKg_FirstPackageHasDryIceWeight_WhenNotUsingGroundService()
        {
            shipment.FedEx.Service = (int) FedExServiceType.PriorityOvernight;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Equal(WeightUnits.KG, result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DryIceWeight.Units);
        }

        [Fact]
        public void Manipulate_DryIceInShipmentSpecialServiceType_WhenUsingLTLFreightService_ShipmentEntityHasThreeKgOfDryIce()
        {
            shipment.FedEx.Service = (int) FedExServiceType.FedExFreightPriority;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Equal(1, result.Value.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Count(t => t == ShipmentSpecialServiceType.DRY_ICE));
        }

        [Fact]
        public void Manipulate_HasOneShipmentSpecialServiceType_WhenUsingLTLFreightService_ShipmentEntityHasThreeKgOfDryIce()
        {
            shipment.FedEx.Service = (int) FedExServiceType.FedExFreightPriority;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Equal(1, result.Value.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Count());
        }

        [Fact]
        public void Manipulate_WeightOfShipmentIsOne_WhenUsingLTLFreightService_ShipmentEntityHasThreeKgOfDryIce()
        {
            shipment.FedEx.Service = (int) FedExServiceType.FedExFreightPriority;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Equal(1, result.Value.RequestedShipment.SpecialServicesRequested.ShipmentDryIceDetail.TotalWeight.Value);
        }

        [Fact]
        public void Manipulate_WeightOfShipmentIsInKg_FirstPackageHasDryIceWeight_WhenUsingLTLFreightService()
        {
            shipment.FedEx.Service = (int) FedExServiceType.FedExFreightPriority;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Equal(WeightUnits.KG, result.Value.RequestedShipment.SpecialServicesRequested.ShipmentDryIceDetail.TotalWeight.Units);
        }

        [Theory]
        [InlineData(FedExPackagingType.Box)]
        [InlineData(FedExPackagingType.Envelope)]
        [InlineData(FedExPackagingType.Pak)]
        [InlineData(FedExPackagingType.Tube)]
        [InlineData(FedExPackagingType.Box10Kg)]
        [InlineData(FedExPackagingType.Box25Kg)]
        public void Manipulate_ReturnsFailure_WhenUsingPackagingOtherThanCustom(FedExPackagingType packaging)
        {
            shipment.FedEx.PackagingType = (int) packaging;
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);
            Assert.True(result.Failure);
            Assert.IsAssignableFrom<FedExException>(result.Exception);
        }

        [Fact]
        public void Manipulate_DryIceNotAdded_WhenDryIceAmountIs0AndCustomPackagingType()
        {
            shipment.FedEx.Packages[0].DryIceWeight = 0;
            shipment.FedEx.Packages[1].DryIceWeight = 0;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Null(result.Value.RequestedShipment);
        }

        [Fact]
        public void Manipulate_DryIceNotAddedToSecondPackage_WhenDryIceAmountIs0()
        {
            shipment.FedEx.Packages[1].DryIceWeight = 0;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 1);

            Assert.Null(result.Value.RequestedShipment);
        }

        [Fact]
        public void Manipulate_DryIceNotAdded_WhenDryIceAmountIs0()
        {
            shipment.FedEx.Packages[0].DryIceWeight = 0;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Null(result.Value.RequestedShipment);
        }
    }
}
