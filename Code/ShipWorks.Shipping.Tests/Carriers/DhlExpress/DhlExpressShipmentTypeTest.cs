using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Dhl;
using ShipWorks.Tests.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.DhlExpress
{
    public class DhlExpressShipmentTypeTest : IDisposable
    {
        private readonly DhlExpressShipmentType testObject;
        readonly AutoMock mock;

        public DhlExpressShipmentTypeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<DhlExpressShipmentType>();
        }

        [Fact]
        public void SupportsMultiplePackages_ReturnsTrue()
        {
            Assert.True(testObject.SupportsMultiplePackages);
        }

        [Fact]
        public void ShipmentTypeCode_ReturnsDhlExpress()
        {
            Assert.Equal(ShipmentTypeCode.DhlExpress, testObject.ShipmentTypeCode);
        }

        [Fact]
        public void GetPackageAdapters_ReturnsPackageAdapterPerPackage()
        {
            var shipment = new ShipmentEntity();
            var dhlShipment = new DhlExpressShipmentEntity();
            dhlShipment.Packages.Add(new DhlExpressPackageEntity());
            dhlShipment.Packages.Add(new DhlExpressPackageEntity());
            dhlShipment.Packages.Add(new DhlExpressPackageEntity());

            shipment.DhlExpress = dhlShipment;
            
            Assert.Equal(3, testObject.GetPackageAdapters(shipment).Count());
        }
        
        [Fact]
        public void RedistributeContentWeight_SetsPackageWeightToShipmentWeightDividedByPackageCount()
        {
            var shipment = new ShipmentEntity();
            shipment.ContentWeight = 123;
            var dhlShipment = new DhlExpressShipmentEntity();
            dhlShipment.Packages.Add(new DhlExpressPackageEntity());
            dhlShipment.Packages.Add(new DhlExpressPackageEntity());
            dhlShipment.Packages.Add(new DhlExpressPackageEntity());

            shipment.DhlExpress = dhlShipment;

            Assert.True(DhlExpressShipmentType.RedistributeContentWeight(shipment));
            Assert.Equal(41, shipment.DhlExpress.Packages[0].Weight);
        }


        [Fact]
        public void RedistributeContentWeight_DoesNothingWhenPackageWeightAndShipmentWeightMatch()
        {
            var shipment = new ShipmentEntity();
            shipment.ContentWeight = 123;
            var dhlShipment = new DhlExpressShipmentEntity();
            dhlShipment.Packages.Add(new DhlExpressPackageEntity() { Weight = 41 });
            dhlShipment.Packages.Add(new DhlExpressPackageEntity() { Weight = 41 });
            dhlShipment.Packages.Add(new DhlExpressPackageEntity() { Weight = 41 });

            shipment.DhlExpress = dhlShipment;

            Assert.False(DhlExpressShipmentType.RedistributeContentWeight(shipment));
        }

        [Fact]
        public void UpdateTotalWeight_SetsShipmentContentWeight()
        {
            var shipment = new ShipmentEntity();
            shipment.ContentWeight = 123;
            var dhlShipment = new DhlExpressShipmentEntity();
            dhlShipment.Packages.Add(new DhlExpressPackageEntity() { Weight = 2 });
            dhlShipment.Packages.Add(new DhlExpressPackageEntity() { Weight = 3 });
            dhlShipment.Packages.Add(new DhlExpressPackageEntity() { Weight = 4 });

            shipment.DhlExpress = dhlShipment;

            testObject.UpdateTotalWeight(shipment);

            Assert.Equal(9, shipment.ContentWeight);
        }

        [Fact]
        public void UpdateTotalWeight_SetsShipmentTotalWeight()
        {
            var shipment = new ShipmentEntity();
            shipment.ContentWeight = 123;
            var dhlShipment = new DhlExpressShipmentEntity();
            dhlShipment.Packages.Add(new DhlExpressPackageEntity() { Weight = 2 });
            dhlShipment.Packages.Add(new DhlExpressPackageEntity() { Weight = 3 });
            dhlShipment.Packages.Add(new DhlExpressPackageEntity() { Weight = 4, DimsAddWeight = true, DimsWeight = 3 });

            shipment.DhlExpress = dhlShipment;

            testObject.UpdateTotalWeight(shipment);

            Assert.Equal(12, shipment.TotalWeight);
        }

        [Fact]
        public void GetParcelCount_ReturnsParcelCount()
        {
            var shipment = new ShipmentEntity();
            shipment.ContentWeight = 123;
            var dhlShipment = new DhlExpressShipmentEntity();
            dhlShipment.Packages.Add(new DhlExpressPackageEntity() { Weight = 2 });
            dhlShipment.Packages.Add(new DhlExpressPackageEntity() { Weight = 3 });
            dhlShipment.Packages.Add(new DhlExpressPackageEntity() { Weight = 4, DimsAddWeight = true, DimsWeight = 3 });

            shipment.DhlExpress = dhlShipment;
            
            Assert.Equal(3,testObject.GetParcelCount(shipment));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
