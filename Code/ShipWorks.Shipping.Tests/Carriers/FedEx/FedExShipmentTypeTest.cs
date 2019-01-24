using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Tests.Shared;
using System;
using System.Linq;
using ShipWorks.Shipping.Carriers.FedEx;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx
{
    public class FedExShipmentTypeTest : IDisposable
    {
        private readonly FedExShipmentType testObject;
        readonly AutoMock mock;

        public FedExShipmentTypeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<FedExShipmentType>();
        }

        [Fact]
        public void SupportsMultiplePackages_ReturnsTrue()
        {
            Assert.True(testObject.SupportsMultiplePackages);
        }

        [Fact]
        public void ShipmentTypeCode_ReturnsFedEx()
        {
            Assert.Equal(ShipmentTypeCode.FedEx, testObject.ShipmentTypeCode);
        }

        [Fact]
        public void GetPackageAdapters_ReturnsPackageAdapterPerPackage()
        {
            var shipment = new ShipmentEntity();
            var fedexShipment = new FedExShipmentEntity();
            fedexShipment.Packages.Add(new FedExPackageEntity());
            fedexShipment.Packages.Add(new FedExPackageEntity());
            fedexShipment.Packages.Add(new FedExPackageEntity());

            shipment.FedEx = fedexShipment;
            
            Assert.Equal(3, testObject.GetPackageAdapters(shipment).Count());
        }
        
        [Fact]
        public void RedistributeContentWeight_SetsPackageWeightToShipmentWeightDividedByPackageCount()
        {
            var shipment = new ShipmentEntity();
            shipment.ContentWeight = 123;
            var fedexShipment = new FedExShipmentEntity();
            fedexShipment.Packages.Add(new FedExPackageEntity());
            fedexShipment.Packages.Add(new FedExPackageEntity());
            fedexShipment.Packages.Add(new FedExPackageEntity());

            shipment.FedEx = fedexShipment;

            Assert.True(FedExShipmentType.RedistributeContentWeight(shipment));
            Assert.Equal(41, shipment.FedEx.Packages[0].Weight);
        }


        [Fact]
        public void RedistributeContentWeight_DoesNothingWhenPackageWeightAndShipmentWeightMatch()
        {
            var shipment = new ShipmentEntity();
            shipment.ContentWeight = 123;
            var fedexShipment = new FedExShipmentEntity();
            fedexShipment.Packages.Add(new FedExPackageEntity() { Weight = 41 });
            fedexShipment.Packages.Add(new FedExPackageEntity() { Weight = 41 });
            fedexShipment.Packages.Add(new FedExPackageEntity() { Weight = 41 });

            shipment.FedEx = fedexShipment;

            Assert.False(FedExShipmentType.RedistributeContentWeight(shipment));
        }

        [Fact]
        public void UpdateTotalWeight_SetsShipmentContentWeight()
        {
            var shipment = new ShipmentEntity();
            shipment.ContentWeight = 123;
            var fedexShipment = new FedExShipmentEntity();
            fedexShipment.Packages.Add(new FedExPackageEntity() { Weight = 2 });
            fedexShipment.Packages.Add(new FedExPackageEntity() { Weight = 3 });
            fedexShipment.Packages.Add(new FedExPackageEntity() { Weight = 4 });

            shipment.FedEx = fedexShipment;

            testObject.UpdateTotalWeight(shipment);

            Assert.Equal(9, shipment.ContentWeight);
        }

        [Fact]
        public void UpdateTotalWeight_SetsShipmentTotalWeight()
        {
            var shipment = new ShipmentEntity();
            shipment.ContentWeight = 123;
            var fedexShipment = new FedExShipmentEntity();
            fedexShipment.Packages.Add(new FedExPackageEntity() { Weight = 2 });
            fedexShipment.Packages.Add(new FedExPackageEntity() { Weight = 3 });
            fedexShipment.Packages.Add(new FedExPackageEntity() { Weight = 4, DimsAddWeight = true, DimsWeight = 3 });

            shipment.FedEx = fedexShipment;

            testObject.UpdateTotalWeight(shipment);

            Assert.Equal(12, shipment.TotalWeight);
        }

        [Fact]
        public void GetParcelCount_ReturnsParcelCount()
        {
            var shipment = new ShipmentEntity();
            shipment.ContentWeight = 123;
            var fedexShipment = new FedExShipmentEntity();
            fedexShipment.Packages.Add(new FedExPackageEntity() { Weight = 2 });
            fedexShipment.Packages.Add(new FedExPackageEntity() { Weight = 3 });
            fedexShipment.Packages.Add(new FedExPackageEntity() { Weight = 4, DimsAddWeight = true, DimsWeight = 3 });

            shipment.FedEx = fedexShipment;
            
            Assert.Equal(3,testObject.GetParcelCount(shipment));
        }
        
        [Theory]
        [InlineData(true, 1.1, 1.2, 2.3)]
        [InlineData(false, 1.1, 1.2, 1.2)]
        [InlineData(true, 0, 1.2, 1.2)]
        [InlineData(false, 0, 1.2, 1.2)]
        public void GetParcelDetail_HasCorrectTotalWeight(bool dimsAddWeight, double dimsWeight, double weight, double expectedTotalWeight)
        {
            var testObject = mock.Create<FedExShipmentType>();

            var shipment = new ShipmentEntity()
            {
                TrackingNumber = "test",
                FedEx = new FedExShipmentEntity()
            };
            shipment.FedEx.Packages.Add(new FedExPackageEntity()
            {
                DimsAddWeight = dimsAddWeight,
                DimsWeight = dimsWeight,
                Weight = weight
            });

            var parcelDetail = testObject.GetParcelDetail(shipment, 0);

            Assert.Equal(expectedTotalWeight, parcelDetail.TotalWeight);
        }
        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
