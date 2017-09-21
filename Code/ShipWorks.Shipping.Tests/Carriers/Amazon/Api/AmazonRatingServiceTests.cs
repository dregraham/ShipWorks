using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Carriers.Amazon.Api;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Shipping.Carriers.Amazon.RateGroupFilters;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.Amazon.Api
{
    public class AmazonRatingServiceTests : IDisposable
    {
        readonly AutoMock mock;

        public AmazonRatingServiceTests()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            mock.Mock<IAmazonRateGroupFilter>()
                .Setup(x => x.Filter(It.IsAny<RateGroup>()))
                .Returns<RateGroup>(x => x);
        }

        [Fact]
        public void GetRates_ReturnsNoRates_WhenApiReturnsOneServiceWithNullRate()
        {
            GetEligibleShippingServicesResponse response = ResponseWithService(new ShippingService());

            mock.Mock<IAmazonShippingWebClient>()
                    .Setup(w => w.GetRates(It.IsAny<ShipmentRequestDetails>(), It.IsAny<AmazonShipmentEntity>()))
                    .Returns(response);

            AmazonRatingService testObject = mock.Create<AmazonRatingService>();

            RateGroup result = testObject.GetRates(SampleShipmentAmazonOrder(AmazonMwsIsPrime.Yes));

            Assert.Equal(0, result.Rates.Count);
        }

        [Theory]
        [InlineData(AmazonMwsIsPrime.No)]
        [InlineData(AmazonMwsIsPrime.Unknown)]
        public void GetRates_ReturnsAmazonShippingException_WhenNotPrimeOrder(AmazonMwsIsPrime isPrime)
        {
            AmazonRatingService testObject = mock.Create<AmazonRatingService>();

            Assert.Throws<AmazonShippingException>(() => testObject.GetRates(SampleShipmentAmazonOrder(isPrime)));
        }

        [Fact]
        public void GetRates_ReturnsAmazonShippingException_WhenOrderIsNotIAmazonOrder()
        {
            AmazonRatingService testObject = mock.Create<AmazonRatingService>();

            Assert.Throws<AmazonShippingException>(() => testObject.GetRates(SampleShipmentNotIAmazonOrder));
        }

        [Fact]
        public void GetRates_RatesArePassedToFilters()
        {
            RateGroup filteredRate = mock.CreateMock<RateGroup>().Object;

            GetEligibleShippingServicesResponse response = ResponseWithService(new ShippingService
            {
                Rate = new Rate { Amount = 2.34M },
                ShippingServiceName = "UPS",
                ShippingServiceId = "Foo",
                ShippingServiceOfferId = "Bar"
            });

            mock.Mock<IAmazonRateGroupFactory>()
                .Setup(x => x.GetRateGroupFromResponse(response))
                .Returns(filteredRate);

            mock.Mock<IAmazonShippingWebClient>()
                .Setup(w => w.GetRates(It.IsAny<ShipmentRequestDetails>(), It.IsAny<AmazonShipmentEntity>()))
                .Returns(response);

            AmazonRatingService testObject = mock.Create<AmazonRatingService>();

            RateGroup rates = testObject.GetRates(SampleShipmentAmazonOrder(AmazonMwsIsPrime.Yes));

            Assert.Equal(filteredRate, rates);
        }

        private static GetEligibleShippingServicesResponse GetEligibleShippingServicesResponse(List<string> tAndC, List<string> tAndC2)
        {
            tAndC.AddRange(tAndC2);

            GetEligibleShippingServicesResponse response = new GetEligibleShippingServicesResponse
            {
                GetEligibleShippingServicesResult = new GetEligibleShippingServicesResult
                {
                    ShippingServiceList = new ShippingServiceList { ShippingService = new List<ShippingService>() },
                    TermsAndConditionsNotAcceptedCarrierList = new TermsAndConditionsNotAcceptedCarrierList()
                    {
                        TermsAndConditionsNotAcceptedCarrier = new TermsAndConditionsNotAcceptedCarrier()
                        {
                            CarrierName = tAndC
                        }
                    }
                }
            };
            return response;
        }

        private void VerifyApiRequest(Action<ShipmentEntity, AmazonOrderEntity> configureInput, Expression<Func<ShipmentRequestDetails, bool>> verifyCall)
        {
            ShipmentEntity shipment = SampleShipmentAmazonOrder(AmazonMwsIsPrime.Yes);
            configureInput(shipment, shipment.Order as AmazonOrderEntity);

            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IAmazonShippingWebClient>()
                    .Setup(w => w.GetRates(It.IsAny<ShipmentRequestDetails>(), It.IsAny<AmazonShipmentEntity>()))
                    .Returns(ResponseWithService(new ShippingService()));

                AmazonRatingService testObject = mock.Create<AmazonRatingService>();

                testObject.GetRates(shipment);

                mock.Mock<IAmazonShippingWebClient>()
                    .Verify(x => x.GetRates(It.Is(verifyCall), It.IsAny<AmazonShipmentEntity>()));
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
                string resource = GetEmbeddedResourceXml("ShipWorks.Tests.Shipping.Carriers.Amazon.Api.Artifacts.GetEligibleShippingServicesResponse.xml");
                return SerializationUtility.DeserializeFromXml<GetEligibleShippingServicesResponse>(resource);
            }
        }

        public ShipmentEntity SampleShipmentAmazonOrder(AmazonMwsIsPrime isPrime)
        {
            return new ShipmentEntity()
            {
                Order = new AmazonOrderEntity()
                {
                    AmazonOrderID = "10",
                    IsPrime = (int) isPrime
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

        public ShipmentEntity SampleShipmentNotIAmazonOrder
        {
            get
            {
                return new ShipmentEntity()
                {
                    Order = new EbayOrderEntity()
                    {
                        Store = new EbayStoreEntity()
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

        public ShipmentEntity SampleShipmentChannelAdvisorOrder(AmazonMwsIsPrime isPrime)
        {
            return new ShipmentEntity()
            {
                Order = new ChannelAdvisorOrderEntity()
                {
                    IsPrime = (int) isPrime,
                    Store = new ChannelAdvisorStoreEntity()
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

        public void Dispose() => mock?.Dispose();
    }
}
