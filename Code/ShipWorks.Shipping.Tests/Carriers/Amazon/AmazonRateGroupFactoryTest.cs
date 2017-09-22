﻿using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Carriers.Amazon.Api;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Shipping.Carriers.Amazon.RateGroupFilters;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using Xunit;
using ShipWorks.Tests.Shared;

namespace ShipWorks.Shipping.Tests.Carriers.Amazon
{
    public class AmazonRateGroupFactoryTest
    {
        private readonly AutoMock mock;

        public AmazonRateGroupFactoryTest()
        {
            mock = AutoMock.GetFromRepository(new MockRepository(MockBehavior.Loose) { DefaultValue = DefaultValue.Mock });
            mock.Mock<IAmazonRateGroupFilter>()
                .Setup(x => x.Filter(It.IsAny<RateGroup>()))
                .Returns<RateGroup>(x => x);

            mock.Mock<IAmazonServiceTypeRepository>()
                .Setup(f => f.Get())
                .Returns(new List<AmazonServiceTypeEntity>()
                {
                    new AmazonServiceTypeEntity()
                    {
                        AmazonServiceTypeID = 1,
                        ApiValue = null
                    }
                });
        }

        [Fact]
        public void GetRateGroupFromResponse_ReturnsValidRate_WhenApiReturnsOneValidService()
        {
            GetEligibleShippingServicesResponse response = ResponseWithService(new ShippingService
            {
                Rate = new Rate { Amount = 2.34M },
                ShippingServiceName = "UPS",
                ShippingServiceId = "Foo",
                ShippingServiceOfferId = "Bar"
            });

            mock.Mock<IAmazonServiceTypeRepository>()
                .Setup(f => f.Get())
                .Returns(new List<AmazonServiceTypeEntity>()
                {
                                new AmazonServiceTypeEntity()
                                {
                                    AmazonServiceTypeID = 1,
                                    ApiValue = "Foo"
                                }
                });

            AmazonRateGroupFactory testObject = mock.Create<AmazonRateGroupFactory>();

            RateGroup result = testObject.GetRateGroupFromResponse(response);
            RateResult rateResult = result.Rates.FirstOrDefault();

            Assert.Equal("UPS", rateResult.Description);
            Assert.Equal(2.34M, rateResult.AmountOrDefault);
            Assert.Equal("Foo", ((AmazonRateTag) rateResult.Tag).ShippingServiceId);
        }

        [Fact]
        public void GetRateGroupFromResponse_ReturnsThreeRates_WhenApiResponseHasThreeServices()
        {
            GetEligibleShippingServicesResponse response = ResponseWithServices(() => Enumerable.Range(1, 3)
                .Select(x => new ShippingService { Rate = new Rate { Amount = x } }));

            AmazonRateGroupFactory testObject = mock.Create<AmazonRateGroupFactory>();

            RateGroup result = testObject.GetRateGroupFromResponse(response);

            Assert.Equal(3, result.Rates.Count);
        }

        [Fact]
        public void GetRateGroupFromResponse_ReturnsRatesSortedByPrice()
        {
            GetEligibleShippingServicesResponse response = new GetEligibleShippingServicesResponse
            {
                GetEligibleShippingServicesResult = new GetEligibleShippingServicesResult
                {
                    ShippingServiceList = new ShippingServiceList
                    {
                        ShippingService = new List<ShippingService>
                        {
                            new ShippingService {Rate = new Rate() { Amount = 1.249M, CurrencyCode = "US"}},
                            new ShippingService {Rate = new Rate() { Amount = 10.0M, CurrencyCode = "US"}},
                            new ShippingService {Rate = new Rate() { Amount = 0.0M, CurrencyCode = "US"}},
                            new ShippingService {Rate = new Rate() { Amount = -0.04M, CurrencyCode = "US"}},
                            new ShippingService {Rate = new Rate() { Amount = 1.24M, CurrencyCode = "US"}},
                            new ShippingService {Rate = new Rate() { Amount = -1.0M, CurrencyCode = "US"}}
                        }
                    }
                }
            };

            AmazonRateGroupFactory testObject = mock.Create<AmazonRateGroupFactory>();

            RateGroup result = testObject.GetRateGroupFromResponse(response);

            Assert.Equal(-1.0M, result.Rates[0].AmountOrDefault);
            Assert.Equal(-0.04M, result.Rates[1].AmountOrDefault);
            Assert.Equal(0.0M, result.Rates[2].AmountOrDefault);
            Assert.Equal(1.24M, result.Rates[3].AmountOrDefault);
            Assert.Equal(1.249M, result.Rates[4].AmountOrDefault);
            Assert.Equal(10.0M, result.Rates[5].AmountOrDefault);
        }

