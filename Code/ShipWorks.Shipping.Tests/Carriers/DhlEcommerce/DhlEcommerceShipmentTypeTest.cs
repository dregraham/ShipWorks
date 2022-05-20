using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.DhlEcommerce;
using ShipWorks.Shipping.Settings;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.DhlEcommerce
{
    public class DhlEcommerceShipmentTypeTest
    {
        private readonly AutoMock mock;
        private readonly DhlEcommerceShipmentType testObject;

        public DhlEcommerceShipmentTypeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<DhlEcommerceShipmentType>();
        }

        [Fact]
        public void BuildPackageTypeDictionary_GetsAllAvailable_PackageTypes()
        {
            var excludedPackages = mock.Mock<IExcludedPackageTypeRepository>();

            var shipments = new List<ShipmentEntity>();
            var shipment = new ShipmentEntity()
            {
                DhlEcommerce = new DhlEcommerceShipmentEntity()
            };

            shipments.Add(shipment);
            var result = testObject.BuildPackageTypeDictionary(shipments, excludedPackages.Object);
            Assert.Equal(Enum.GetValues(typeof(DhlEcommercePackagingType)).Length, result.Count);
        }

        [Fact]
        public void BuildPackageTypeDictionary_Excludes_PackageTypes()
        {
            var excludedPackages = mock.Mock<IExcludedPackageTypeRepository>();
            excludedPackages.Setup(e => e.GetExcludedPackageTypes(It.IsAny<ShipmentType>())).Returns(new List<ExcludedPackageTypeEntity> { new ExcludedPackageTypeEntity() { PackageType = (int) DhlEcommercePackagingType.MachinableParcel } });

            var shipments = new List<ShipmentEntity>();
            var shipment = new ShipmentEntity()
            {
                DhlEcommerce = new DhlEcommerceShipmentEntity()
            };

            shipments.Add(shipment);
            var result = testObject.BuildPackageTypeDictionary(shipments, excludedPackages.Object);
            Assert.False(result.ContainsKey((int) DhlEcommercePackagingType.MachinableParcel));
        }

        [Fact]
        public void BuildPackageTypeDictionary_DoenstExclude_PackageTypes_AlreadyPresent()
        {
            var excludedPackages = mock.Mock<IExcludedPackageTypeRepository>();
            excludedPackages.Setup(e => e.GetExcludedPackageTypes(It.IsAny<ShipmentType>())).Returns(
                new List<ExcludedPackageTypeEntity> { 
                    new ExcludedPackageTypeEntity() { PackageType = (int) DhlEcommercePackagingType.MachinableParcel },
                    new ExcludedPackageTypeEntity() { PackageType = (int) DhlEcommercePackagingType.BpmParcel },
                });

            var shipments = new List<ShipmentEntity>();
            var shipment = new ShipmentEntity()
            {
                DhlEcommerce = new DhlEcommerceShipmentEntity()
                {
                    PackagingType = (int) DhlEcommercePackagingType.BpmParcel
                }
            };

            shipments.Add(shipment);
            var result = testObject.BuildPackageTypeDictionary(shipments, excludedPackages.Object);
            Assert.False(result.ContainsKey((int) DhlEcommercePackagingType.MachinableParcel));
            Assert.True(result.ContainsKey((int) DhlEcommercePackagingType.BpmParcel));
        }
    }
}
