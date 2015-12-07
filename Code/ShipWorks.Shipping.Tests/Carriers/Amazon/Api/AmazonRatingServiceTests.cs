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
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Amazon;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.Amazon.Api
{
    public class AmazonRatingServiceTests : IDisposable
    {
        AutoMock mock;

        public AmazonRatingServiceTests()
        {
            mock = AutoMock.GetFromRepository(new MockRepository(MockBehavior.Loose) { DefaultValue = DefaultValue.Mock });
            mock.Mock<IAmazonRateGroupFilter>()
                .Setup(x => x.Filter(It.IsAny<RateGroup>()))
                .Returns<RateGroup>(x => x);
        }

        [Fact]
        public void GetRates_ReturnsThreeRates_WhenApiResponseHasThreeServices()
        {
            GetEligibleShippingServicesResponse response = ResponseWithServices(() => Enumerable.Range(1, 3)
                .Select(x => new ShippingService { Rate = new Rate { Amount = x } }));

            mock.Mock<IAmazonShippingWebClient>()
                .Setup(w => w.GetRates(It.IsAny<ShipmentRequestDetails>()))
                .Returns(response);

            AmazonRatingService testObject = mock.Create<AmazonRatingService>();

            RateGroup result = testObject.GetRates(SampleShipmentAmazonOrer(AmazonMwsIsPrime.Yes));

            Assert.Equal(3, result.Rates.Count);
        }

        [Fact]
        public void GetRates_ReturnsNoRates_WhenApiReturnsOneServiceWithNullRate()
        {
            GetEligibleShippingServicesResponse response = ResponseWithService(new ShippingService());

            mock.Mock<IAmazonShippingWebClient>()
                    .Setup(w => w.GetRates(It.IsAny<ShipmentRequestDetails>()))
                    .Returns(response);

            AmazonRatingService testObject = mock.Create<AmazonRatingService>();

            RateGroup result = testObject.GetRates(SampleShipmentAmazonOrer(AmazonMwsIsPrime.Yes));

            Assert.Equal(0, result.Rates.Count);
        }

        [Fact]
        public void GetRates_ReturnsRateWithDefaultCarrierName_WhenApiReturnsOneServiceWithNullCarrierName()
        {
            GetEligibleShippingServicesResponse response = ResponseWithService(new ShippingService
            {
                Rate = new Rate { Amount = 1 },
                ShippingServiceName = null
            });

            mock.Mock<IAmazonShippingWebClient>()
                .Setup(w => w.GetRates(It.IsAny<ShipmentRequestDetails>()))
                .Returns(response);


            AmazonRatingService testObject = mock.Create<AmazonRatingService>();
            

            RateGroup result = testObject.GetRates(SampleShipmentAmazonOrer(AmazonMwsIsPrime.Yes));
            RateResult rateResult = result.Rates.FirstOrDefault();

            Assert.Equal("Unknown", rateResult.Description);
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

            mock.Mock<IAmazonShippingWebClient>()
                .Setup(w => w.GetRates(It.IsAny<ShipmentRequestDetails>()))
                .Returns(response);

            AmazonRatingService testObject = mock.Create<AmazonRatingService>();

            RateGroup result = testObject.GetRates(SampleShipmentAmazonOrer(AmazonMwsIsPrime.Yes));
            RateResult rateResult = result.Rates.FirstOrDefault();

            Assert.Equal("UPS", rateResult.Description);
            Assert.Equal(2.34m, rateResult.Amount);
            Assert.Equal("Foo", ((AmazonRateTag)rateResult.Tag).ShippingServiceId);
            Assert.Equal("Bar", ((AmazonRateTag)rateResult.Tag).ShippingServiceOfferId);
        }

        [Fact]
        public void GetRates_ReturnsTermsAndConditionsFootNoteFactory_WhenApiResponseHasTermsAndConditionsCarriers()
        {
            List<string> tAndC = new List<string>() { "FEDEX", "UPS" };
            List<string> tAndC2 = new List<string>() { "USPS" };
            GetEligibleShippingServicesResponse response = GetEligibleShippingServicesResponse(tAndC, tAndC2);

            mock.Mock<IAmazonShippingWebClient>()
                .Setup(w => w.GetRates(It.IsAny<ShipmentRequestDetails>()))
                .Returns(response);

            AmazonRatingService testObject = mock.Create<AmazonRatingService>();

            RateGroup result = testObject.GetRates(SampleShipmentAmazonOrer(AmazonMwsIsPrime.Yes));

            Assert.Equal(0, result.Rates.Count);
            Assert.Equal(1, result.FootnoteFactories.OfType<AmazonCarrierTermsAndConditionsNotAcceptedFootnoteFactory>().Count());
        }

        [Fact]
        public void GetRates_TermsAndConditionsFootNoteFactory_CreatesCorrectFootnoteControl_WhenApiResponseHasTermsAndConditionsCarriers()
        {
            List<string> tAndC = new List<string>() { "FEDEX", "UPS" };
            List<string> tAndC2 = new List<string>() { "USPS" };
            GetEligibleShippingServicesResponse response = GetEligibleShippingServicesResponse(tAndC, tAndC2);

            mock.Mock<IAmazonShippingWebClient>()
                .Setup(w => w.GetRates(It.IsAny<ShipmentRequestDetails>()))
                .Returns(response);

            AmazonRatingService testObject = mock.Create<AmazonRatingService>();

            RateGroup result = testObject.GetRates(SampleShipmentAmazonOrer(AmazonMwsIsPrime.Yes));

            AmazonCarrierTermsAndConditionsNotAcceptedFootnoteControl footnoteControl = result.FootnoteFactories.OfType<AmazonCarrierTermsAndConditionsNotAcceptedFootnoteFactory>().First().CreateFootnote(null) as AmazonCarrierTermsAndConditionsNotAcceptedFootnoteControl;
            Assert.Equal(3, footnoteControl.CarrierNames.Count());
            Assert.True(footnoteControl.CarrierNames.Contains("FEDEX"));
            Assert.True(footnoteControl.CarrierNames.Contains("UPS"));
            Assert.True(footnoteControl.CarrierNames.Contains("USPS"));
        }

        [Theory]
        [InlineData(AmazonMwsIsPrime.No)]
        [InlineData(AmazonMwsIsPrime.Unknown)]
        public void GetRates_ReturnsAmazonShippingException_WhenNotPrimeOrder(AmazonMwsIsPrime isPrime)
        {
            AmazonRatingService testObject = mock.Create<AmazonRatingService>();

            Assert.Throws<AmazonShippingException>(() => testObject.GetRates(SampleShipmentAmazonOrer(isPrime)));
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
            RateGroup filteredRate = null;

            GetEligibleShippingServicesResponse response = ResponseWithService(new ShippingService
            {
                Rate = new Rate { Amount = 2.34m },
                ShippingServiceName = "UPS",
                ShippingServiceId = "Foo",
                ShippingServiceOfferId = "Bar"
            });

            mock.Mock<IAmazonRateGroupFilter>()
                .Setup(x => x.Filter(It.IsAny<RateGroup>()))
                .Callback<RateGroup>(x => filteredRate = x);

            mock.Mock<IAmazonShippingWebClient>()
                .Setup(w => w.GetRates(It.IsAny<ShipmentRequestDetails>()))
                .Returns(response);

            AmazonRatingService testObject = mock.Create<AmazonRatingService>();

            testObject.GetRates(SampleShipmentAmazonOrer(AmazonMwsIsPrime.Yes));

            Assert.Equal(1, filteredRate.Rates.Count);
            Assert.Equal(2.34m, filteredRate.Rates.First().Amount);
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
            ShipmentEntity shipment = SampleShipmentAmazonOrer(AmazonMwsIsPrime.Yes);
            configureInput(shipment, shipment.Order as AmazonOrderEntity);

            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IAmazonShippingWebClient>()
                    .Setup(w => w.GetRates(It.IsAny<ShipmentRequestDetails>()))
                    .Returns(ResponseWithService(new ShippingService()));

                AmazonRatingService testObject = mock.Create<AmazonRatingService>();

                testObject.GetRates(shipment);

                mock.Mock<IAmazonShippingWebClient>()
                    .Verify(x => x.GetRates(It.Is(verifyCall)));
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

        public ShipmentEntity SampleShipmentAmazonOrer(AmazonMwsIsPrime isPrime)
        {
            return new ShipmentEntity()
            {
                Order = new AmazonOrderEntity()
                {
                    AmazonOrderID = "10",
                    IsPrime = (int)isPrime
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
                    IsPrime = (int)isPrime,
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