        [Fact]
        public void GetRateGroupFromResponse_ReturnsTermsAndConditionsFootNoteFactory_WhenApiResponseHasTermsAndConditionsCarriers()
        {
            List<string> tAndC = new List<string>() { "FEDEX", "UPS" };
            List<string> tAndC2 = new List<string>() { "USPS" };
            GetEligibleShippingServicesResponse response = GetEligibleShippingServicesResponse(tAndC, tAndC2);

            mock.Mock<IAmazonShippingWebClient>()
                .Setup(w => w.GetRates(It.IsAny<ShipmentRequestDetails>(), It.IsAny<AmazonShipmentEntity>()))
                .Returns(response);

            AmazonRateGroupFactory testObject = mock.Create<AmazonRateGroupFactory>();

            RateGroup result = testObject.GetRateGroupFromResponse(response);

            Assert.Equal(0, result.Rates.Count);
            Assert.Equal(1, result.FootnoteFactories.OfType<AmazonCarrierTermsAndConditionsNotAcceptedFootnoteFactory>().Count());
        }

        [Fact]
        public void GetRateGroupFromResponse_TermsAndConditionsFootNoteFactory_CreatesCorrectFootnoteControl_WhenApiResponseHasTermsAndConditionsCarriers()
        {
            List<string> tAndC = new List<string>() { "FEDEX", "UPS" };
            List<string> tAndC2 = new List<string>() { "USPS" };
            GetEligibleShippingServicesResponse response = GetEligibleShippingServicesResponse(tAndC, tAndC2);

            AmazonRateGroupFactory testObject = mock.Create<AmazonRateGroupFactory>();

            RateGroup result = testObject.GetRateGroupFromResponse(response);

            AmazonCarrierTermsAndConditionsNotAcceptedFootnoteControl footnoteControl = result.FootnoteFactories.OfType<AmazonCarrierTermsAndConditionsNotAcceptedFootnoteFactory>().First().CreateFootnote(null) as AmazonCarrierTermsAndConditionsNotAcceptedFootnoteControl;
            Assert.Equal(3, footnoteControl.CarrierNames.Count());
            Assert.True(footnoteControl.CarrierNames.Contains("FEDEX"));
            Assert.True(footnoteControl.CarrierNames.Contains("UPS"));
            Assert.True(footnoteControl.CarrierNames.Contains("USPS"));
        }

        [Fact]
        public void GetRates_ReturnsRateWithDefaultCarrierName_WhenApiReturnsOneServiceWithNullCarrierName()
        {
            GetEligibleShippingServicesResponse response = ResponseWithService(new ShippingService
            {
                Rate = new Rate { Amount = 1 },
                ShippingServiceName = null
            });

            AmazonRateGroupFactory testObject = mock.Create<AmazonRateGroupFactory>();
            
            RateGroup result = testObject.GetRateGroupFromResponse(response);
            RateResult rateResult = result.Rates.FirstOrDefault();

            Assert.Equal("Unknown", rateResult.Description);
        }

        [Fact]
        public void GetRateGroupFromResponse_RatesArePassedToFilters()
        {
            RateGroup filteredRate = null;

            GetEligibleShippingServicesResponse response = ResponseWithServices(() => Enumerable.Range(1, 3)
                .Select(x => new ShippingService{ Rate = new Rate { Amount = x },
                    ShippingServiceName = x == 1 ? "UPS" : "FedEx",
                    ShippingServiceId = "Foo",
                    ShippingServiceOfferId = "Bar",
                    CarrierName = x == 1 ? "UPS" : "FedEx"
                }));

            mock.Mock<IAmazonServiceTypeRepository>()
                .Setup(f => f.Get())
                .Returns(new List<AmazonServiceTypeEntity>()
                {
                    new AmazonServiceTypeEntity()
                    {
                        AmazonServiceTypeID = 1,
                        ApiValue = "Foo"
                    }
                });

            mock.Mock<IAmazonRateGroupFilter>()
                .Setup(x => x.Filter(It.IsAny<RateGroup>()))
                .Callback<RateGroup>(x =>
                {
                    filteredRate = new RateGroup(x.Rates.Where(r => r.Description != "UPS"));
                });

            AmazonRateGroupFactory testObject = mock.Create<AmazonRateGroupFactory>();

            testObject.GetRateGroupFromResponse(response);

            Assert.Equal(2, filteredRate.Rates.Count);
            Assert.Equal(2M, filteredRate.Rates.First().AmountOrDefault);
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

        public ShipmentEntity SampleShipmentAmazonOrer(AmazonMwsIsPrime isPrime)
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
    }
}