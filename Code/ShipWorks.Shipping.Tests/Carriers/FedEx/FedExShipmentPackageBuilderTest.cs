using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx
{
    public class FedExShipmentPackageBuilderTest
    {
        [Fact]
        public void BuildPackageTypeDictionary_ReturnsCustomPackagingOnly_ShipmentsAreNull()
        {
            var availablePackageTypes = new List<FedExPackagingType> { FedExPackagingType.Box }.Cast<int>();
            var validPackageTypes = new List<FedExPackagingType> { FedExPackagingType.Box };

            var packageTypes = BuildPackageTypeDictionary(availablePackageTypes, validPackageTypes, null);

            Assert.Equal(1, packageTypes.Count);
            Assert.Contains((int)FedExPackagingType.Custom, packageTypes.Keys);
        }

        [Fact]
        public void BuildPackageTypeDictionary_ReturnsCustomPackagingOnly_MultipleFedExShipmentsWithDifferentServiceTypes()
        {
            var shipments = GetSingleShipment(FedExServiceType.FedEx1DayFreight, FedExPackagingType.Box).Union(GetSingleShipment(FedExServiceType.FedExGround, FedExPackagingType.Box));
            var availablePackageTypes = new List<FedExPackagingType> { FedExPackagingType.Box }.Cast<int>();
            var validPackageTypes = new List<FedExPackagingType> { FedExPackagingType.Box };

            var packageTypes = BuildPackageTypeDictionary(availablePackageTypes, validPackageTypes, shipments);

            Assert.Equal(1, packageTypes.Count);
            Assert.Contains((int)FedExPackagingType.Custom, packageTypes.Keys);
        }

        [Fact]
        public void BuildPackageTypeDictionary_ReturnsValidPackageType_IfNoAvailablePackageTypesAndShipmentNotValid()
        {
            var shipments = GetSingleShipment(FedExServiceType.FedEx1DayFreight, FedExPackagingType.Box);
            var availablePackageTypes = new List<FedExPackagingType> { FedExPackagingType.Box }.Cast<int>();
            var validPackageTypes = new List<FedExPackagingType> { FedExPackagingType.Envelope };

            var packageTypes = BuildPackageTypeDictionary(availablePackageTypes, validPackageTypes, shipments);

            Assert.Equal(1, packageTypes.Count);
            Assert.Contains((int)FedExPackagingType.Envelope, packageTypes.Keys);
        }

        [Fact]
        public void BuildPackageTypeDictionary_ReturnsShipmentPackageType_ShipmentPackageTypeNotAvailableButValid()
        {
            var shipments = GetSingleShipment(FedExServiceType.FedEx1DayFreight, FedExPackagingType.Pak);
            var availablePackageTypes = new List<FedExPackagingType> { FedExPackagingType.Box }.Cast<int>();
            var validPackageTypes = new List<FedExPackagingType> { FedExPackagingType.ExtraLargeBox, FedExPackagingType.Pak };

            var packageTypes = BuildPackageTypeDictionary(availablePackageTypes, validPackageTypes, shipments);

            Assert.Contains((int)FedExPackagingType.Pak, packageTypes.Keys);
        }

        [Fact]
        public void BuildPackageTypeDictionary_ReturnsAvailableType_ShipmentPackageAvailableButNotInShipments()
        {
            var shipments = GetSingleShipment(FedExServiceType.FedEx1DayFreight, FedExPackagingType.Envelope);
            var availablePackageTypes = new List<FedExPackagingType> { FedExPackagingType.Box }.Cast<int>();
            var validPackageTypes = new List<FedExPackagingType> { FedExPackagingType.ExtraLargeBox, FedExPackagingType.Box };

            var packageTypes = BuildPackageTypeDictionary(availablePackageTypes, validPackageTypes, shipments);

            Assert.Contains((int)FedExPackagingType.Box, packageTypes.Keys);
        }

        /// <summary>
        /// Returns an IEnumberable with 1 shipment that is of type FedExServiceType.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="shipCountryCode">The ship country code.</param>
        /// <returns></returns>
        private static IEnumerable<ShipmentEntity> GetSingleShipment(FedExServiceType serviceType, FedExPackagingType packageType)
        {
            return new[] {
                new ShipmentEntity()
                {
                    FedEx = new FedExShipmentEntity()
                    {
                        Service = (int)serviceType,
                        PackagingType = (int) packageType
                    }
                }
            };
        }

        private static Dictionary<int, string> BuildPackageTypeDictionary(
            IEnumerable<int> availablePackageTypes,
            List<FedExPackagingType> validPackageTypes,
            IEnumerable<ShipmentEntity> shipments)
        {
            Dictionary<int, string> results;

            using (var mock = AutoMock.GetLoose())
            {
                var fedExShipmentType = mock.MockRepository.Create<FedExShipmentType>();
                fedExShipmentType.Setup(x => x.GetAvailablePackageTypes())
                    .Returns(availablePackageTypes);

                mock.Provide(fedExShipmentType.Object);

                mock.Mock<IFedExUtility>()
                    .Setup(x => x.GetValidPackagingTypes(It.IsAny<FedExServiceType>()))
                    .Returns(validPackageTypes);

                var testObject = mock.Create<FedExShipmentPackageTypesBuilder>();
                results = testObject.BuildPackageTypeDictionary(shipments);
            }
            return results;
        }
    }
}
