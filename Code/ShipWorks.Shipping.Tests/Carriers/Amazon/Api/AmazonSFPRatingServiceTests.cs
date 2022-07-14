using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Autofac.Extras.Moq;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.SFP;
using ShipWorks.Shipping.Carriers.Amazon.SFP.Api;
using ShipWorks.Shipping.Carriers.Amazon.SFP.Api.DTOs;
using ShipWorks.Shipping.Carriers.Amazon.SFP.Platform;
using ShipWorks.Shipping.Carriers.Amazon.SFP.RateGroupFilters;
using ShipWorks.Shipping.Editing.Rating;
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
            mock.Mock<IAmazonSFPRateGroupFilter>()
                .Setup(x => x.Filter(It.IsAny<RateGroup>()))
                .Returns<RateGroup>(x => x);
        }

        [Fact]
        public void GetRates_ReturnsNoRates_WhenApiReturnsOneServiceWithNullRate()
        {
            GetEligibleShippingServicesResponse response = ResponseWithService(new ShippingService());

            mock.Mock<IAmazonSFPShippingWebClient>()
                    .Setup(w => w.GetRates(It.IsAny<ShipmentRequestDetails>(), It.IsAny<AmazonSFPShipmentEntity>()))
                    .Returns(response);

            AmazonSFPRatingService testObject = mock.Create<AmazonSFPRatingService>();

            RateGroup result = testObject.GetRates(SampleShipmentAmazonOrder(AmazonIsPrime.Yes));

            Assert.Equal(0, result.Rates.Count);
        }

        [Fact]
        public void GetRates_ReturnsAmazonShippingException_WhenOrderIsNotIAmazonOrder()
        {
            AmazonSFPRatingService testObject = mock.Create<AmazonSFPRatingService>();

            Assert.Throws<AmazonSFPShippingException>(() => testObject.GetRates(SampleShipmentNotIAmazonOrder));
        }

        //[Fact]
        //public void GetRates_RatesArePassedToFilters()
        //{
        //    RateGroup filteredRate = mock.CreateMock<RateGroup>().Object;

        //    GetEligibleShippingServicesResponse response = ResponseWithService(new ShippingService
        //    {
        //        Rate = new Rate { Amount = 2.34M },
        //        ShippingServiceName = "UPS",
        //        ShippingServiceId = "Foo",
        //        ShippingServiceOfferId = "Bar"
        //    });

        //    mock.Mock<IAmazonSfpRateGroupFactory>()
        //        .Setup(x => x.GetRateGroupFromResponse(response))
        //        .Returns(filteredRate);

        //    mock.Mock<IAmazonSFPShippingWebClient>()
        //        .Setup(w => w.GetRates(It.IsAny<ShipmentRequestDetails>(), It.IsAny<AmazonSFPShipmentEntity>()))
        //        .Returns(response);

        //    AmazonSFPRatingService testObject = mock.Create<AmazonSFPRatingService>();

        //    RateGroup rates = testObject.GetRates(SampleShipmentAmazonOrder(AmazonIsPrime.Yes));

        //    Assert.Equal(filteredRate, rates);
        //}

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
            ShipmentEntity shipment = SampleShipmentAmazonOrder(AmazonIsPrime.Yes);
            configureInput(shipment, shipment.Order as AmazonOrderEntity);

            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IAmazonSFPShippingWebClient>()
                    .Setup(w => w.GetRates(It.IsAny<ShipmentRequestDetails>(), It.IsAny<AmazonSFPShipmentEntity>()))
                    .Returns(ResponseWithService(new ShippingService()));

                AmazonSFPRatingService testObject = mock.Create<AmazonSFPRatingService>();

                testObject.GetRates(shipment);

                mock.Mock<IAmazonSFPShippingWebClient>()
                    .Verify(x => x.GetRates(It.Is(verifyCall), It.IsAny<AmazonSFPShipmentEntity>()));
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

        public ShipmentEntity SampleShipmentAmazonOrder(AmazonIsPrime isPrime)
        {
            return new ShipmentEntity()
            {
                Order = new AmazonOrderEntity()
                {
                    AmazonOrderID = "10",
                    IsPrime = (int) isPrime
                },
                AmazonSFP = new AmazonSFPShipmentEntity()
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
                    AmazonSFP = new AmazonSFPShipmentEntity()
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

        public ShipmentEntity SampleShipmentChannelAdvisorOrder(AmazonIsPrime isPrime)
        {
            return new ShipmentEntity()
            {
                Order = new ChannelAdvisorOrderEntity()
                {
                    IsPrime = (int) isPrime,
                    Store = new ChannelAdvisorStoreEntity()
                },
                AmazonSFP = new AmazonSFPShipmentEntity()
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
