using System.Collections.Generic;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.OrderLookup.Controls.ShipmentDetails;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Services.Builders;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.OrderLookup.Tests.Controls.ShipmentDetails
{
    public class CarrierShipmentAdapterOptionsProviderTest
    {
        private readonly AutoMock mock;
        private readonly CarrierShipmentAdapterOptionsProvider testObject;
        private readonly Mock<ICarrierShipmentAdapter> shipmentAdapter;
        private readonly Mock<IShipmentTypeManager> shipmentTypeManager;
        private readonly Mock<IDimensionsManager> dimensionsManager;
        private readonly ShipmentTypeProvider ShipmentTypeProvider;

        public CarrierShipmentAdapterOptionsProviderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            shipmentTypeManager = mock.Mock<IShipmentTypeManager>();
            shipmentTypeManager.SetupGet(s => s.EnabledShipmentTypeCodes).Returns(new[] { ShipmentTypeCode.Usps });

            ShipmentTypeProvider = new ShipmentTypeProvider(new TestMessenger(), shipmentTypeManager.Object);

            dimensionsManager = mock.Mock<IDimensionsManager>();

            testObject = mock.Create<CarrierShipmentAdapterOptionsProvider>();
            shipmentAdapter = mock.Mock<ICarrierShipmentAdapter>();
        }

        [Fact]
        public void GetPackageTypes_DelegatesToShipmentPackageTypesBuilderFactoryForShipmentPackageTypesBuilder()
        {
            shipmentAdapter.SetupGet(a => a.ShipmentTypeCode).Returns(ShipmentTypeCode.UpsOnLineTools);

            testObject.GetPackageTypes(shipmentAdapter.Object);

            shipmentAdapter.VerifyGet(a => a.ShipmentTypeCode);
            mock.Mock<IShipmentPackageTypesBuilderFactory>().Verify(f => f.Get(ShipmentTypeCode.UpsOnLineTools));
        }

        [Fact]
        public void GetPackageTypes_DelegatesToShipmentPackageTypesBuilderForPackageTypes()
        {
            ShipmentEntity shipment = new ShipmentEntity();
            shipmentAdapter.SetupGet(a => a.Shipment).Returns(shipment);

            Mock<IShipmentPackageTypesBuilder> shipmentPackageTypesBuilder = mock.Mock<IShipmentPackageTypesBuilder>();
            mock.Mock<IShipmentPackageTypesBuilderFactory>().Setup(f => f.Get(It.IsAny<ShipmentTypeCode>())).Returns(shipmentPackageTypesBuilder);

            testObject.GetPackageTypes(shipmentAdapter.Object);

            shipmentAdapter.VerifyGet(a => a.Shipment);
            shipmentPackageTypesBuilder.Verify(b => b.BuildPackageTypeDictionary(new[] { shipment }));
        }

        [Fact]
        public void GetProviders_DelegatesToShipmentTypeProvider()
        {
            testObject.GetProviders(shipmentAdapter.Object, ShipmentTypeCode.UpsOnLineTools);

            shipmentTypeManager.VerifyGet(s => s.EnabledShipmentTypeCodes);
        }

        [Fact]
        public void GetProviders_UnionsAdaptersShipmentTypeCode()
        {
            shipmentAdapter.SetupGet(s => s.ShipmentTypeCode).Returns(ShipmentTypeCode.iParcel);

            Dictionary<ShipmentTypeCode, string> result = testObject.GetProviders(shipmentAdapter.Object, ShipmentTypeCode.UpsOnLineTools);

            Assert.True(result.ContainsKey(ShipmentTypeCode.iParcel));
        }

        [Fact]
        public void GetProviders_UnionsIncludeShipmentTypeCode()
        {
            Dictionary<ShipmentTypeCode, string> result = testObject.GetProviders(shipmentAdapter.Object, ShipmentTypeCode.OnTrac);

            Assert.True(result.ContainsKey(ShipmentTypeCode.OnTrac));
        }

        [Fact]
        public void GetDimensionsProfiles_DelegatesToDimensionsManager()
        {
            IPackageAdapter package = mock.Mock<IPackageAdapter>().Object;

            testObject.GetDimensionsProfiles(package);

            dimensionsManager.Verify(d => d.ProfilesReadOnly(package));
        }
    }
}
