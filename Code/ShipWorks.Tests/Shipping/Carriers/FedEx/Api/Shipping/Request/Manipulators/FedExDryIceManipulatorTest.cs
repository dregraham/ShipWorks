using System.Linq;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    public class FedExDryIceManipulatorTest
    {
        private FedExDryIceManipulator testObject;

        private ShipmentEntity shipmentEntity;

        private FedExShipRequest shipRequest;
        private Mock<ICarrierSettingsRepository> settingsRepository;

        public FedExDryIceManipulatorTest()
        {
            settingsRepository = new Mock<ICarrierSettingsRepository>();

            shipmentEntity = BuildFedExShipmentEntity.SetupRequestShipmentEntity();

            // All dry ice shipments need to use custom packaging
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.Custom;

            shipRequest = new FedExShipRequest(
                null,
                shipmentEntity,
                null,
                null,
                settingsRepository.Object,
                new ProcessShipmentRequest());

            testObject = new FedExDryIceManipulator();

            shipmentEntity.FedEx.Packages[0].DryIceWeight = 2.2046; //1KG
            shipmentEntity.FedEx.Packages[1].DryIceWeight = 4.4092; //2KG

            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;
        }

        [Fact]
        public void Manipulate_DryIceInPackageSpecialServiceType_WhenNotUsingGroundService_ShipmentEntityHasThreeKgOfDryIce()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.PriorityOvernight;

            testObject.Manipulate(shipRequest);

            Assert.Equal(1, ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes.Count(t => t == PackageSpecialServiceType.DRY_ICE));
        }

        [Fact]
        public void Manipulate_HasOnePackageSpecialServiceType_WhenNotUsingGroundService_ShipmentEntityHasThreeKgOfDryIce()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.PriorityOvernight;

            testObject.Manipulate(shipRequest);

            Assert.Equal(1, ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes.Count());
        }

        [Fact]
        public void Manipulate_WeightOfPackageIsOne_WhenNotUsingGroundService_ShipmentEntityHasThreeKgOfDryIce()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.PriorityOvernight;

            testObject.Manipulate(shipRequest);

            Assert.Equal(1, ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DryIceWeight.Value);
        }

        [Fact]
        public void Manipulate_WeightOfPackageIsInKg_FirstPackageHasDryIceWeight_WhenNotUsingGroundService()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.PriorityOvernight;

            testObject.Manipulate(shipRequest);

            Assert.Equal(WeightUnits.KG, ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DryIceWeight.Units);
        }

        [Fact]
        public void Manipulate_ThrowsFedExException_WhenUsingFedExBoxPackagingType()
        {
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.Box;
            Assert.Throws<FedExException>(() => testObject.Manipulate(shipRequest));
        }

        [Fact]
        public void Manipulate_ThrowsFedExException_WhenUsingEnvelopePackagingType()
        {
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.Envelope;
            Assert.Throws<FedExException>(() => testObject.Manipulate(shipRequest));
        }

        [Fact]
        public void Manipulate_ThrowsFedExException_WhenUsingPakPackagingType()
        {
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.Pak;
            Assert.Throws<FedExException>(() => testObject.Manipulate(shipRequest));
        }

        [Fact]
        public void Manipulate_ThrowsFedExException_WhenUsingTubePackagingType()
        {
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.Tube;
            Assert.Throws<FedExException>(() => testObject.Manipulate(shipRequest));
        }

        [Fact]
        public void Manipulate_ThrowsFedExException_WhenUsing10KgBoxPackagingType()
        {
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.Box10Kg;
            Assert.Throws<FedExException>(() => testObject.Manipulate(shipRequest));
        }

        [Fact]
        public void Manipulate_ThrowsFedExException_WhenUsing25KgBoxPackagingType()
        {
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.Box25Kg;
            Assert.Throws<FedExException>(() => testObject.Manipulate(shipRequest));
        }

        [Fact]
        public void Manipulate_DryIceNotAdded_WhenDryIceAmountIs0AndCustomPackagingType()
        {
            shipmentEntity.FedEx.Packages[0].DryIceWeight = 0;
            shipmentEntity.FedEx.Packages[1].DryIceWeight = 0;

            testObject.Manipulate(shipRequest);

            Assert.Equal(0, ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes.Count(t => t == PackageSpecialServiceType.DRY_ICE));
        }

        [Fact]
        public void Manipulate_DryIceNotAdded_WhenDryIceAmountIs0AndUsing25KgBoxPackageType()
        {
            shipmentEntity.FedEx.Packages[0].DryIceWeight = 0;
            shipmentEntity.FedEx.Packages[1].DryIceWeight = 0;

            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.Box25Kg;

            testObject.Manipulate(shipRequest);

            Assert.Equal(0, ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes.Count(t => t == PackageSpecialServiceType.DRY_ICE));
        }

        /// <summary>
        /// NativeRequest from shipRequest converted to ProcessShipmentRequest
        /// </summary>
        private ProcessShipmentRequest ProcessShipmentRequest
        {
            get
            {
                return shipRequest.NativeRequest as ProcessShipmentRequest;
            }
        }


    }
}
