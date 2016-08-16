using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.Settings;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Postal.Express1
{
    public class Express1EndiciaShipmentPackageTypesBuilderTest
    {
        [Fact]
        public void BuildPackageTypeDictionary_DelegatesToShipmentType()
        {
            List<ShipmentEntity> shipments = GetSingleShipment(PostalServiceType.FirstClass, PostalPackagingType.Package).ToList();

            using (var mock = AutoMock.GetLoose())
            {
                Mock<Express1EndiciaShipmentType> shipmentType = mock.MockRepository.Create<Express1EndiciaShipmentType>();
                shipmentType.Setup(x => x.BuildPackageTypeDictionary(It.IsAny<List<ShipmentEntity>>(), It.IsAny<IExcludedPackageTypeRepository>()))
                    .Verifiable();

                mock.Provide(shipmentType.Object);

                var testObject = mock.Create<Express1EndiciaShipmentPackageTypesBuilder>();

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
