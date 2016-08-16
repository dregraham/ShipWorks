using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx
{

    public class FedExShipmentServicesBuilderTest
    {
        [Fact]
        public void BuildServiceTypeDictionary_ReturnsGround_ShipmentsAreNullGroundValidAndNotExcluded()
        {
            IEnumerable<ShipmentEntity> shipments = Enumerable.Empty<ShipmentEntity>();
            var availableServiceTypes = new List<FedExServiceType>() { FedExServiceType.FedExGround }.Cast<int>();
            var validServiceTypes = new List<FedExServiceType>() { FedExServiceType.FedExGround };

            var results = BuildServiceTypeDictionary(availableServiceTypes, validServiceTypes, shipments);

            Assert.Contains((int)FedExServiceType.FedExGround, results.Keys);
        }

        [Fact]
        public void BuildServiceTypeDictionary_DoesNotReturnGround_ShipmentsHaveGroundAndGroundAvailableAndNotValid()
        {
            IEnumerable<ShipmentEntity> shipments = GetSingleShipment(FedExServiceType.FedExGround);
            var availableServiceTypes = new List<FedExServiceType>() { FedExServiceType.FedExGround }.Cast<int>();
            var validServiceTypes = new List<FedExServiceType>() { FedExServiceType.FedEx2Day };

            var results = BuildServiceTypeDictionary(availableServiceTypes, validServiceTypes, shipments);

            Assert.DoesNotContain((int)FedExServiceType.FedExGround, results.Keys);
        }

        [Fact]
        public void BuildServiceTypeDictionary_ReturnsFedExGround_ShipmentUsesFedExGroundAndGroundIsValidServiceAndAllDomestic()
        {
            IEnumerable<ShipmentEntity> shipments = GetSingleShipment(FedExServiceType.FedExGround);
            var availableServiceTypes = Enumerable.Empty<int>();
            var validServiceTypes = new List<FedExServiceType> { FedExServiceType.FedExGround };

            var results = BuildServiceTypeDictionary(availableServiceTypes, validServiceTypes, shipments);

            Assert.Contains((int)FedExServiceType.FedExGround, results.Keys);
        }

        [Fact]
        public void BuildServiceTypeDictionary_ReturnsFedExGround_ShipmentUsesFedExGroundAndGroundIsValidServiceAndAllInternational()
        {
            IEnumerable<ShipmentEntity> shipments = GetSingleShipment(FedExServiceType.FedExGround, "IT");
            var availableServiceTypes = Enumerable.Empty<int>();
            var validServiceTypes = new List<FedExServiceType>() { FedExServiceType.FedExGround };

            var results = BuildServiceTypeDictionary(availableServiceTypes, validServiceTypes, shipments);

            Assert.Contains((int)FedExServiceType.FedExGround, results.Keys);
        }

        [Fact]
        public void BuildServiceTypeDictionary_DoesntAddShipmentTypesFromShipments_WhenShipmentsAreMixOfDomesticAndInternational()
        {
            List<ShipmentEntity> shipments = GetSingleShipment(FedExServiceType.FedExGround).Union(GetSingleShipment(FedExServiceType.FedEx2Day, "IT")).ToList();
            
            var availableServiceTypes = new List<FedExServiceType>() { FedExServiceType.FedExGround }.Cast<int>();
            var validServiceTypes = new List<FedExServiceType>() { FedExServiceType.FedExGround };

            var results = BuildServiceTypeDictionary(availableServiceTypes, validServiceTypes, shipments);

            Assert.DoesNotContain((int)FedExServiceType.FedExGround, results.Keys);
        }

        /// <summary>
        /// Returns an IEnumberable with 1 shipment that is of type FedExServiceType.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="shipCountryCode">The ship country code.</param>
        /// <returns></returns>
        private static IEnumerable<ShipmentEntity> GetSingleShipment(FedExServiceType serviceType, string shipCountryCode="US")
        {
            return new [] {
                new ShipmentEntity()
                {
                    ShipCountryCode = shipCountryCode,
                    FedEx = new FedExShipmentEntity()
                    {
                        Service = (int)serviceType
                    }
                }
            };
        }

        /// <summary>
        /// Calls BuildServiceTypeDictionary with the following values set.
        /// </summary>
        /// <param name="availableServiceTypes">The available service types. (Those not excluded in fedex settings)</param>
        /// <param name="validServiceTypes">The valid service types for this shipment.</param>
        /// <param name="shipments">The shipments.</param>
        /// <returns></returns>
        private static Dictionary<int, string> BuildServiceTypeDictionary(
            IEnumerable<int> availableServiceTypes, 
            List<FedExServiceType> validServiceTypes, 
            IEnumerable<ShipmentEntity> shipments)
        {
            Dictionary<int, string> results;

            using (var mock = AutoMock.GetLoose())
            {
                var fedExShipmentType = mock.MockRepository.Create<FedExShipmentType>();
                fedExShipmentType.Setup(x => x.GetAvailableServiceTypes())
                    .Returns(availableServiceTypes);
                fedExShipmentType
                    .Setup(x => x.IsDomestic(It.IsAny<ShipmentEntity>()))
                    .Returns((ShipmentEntity s) => s.ShipCountryCode == "US");

                mock.Provide(fedExShipmentType.Object);

                mock.Mock<IFedExUtility>()
                    .Setup(x => x.GetValidServiceTypes(It.IsAny<IEnumerable<ShipmentEntity>>()))
                    .Returns(validServiceTypes);

                mock.Mock<IShippingManager>()
                    .Setup(x => x.GetOverriddenStoreShipment(It.IsAny<ShipmentEntity>()))
                    .Returns((ShipmentEntity x) => x);

                var testObject = mock.Create<FedExShipmentServicesBuilder>();
                results = testObject.BuildServiceTypeDictionary(shipments);
            }
            return results;
        }
    }
}
