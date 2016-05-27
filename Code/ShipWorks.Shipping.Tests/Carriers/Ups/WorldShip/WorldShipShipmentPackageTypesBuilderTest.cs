using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using ShipWorks.Shipping.Settings;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Ups.WorldShip
{
    public class WorldShipShipmentPackageTypesBuilderTest
    {
        [Fact]
        public void BuildPackageTypeDictionary_DelegatesToShipmentType()
        {
            List<ShipmentEntity> shipments = GetSingleShipment(UpsServiceType.UpsGround, UpsPackagingType.Custom).ToList();

            using (var mock = AutoMock.GetLoose())
            {
                Mock<WorldShipShipmentType> shipmentType = mock.MockRepository.Create<WorldShipShipmentType>();
                shipmentType.Setup(x => x.BuildPackageTypeDictionary(It.IsAny<List<ShipmentEntity>>(), It.IsAny<IExcludedPackageTypeRepository>()))
                    .Verifiable();

                mock.Provide(shipmentType.Object);

                var testObject = mock.Create<WorldShipShipmentPackageTypesBuilder>();

                testObject.BuildPackageTypeDictionary(shipments);

                shipmentType.Verify(s => s.BuildPackageTypeDictionary(It.IsAny<List<ShipmentEntity>>(), It.IsAny<IExcludedPackageTypeRepository>()));
            }
        }

        /// <summary>
        /// Returns an IEnumberable with 1 shipment
        /// </summary>
        private static IEnumerable<ShipmentEntity> GetSingleShipment(UpsServiceType serviceType, UpsPackagingType packageType)
        {
            return new[] {
                new ShipmentEntity()
                {
                    Ups = new UpsShipmentEntity()
                    {
                        Service = (int)serviceType
                    }
                }
            };
        }
    }
}
