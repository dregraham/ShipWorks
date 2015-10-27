using System;
using System.Linq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.Api;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using System.IO;
using System.Reflection;
using System.Linq.Expressions;
using System.Collections.Generic;
using ShipWorks.Shipping.Carriers.Amazon.Enums;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.Amazon.Api
{
    public class AmazonRateTests
    {
        [Fact]
        public void GetRates_SendsOrderId_ToAmazonApi()
        {
            VerifyApiRequest((s, o) => o.AmazonOrderID = "123", x => x.AmazonOrderId == "123");
        }

        [Fact]
        public void GetRates_SendsShipmentWeight_ToAmazonApi()
        {
            VerifyApiRequest((s, o) => s.TotalWeight = 1.23, x => x.Weight == 1.23);
        }

        [Fact]
        public void GetRates_SendsInsurance_ToAmazonApiWhenDeclaredValueIsSet()
        {
            VerifyApiRequest((s, o) => s.Amazon.DeclaredValue = 12,
            x => x.Insurance.Amount == 12 &&
                x.Insurance.CurrencyCode == "USD");
        }

        [Fact]
        public void GetRates_SendsNullInsurance_ToAmazonApiWhenDeclaredValueIsNotSet()
        {
            VerifyApiRequest((s, o) => s.Amazon.DeclaredValue = null, x => x.Insurance == null);
        }

        [Fact]
        public void GetRates_SendsOrderItems_ToAmazonApi()
        {
            VerifyApiRequest((s, o) =>
            {
                o.OrderItems.Add(new AmazonOrderItemEntity
                {
                    AmazonOrderItemCode = "abc123",
                    Quantity = 2
                });
            },
            x => x.ItemList[0].OrderItemId == "abc123" &&
                x.ItemList[0].Quantity == 2);
        }

        [Fact]
        public void GetRates_SendsShipmentDimensions_ToAmazonApi()
        {
            VerifyApiRequest((s, o) =>
            {
                s.Amazon.DimsHeight = 3;
                s.Amazon.DimsLength = 4;
                s.Amazon.DimsWidth = 2;
            }, 
            x => x.PackageDimensions.Height == 3 &&
                x.PackageDimensions.Length == 4 &&
                x.PackageDimensions.Width == 2);
        }

        [Fact]
        public void GetRates_SendsShipmentAddress_ToAmazonApi()
        {
            VerifyApiRequest((s, o) =>
            {
                s.OriginStreet1 = "1 Memorial Dr.";
                s.OriginStreet2 = "Suite 2000";
                s.OriginStreet3 = "Baz";
                s.OriginCity = "St. Louis";
                s.OriginCountryCode = "US";
                s.OriginPhone = "314-555-1234";
                s.OriginUnparsedName = "John Doe";
                s.OriginPostalCode = "63102";
                s.OriginStateProvCode = "MO";
                s.OriginEmail = "foo@example.com";
            },
            x => x.ShipFromAddress.AddressLine1 == "1 Memorial Dr." &&
                x.ShipFromAddress.AddressLine2 == "Suite 2000" &&
                x.ShipFromAddress.AddressLine3 == "Baz" &&
                x.ShipFromAddress.City == "St. Louis" &&
                x.ShipFromAddress.CountryCode == "US" &&
                x.ShipFromAddress.Phone == "314-555-1234" &&
                x.ShipFromAddress.Name == "John Doe" &&
                x.ShipFromAddress.PostalCode == "63102" &&
                x.ShipFromAddress.StateOrProvinceCode == "MO" &&
                x.ShipFromAddress.Email == "foo@example.com");
        }

        [Fact]
        public void GetRates_SendsShippingServiceOptions_ToAmazonApi()
        {
            VerifyApiRequest((s, o) =>
            {
                s.Amazon.DeliveryExperience = (int)AmazonDeliveryExperienceType.DeliveryConfirmationWithoutSignature;
                s.Amazon.CarrierWillPickUp = true;
            },
            x => x.ShippingServiceOptions.DeliveryExperience == "DeliveryConfirmationWithoutSignature" &&
                x.ShippingServiceOptions.CarrierWillPickUp == true);
        }

        [Fact]
        public void GetRates_ReturnsThreeRates_WhenApiResponseHasThreeServices()
        {
            GetEligibleShippingServicesResponse response = ResponseWithServices(() => Enumerable.Range(1, 3)
                .Select(x => new ShippingService { Rate = new Rate { Amount = x } }));

            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IAmazonShippingWebClient>()
                    .Setup(w => w.GetRates(It.IsAny<ShipmentRequestDetails>(), It.IsAny<AmazonMwsWebClientSettings>()))
                    .Returns(response);

                AmazonRates testObject = mock.Create<AmazonRates>();

                RateGroup result = testObject.GetRates(SampleShipment);

                Assert.Equal(3, result.Rates.Count);
            }
        }

        [Fact]
        public void GetRates_ReturnsNoRates_WhenApiReturnsOneServiceWithNullRate()
        {
            GetEligibleShippingServicesResponse response = ResponseWithService(new ShippingService());

            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IAmazonShippingWebClient>()
                    .Setup(w => w.GetRates(It.IsAny<ShipmentRequestDetails>(), It.IsAny<AmazonMwsWebClientSettings>()))
                    .Returns(response);

                AmazonRates testObject = mock.Create<AmazonRates>();

                RateGroup result = testObject.GetRates(SampleShipment);

                Assert.Equal(0, result.Rates.Count);
            }
        }

        [Fact]
        public void GetRates_ReturnsRateWithDefaultCarrierName_WhenApiReturnsOneServiceWithNullCarrierName()
        {
            GetEligibleShippingServicesResponse response = ResponseWithService(new ShippingService {
                Rate = new Rate { Amount = 1 },
                ShippingServiceName = null
            });

            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IAmazonShippingWebClient>()
                    .Setup(w => w.GetRates(It.IsAny<ShipmentRequestDetails>(), It.IsAny<AmazonMwsWebClientSettings>()))
                    .Returns(response);

                AmazonRates testObject = mock.Create<AmazonRates>();

                RateGroup result = testObject.GetRates(SampleShipment);
                RateResult rateResult = result.Rates.FirstOrDefault();

                Assert.Equal("Unknown", rateResult.Description);
            }
        }

        [Fact]
        public void GetRates_ReturnsValidRate_WhenApiReturnsOneValidService()
        {
            GetEligibleShippingServicesResponse response = ResponseWithService(new ShippingService
            {
                Rate = new Rate { Amount = 2.34m },
                ShippingServiceName = "UPS",
                ShippingServiceId = "Foo",
                ShippingServiceOfferId = "Bar"
            });

            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IAmazonShippingWebClient>()
                    .Setup(w => w.GetRates(It.IsAny<ShipmentRequestDetails>(), It.IsAny<AmazonMwsWebClientSettings>()))
                    .Returns(response);

                AmazonRates testObject = mock.Create<AmazonRates>();

                RateGroup result = testObject.GetRates(SampleShipment);
                RateResult rateResult = result.Rates.FirstOrDefault();

                Assert.Equal("UPS", rateResult.Description);
                Assert.Equal(2.34m, rateResult.Amount);
                Assert.Equal("Foo", ((AmazonRateTag)rateResult.Tag).ShippingServiceId);
                Assert.Equal("Bar", ((AmazonRateTag)rateResult.Tag).ShippingServiceOfferId);
            }
        }
        
        private void VerifyApiRequest(Action<ShipmentEntity, AmazonOrderEntity> configureInput, Expression<Func<ShipmentRequestDetails, bool>> verifyCall)
        {
            ShipmentEntity shipment = SampleShipment;
            configureInput(shipment, shipment.Order as AmazonOrderEntity);

            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IAmazonShippingWebClient>()
                    .Setup(w => w.GetRates(It.IsAny<ShipmentRequestDetails>(), It.IsAny<AmazonMwsWebClientSettings>()))
                    .Returns(ResponseWithService(new ShippingService()));

                AmazonRates testObject = mock.Create<AmazonRates>();

                testObject.GetRates(shipment);

                mock.Mock<IAmazonShippingWebClient>()
                    .Verify(x => x.GetRates(It.Is(verifyCall), It.IsAny<AmazonMwsWebClientSettings>()));
            }
        }

        public GetEligibleShippingServicesResponse ResponseWithServices(Func<IEnumerable<ShippingService>> serviceCreator)
        {
            return new GetEligibleShippingServicesResponse
            {
                GetEligibleShippingServicesResult = new GetEligibleShippingServicesResult
                {
                    ShippingServiceList = new ShippingServiceList
                    {
                        ShippingService = serviceCreator().ToList()
                    }
                }
            };
        }

        public GetEligibleShippingServicesResponse ResponseWithService(ShippingService service)
        {
            return ResponseWithServices(() => new[] { service });
        }

        public GetEligibleShippingServicesResponse ValidResponse
        {
            get
            {
                return SerializationUtility.DeserializeFromXml<GetEligibleShippingServicesResponse>(GetEmbeddedResourceXml("ShipWorks.Tests.Shipping.Carriers.Amazon.Api.Artifacts.GetEligibleShippingServicesResponse.xml"));
            }
        }

        public ShipmentEntity SampleShipment
        {
            get
            {
                return new ShipmentEntity()
                {
                    Order = new AmazonOrderEntity()
                    {
                        AmazonOrderID = "10",
                    },
                    Amazon = new AmazonShipmentEntity()
                    {
                        DeclaredValue = 12,
                        DimsHeight = 1,
                        DimsLength = 1,
                        DimsWidth = 1,
                        DimsWeight = 1
                    }
                };
            }
        }

        private string GetEmbeddedResourceXml(string embeddedResourceName)
        {
            string xml = string.Empty;
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(embeddedResourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    xml = reader.ReadToEnd();
                }
            }
            return xml;
        }
    }
}
