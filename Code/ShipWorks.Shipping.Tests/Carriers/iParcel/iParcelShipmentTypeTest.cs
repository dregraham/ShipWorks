using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Tests.Shared;
using System;
using System.Linq;
using ShipWorks.Shipping.Carriers.iParcel;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.iParcel
{
    public class iParcelShipmentTypeTest : IDisposable
    {
        private readonly iParcelShipmentType testObject;
        readonly AutoMock mock;

        public iParcelShipmentTypeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<iParcelShipmentType>();
        }

        [Fact]
        public void SupportsMultiplePackages_ReturnsTrue()
        {
            Assert.True(testObject.SupportsMultiplePackages);
        }

        [Fact]
        public void ShipmentTypeCode_ReturnsiParcel()
        {
            Assert.Equal(ShipmentTypeCode.iParcel, testObject.ShipmentTypeCode);
        }

        [Fact]
        public void GetPackageAdapters_ReturnsPackageAdapterPerPackage()
        {
            var shipment = new ShipmentEntity();
            var iParcelShipment = new IParcelShipmentEntity();
            iParcelShipment.Packages.Add(new IParcelPackageEntity());
            iParcelShipment.Packages.Add(new IParcelPackageEntity());
            iParcelShipment.Packages.Add(new IParcelPackageEntity());

            shipment.IParcel = iParcelShipment;
            
            Assert.Equal(3, testObject.GetPackageAdapters(shipment).Count());
        }
        
        [Fact]
        public void RedistributeContentWeight_SetsPackageWeightToShipmentWeightDividedByPackageCount()
        {
            var shipment = new ShipmentEntity();
            shipment.ContentWeight = 123;
            var iParcelShipment = new IParcelShipmentEntity();
            iParcelShipment.Packages.Add(new IParcelPackageEntity());
            iParcelShipment.Packages.Add(new IParcelPackageEntity());
            iParcelShipment.Packages.Add(new IParcelPackageEntity());

            shipment.IParcel = iParcelShipment;

            Assert.True(iParcelShipmentType.RedistributeContentWeight(shipment));
            Assert.Equal(41, shipment.IParcel.Packages[0].Weight);
        }


        [Fact]
        public void RedistributeContentWeight_DoesNothingWhenPackageWeightAndShipmentWeightMatch()
        {
            var shipment = new ShipmentEntity();
            shipment.ContentWeight = 123;
            var iParcelShipment = new IParcelShipmentEntity();
            iParcelShipment.Packages.Add(new IParcelPackageEntity() { Weight = 41 });
            iParcelShipment.Packages.Add(new IParcelPackageEntity() { Weight = 41 });
            iParcelShipment.Packages.Add(new IParcelPackageEntity() { Weight = 41 });

            shipment.IParcel = iParcelShipment;

            Assert.False(iParcelShipmentType.RedistributeContentWeight(shipment));
        }

        [Fact]
        public void UpdateTotalWeight_SetsShipmentContentWeight()
        {
            var shipment = new ShipmentEntity();
            shipment.ContentWeight = 123;
            var iParcelShipment = new IParcelShipmentEntity();
            iParcelShipment.Packages.Add(new IParcelPackageEntity() { Weight = 2 });
            iParcelShipment.Packages.Add(new IParcelPackageEntity() { Weight = 3 });
            iParcelShipment.Packages.Add(new IParcelPackageEntity() { Weight = 4 });

            shipment.IParcel = iParcelShipment;

            testObject.UpdateTotalWeight(shipment);

            Assert.Equal(9, shipment.ContentWeight);
        }

        [Fact]
        public void UpdateTotalWeight_SetsShipmentTotalWeight()
        {
            var shipment = new ShipmentEntity();
            shipment.ContentWeight = 123;
            var iParcelShipment = new IParcelShipmentEntity();
            iParcelShipment.Packages.Add(new IParcelPackageEntity() { Weight = 2 });
            iParcelShipment.Packages.Add(new IParcelPackageEntity() { Weight = 3 });
            iParcelShipment.Packages.Add(new IParcelPackageEntity() { Weight = 4, DimsAddWeight = true, DimsWeight = 3 });

            shipment.IParcel = iParcelShipment;

            testObject.UpdateTotalWeight(shipment);

            Assert.Equal(12, shipment.TotalWeight);
        }

        [Fact]
        public void GetParcelCount_ReturnsParcelCount()
        {
            var shipment = new ShipmentEntity();
            shipment.ContentWeight = 123;
            var iParcelShipment = new IParcelShipmentEntity();
            iParcelShipment.Packages.Add(new IParcelPackageEntity() { Weight = 2 });
            iParcelShipment.Packages.Add(new IParcelPackageEntity() { Weight = 3 });
            iParcelShipment.Packages.Add(new IParcelPackageEntity() { Weight = 4, DimsAddWeight = true, DimsWeight = 3 });

            shipment.IParcel = iParcelShipment;
            
            Assert.Equal(3,testObject.GetParcelCount(shipment));
        }
        
        [Theory]
        [InlineData(true, 1.1, 1.2, 2.3)]
        [InlineData(false, 1.1, 1.2, 1.2)]
        [InlineData(true, 0, 1.2, 1.2)]
        [InlineData(false, 0, 1.2, 1.2)]
        public void GetParcelDetail_HasCorrectTotalWeight(bool dimsAddWeight, double dimsWeight, double weight, double expectedTotalWeight)
        {
            var testObject = mock.Create<iParcelShipmentType>();

            var shipment = new ShipmentEntity()
            {
                TrackingNumber = "test",
                IParcel = new IParcelShipmentEntity()
            };
            shipment.IParcel.Packages.Add(new IParcelPackageEntity()
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
