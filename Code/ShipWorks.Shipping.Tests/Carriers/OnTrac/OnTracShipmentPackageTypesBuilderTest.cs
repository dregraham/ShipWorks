using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.OnTrac;
using ShipWorks.Shipping.Carriers.OnTrac.Enums;
using ShipWorks.Shipping.Settings;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.OnTrac
{
    public class OnTracShipmentPackageTypesBuilderTest
    {
        [Fact]
        public void BuildPackageTypeDictionary_DelegatesToShipmentType()
        {
            List<ShipmentEntity> shipments = GetSingleShipment(OnTracServiceType.Ground, OnTracPackagingType.Package).ToList();

            using (var mock = AutoMock.GetLoose())
            {
                var onTracShipmentType = mock.MockRepository.Create<OnTracShipmentType>();
                onTracShipmentType.Setup(x => x.BuildPackageTypeDictionary(It.IsAny<List<ShipmentEntity>>(), It.IsAny<IExcludedPackageTypeRepository>()))
                    .Verifiable();

                mock.Provide(onTracShipmentType.Object);

                var testObject = mock.Create<OnTracShipmentPackageTypesBuilder>();

                testObject.BuildPackageTypeDictionary(shipments);

                onTracShipmentType.Verify(s => s.BuildPackageTypeDictionary(It.IsAny<List<ShipmentEntity>>(), It.IsAny<IExcludedPackageTypeRepository>()));
            }
        }

        /// <summary>
        /// Returns an IEnumberable with 1 shipment that is of type OnTracServiceType.
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<ShipmentEntity> GetSingleShipment(OnTracServiceType serviceType, OnTracPackagingType packageType)
        {
            return new[] {
                new ShipmentEntity()
                {
                    OnTrac = new OnTracShipmentEntity()
                    {
                        Service = (int)serviceType,
                        PackagingType = (int) packageType
                    }
                }
            };
        }
    }
}
