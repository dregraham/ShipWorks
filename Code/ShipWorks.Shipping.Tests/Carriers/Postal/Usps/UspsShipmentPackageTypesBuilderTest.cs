using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Settings;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Postal.Usps
{
    public class UspsShipmentPackageTypesBuilderTest
    {
        [Fact]
        public void BuildPackageTypeDictionary_DelegatesToShipmentType()
        {
            List<ShipmentEntity> shipments = GetSingleShipment(PostalServiceType.FirstClass, PostalPackagingType.Package).ToList();

            using (var mock = AutoMock.GetLoose())
            {
                Mock<UspsShipmentType> shipmentType = mock.MockRepository.Create<UspsShipmentType>();
                shipmentType.Setup(x => x.BuildPackageTypeDictionary(It.IsAny<List<ShipmentEntity>>(), It.IsAny<IExcludedPackageTypeRepository>()))
                    .Verifiable();

                mock.Provide(shipmentType.Object);

                var testObject = mock.Create<UspsShipmentPackageTypesBuilder>();

                testObject.BuildPackageTypeDictionary(shipments);

                shipmentType.Verify(s => s.BuildPackageTypeDictionary(It.IsAny<List<ShipmentEntity>>(), It.IsAny<IExcludedPackageTypeRepository>()));
            }
        }

        /// <summary>
        /// Returns an IEnumberable with 1 shipment that is of type EndiciaServiceType.
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<ShipmentEntity> GetSingleShipment(PostalServiceType serviceType, PostalPackagingType packageType)
        {
            return new[] {
                new ShipmentEntity()
                {
                    Postal = new PostalShipmentEntity()
                    {
                        Service = (int)serviceType,
                        PackagingType = (int) packageType
                    }
                }
            };
        }
    }
}
