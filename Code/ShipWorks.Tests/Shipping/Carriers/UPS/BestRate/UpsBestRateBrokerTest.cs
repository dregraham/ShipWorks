﻿using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.BestRate;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.Promo;
using ShipWorks.Shipping.Carriers.UPS.UpsEnvironment;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Insurance;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.BestRate
{
    public class UpsBestRateBrokerTest
    {
        private UpsAccountEntity account1;
        private UpsAccountEntity account2;
        private UpsAccountEntity account3;
        private RateGroup rateGroup1;
        private RateGroup rateGroup2;
        private RateGroup rateGroup3;
        private RateResult account1Rate1;
        private RateResult account1Rate2;
        private RateResult account1Rate3;
        private RateResult account2Rate1;
        private RateResult account3Rate1;
        private RateResult account3Rate2;
        private UpsBestRateBroker testObject;
        private ShipmentEntity testShipment;

        /// <summary>
        /// This holds a collection of all the shipment objects that were passed into the UpsShipmentType.GetRates method
        /// </summary>
        private List<ShipmentEntity> getRatesShipments;

        private Mock<ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity>> genericRepositoryMock;
        private Mock<UpsShipmentType> genericShipmentTypeMock;
        private Dictionary<long, RateGroup> rateResults;
        private int timesGetRatesCalled = 0;
        private Mock<IBestRateExcludedAccountRepository> bestRateExludedAccountRepositoryMock;

        public UpsBestRateBrokerTest()
        {
            account1 = new UpsAccountEntity { UpsAccountID = 1, CountryCode = "US", AccountNumber = "1"};
            account2 = new UpsAccountEntity { UpsAccountID = 2, CountryCode = "US", AccountNumber = "2"};
            account3 = new UpsAccountEntity { UpsAccountID = 3, CountryCode = "US", AccountNumber = "3"};

            account1Rate1 = new RateResult("Account 1a", "4", 12, UpsServiceType.Ups2DayAir) { ServiceLevel = ServiceLevelType.TwoDays };
            account1Rate2 = new RateResult("Account 1b", "3", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.FourToSevenDays };
            account1Rate3 = new RateResult("Account 1c", "1", 15, UpsServiceType.UpsNextDayAir) { ServiceLevel = ServiceLevelType.OneDay };
            account2Rate1 = new RateResult("* No rates were returned for the selected Service.", "");
            account3Rate1 = new RateResult("Account 3a", "4", 3, UpsServiceType.UpsExpressSaver) { ServiceLevel = ServiceLevelType.Anytime };
            account3Rate2 = new RateResult("Account 3b", "3", 10, UpsServiceType.Ups2DayAirAM) { ServiceLevel = ServiceLevelType.TwoDays };

            rateGroup1 = new RateGroup(new[] { account1Rate1, account1Rate2, account1Rate3 });
            rateGroup2 = new RateGroup(new[] { account2Rate1 });
            rateGroup3 = new RateGroup(new[] { account3Rate1, account3Rate2 });

            rateResults = new Dictionary<long, RateGroup>
                {
                    {1, rateGroup1},
                    {2, rateGroup2},
                    {3, rateGroup3},
                };

            genericRepositoryMock = new Mock<ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity>>();
            genericRepositoryMock.Setup(x => x.Accounts)
                                 .Returns(new List<UpsAccountEntity> { account1, account2, account3 });
            genericRepositoryMock.Setup(x => x.DefaultProfileAccount)
                                 .Returns(account2);

            getRatesShipments = new List<ShipmentEntity>();

            // Save a copy of all the shipment entities passed into the GetRates method so we can inspect them later
            genericShipmentTypeMock = new Mock<UpsShipmentType>();
            genericShipmentTypeMock.Setup(x => x.ShipmentTypeCode).Returns(ShipmentTypeCode.UpsOnLineTools);

            genericShipmentTypeMock.Setup(x => x.ConfigureNewShipment(It.IsAny<ShipmentEntity>()))
                .Callback<ShipmentEntity>(x =>
                {
                    x.Ups = new UpsShipmentEntity();
                    x.Ups.Packages.Add(new UpsPackageEntity());
                });

            bestRateExludedAccountRepositoryMock = new Mock<IBestRateExcludedAccountRepository>();
            bestRateExludedAccountRepositoryMock.Setup(r => r.GetAll()).Returns(new List<long>());

            testObject = new UpsBestRateBroker(genericShipmentTypeMock.Object, genericRepositoryMock.Object, new UpsSettingsRepository(), bestRateExludedAccountRepositoryMock.Object)
            {
                GetRatesAction = (shipment, type) =>
                {
                    getRatesShipments.Add(EntityUtility.CloneEntity(shipment));
                    timesGetRatesCalled++;
                    return rateResults[shipment.Ups.UpsAccountID];
                }
            };

            testShipment = new ShipmentEntity { ShipmentType = (int) ShipmentTypeCode.BestRate, ContentWeight = 12.1, BestRate = new BestRateShipmentEntity(), OriginCountryCode = "US" };
        }

        [Fact]
        public void HasAccounts_DelegatesToAccountRepository()
        {
            bool hasAccounts = testObject.HasAccounts;

            genericRepositoryMock.Verify(r => r.AccountsReadOnly, Times.Once());
        }

        [Fact]
        public void HasAccounts_ReturnsTrue_WhenRepositoryHasMoreThanZeroAccounts()
        {
            genericRepositoryMock.Setup(r => r.AccountsReadOnly).Returns(new List<UpsAccountEntity> { new UpsAccountEntity() });

            bool hasAccounts = testObject.HasAccounts;

            Assert.True(hasAccounts);
        }

        [Fact]
        public void HasAccounts_ReturnsFalse_WhenRepositoryHaSZeroAccounts()
        {
            genericRepositoryMock.Setup(r => r.AccountsReadOnly).Returns(new List<UpsAccountEntity>());

            bool hasAccounts = testObject.HasAccounts;

            Assert.False(hasAccounts);
        }

        [Fact]
        public void GetBestRates_RetrievesAllAccounts()
        {
            testObject.GetBestRates(testShipment, new List<BrokerException>());

            bestRateExludedAccountRepositoryMock.Verify(x => x.GetAll());
            genericRepositoryMock.VerifyGet(x => x.Accounts);

            IEnumerable<long> accountIDs = getRatesShipments.Select(x => x.Ups.UpsAccountID).Distinct();

            Assert.Equal(3, accountIDs.Count());
        }

        [Fact]
        public void GetBestRates_CallsConfigureNewShipmentForEachAccount()
        {
            testObject.GetBestRates(testShipment, new List<BrokerException>());

            genericShipmentTypeMock.Verify(x => x.ConfigureNewShipment(It.IsAny<ShipmentEntity>()), Times.Exactly(3));
        }

        [Fact]
        public void GetBestRates_CallsGetRatesForEachAccount()
        {
            testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.Equal(3, timesGetRatesCalled);

            foreach (var shipment in getRatesShipments)
            {
                Assert.Equal(ShipmentTypeCode.UpsOnLineTools, (ShipmentTypeCode) shipment.ShipmentType);
                Assert.Equal(12.1, shipment.ContentWeight);
            }
        }

        [Fact]
        public void GetBestRates_ReturnsTwoRates_WhenTwoRatesHaveSameTypeLevelAndPrice()
        {
            rateGroup2.Rates.Clear();

            RateResult result1 = new RateResult("Account 1a", "4", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.FourToSevenDays };
            RateResult result2 = new RateResult("Account 1b", "3", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.FourToSevenDays };

            rateGroup2.Rates.Add(result1);
            rateGroup2.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.True(rates.Rates.Contains(result1));
        }

        [Fact]
        public void GetBestRates_ReturnsTwoRates_WhenTwoRatesHaveSameTypeLevel()
        {
            rateGroup2.Rates.Clear();

            RateResult result1 = new RateResult("Account 1a", "4", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.FourToSevenDays };
            RateResult result2 = new RateResult("Account 1b", "3", 2, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.FourToSevenDays };

            rateGroup2.Rates.Add(result1);
            rateGroup2.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.True(rates.Rates.Contains(result2));
        }

        [Fact]
        public void GetBestRates_ReturnsBothRates_WhenTwoRatesHaveSameTypeAndPriceButDifferentLevels()
        {
            rateGroup2.Rates.Clear();

            RateResult result1 = new RateResult("Account 1a", "4", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.TwoDays };
            RateResult result2 = new RateResult("Account 1b", "3", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.FourToSevenDays };

            rateGroup2.Rates.Add(result1);
            rateGroup2.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.True(rates.Rates.Contains(result1));
            Assert.True(rates.Rates.Contains(result2));
        }

        [Fact]
        public void GetBestRates_ReturnsBothRates_WhenTwoRatesHaveSameLevelAndPriceButDifferentType()
        {
            rateGroup2.Rates.Clear();

            RateResult result1 = new RateResult("Account 1a", "4", 4, UpsServiceType.UpsNextDayAir) { ServiceLevel = ServiceLevelType.OneDay };
            RateResult result2 = new RateResult("Account 1b", "3", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.OneDay };

            rateGroup2.Rates.Add(result1);
            rateGroup2.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.True(rates.Rates.Contains(result1));
            Assert.True(rates.Rates.Contains(result2));
        }

        [Fact]
        public void GetBestRates_OriginalUpsShipmentDetailsAreRestoredAfterCall()
        {
            UpsShipmentEntity upsShipment = new UpsShipmentEntity();
            testShipment.Ups = upsShipment;
            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.Equal(upsShipment, testShipment.Ups);
        }

        [Fact]
        public void GetBestRates_CallsHandler_WhenShippingExceptionIsThrown()
        {
            ShippingException exception = new ShippingException();
            List<BrokerException> brokerExceptions = new List<BrokerException>();

            testObject.GetRatesAction = (shipment, type) =>
            {
                if (shipment.Ups.UpsAccountID == 2) throw exception;
                return rateResults[shipment.Ups.UpsAccountID];
            };

            testObject.GetBestRates(testShipment, brokerExceptions);

            Assert.Equal(exception, brokerExceptions.First().InnerException);
        }

        [Fact]
        public void GetBestRates_SetsPackageDetails()
        {
            testShipment.BestRate.DimsHeight = 3;
            testShipment.BestRate.DimsWidth = 5;
            testShipment.BestRate.DimsLength = 2;

            testObject.GetBestRates(testShipment, new List<BrokerException>());

            foreach (ShipmentEntity shipment in getRatesShipments)
            {
                Assert.Equal(3, shipment.Ups.Packages[0].DimsHeight);
                Assert.Equal(5, shipment.Ups.Packages[0].DimsWidth);
                Assert.Equal(2, shipment.Ups.Packages[0].DimsLength);
                Assert.False(shipment.Ups.Packages[0].DimsAddWeight);
                Assert.Equal(12.1, shipment.Ups.Packages[0].Weight);
            }
        }

        [Fact]
        public void GetBestRates_OverridesProfilePackagingType()
        {
            genericShipmentTypeMock.Setup(x => x.ConfigureNewShipment(It.IsAny<ShipmentEntity>()))
                                   .Callback<ShipmentEntity>(x =>
                                       {
                                           x.Ups.Service = (int) UpsServiceType.UpsMailInnovationsExpedited;
                                           x.Ups.Packages.Add(new UpsPackageEntity { PackagingType = (int) UpsPackagingType.Tube });
                                       });

            testObject.GetBestRates(testShipment, new List<BrokerException>());

            foreach (ShipmentEntity shipment in getRatesShipments)
            {
                Assert.Equal(UpsPackagingType.Custom, (UpsPackagingType) shipment.Ups.Packages[0].PackagingType);
            }
        }

        [Fact]
        public void GetBestRates_OverridesProfileAccount()
        {
            genericShipmentTypeMock.Setup(x => x.ConfigureNewShipment(It.IsAny<ShipmentEntity>()))
                                   .Callback<ShipmentEntity>(x =>
                                   {
                                       x.Ups.UpsAccountID = 999;
                                       x.Ups.Packages.Add(new UpsPackageEntity());
                                   });

            testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.True(getRatesShipments.Any(x => x.Ups.UpsAccountID == 1));
            Assert.True(getRatesShipments.Any(x => x.Ups.UpsAccountID == 2));
            Assert.True(getRatesShipments.Any(x => x.Ups.UpsAccountID == 3));
            Assert.False(getRatesShipments.Any(x => x.Ups.UpsAccountID == 999));
        }

        [Fact]
        public void GetBestRates_OverridesProfileWeight()
        {
            genericShipmentTypeMock.Setup(x => x.ConfigureNewShipment(It.IsAny<ShipmentEntity>()))
                                   .Callback<ShipmentEntity>(x =>
                                       {
                                           x.ContentWeight = 123;
                                           x.Ups.Packages.Add(new UpsPackageEntity());
                                       });

            testObject.GetBestRates(testShipment, new List<BrokerException>());

            foreach (ShipmentEntity shipment in getRatesShipments)
            {
                Assert.Equal(12.1, shipment.Ups.Packages[0].Weight);
            }
        }

        [Fact]
        public void GetBestRates_ReturnsRatesAsRateResults()
        {
            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            foreach (var rate in rates.Rates)
            {
                Assert.IsAssignableFrom<RateResult>(rate);
            }
        }

        [Fact]
        public void GetBestRates_SetsTagToBestRateResultTag()
        {
            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            foreach (var rate in rates.Rates)
            {
                BestRateResultTag tag = rate.Tag as BestRateResultTag;

                if (tag == null)
                {
                    Assert.False(true);
                }

                Assert.IsAssignableFrom<Action<ShipmentEntity>>(tag.RateSelectionDelegate);
                Assert.NotNull(tag.OriginalTag);
                Assert.NotNull(tag.ResultKey);
            }
        }

        [Fact]
        public void GetBestRates_SetsTagResultKeyToCarrierAndServiceType()
        {
            rateGroup2.Rates.Clear();

            rateGroup2.Rates.Add(new RateResult("Account 1b", "3", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.FourToSevenDays });
            rateGroup2.Rates.Add(new RateResult("Account 1c", "1", 15, UpsServiceType.UpsNextDayAir) { ServiceLevel = ServiceLevelType.OneDay });

            RateGroup rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            var resultKeys = rates.Rates.Select(x => x.Tag).Cast<BestRateResultTag>().Select(x => x.ResultKey).ToList();

            Assert.True(resultKeys.Contains("UPSUpsGround"));
            Assert.True(resultKeys.Contains("UPSUpsNextDayAir"));
        }

        [Fact]
        public void GetBestRates_AddsUPSToDescription_WhenItDoesNotAlreadyExist()
        {
            rateGroup2.Rates.Clear();

            RateResult result1 = new RateResult("UPS Ground", "4", 4, UpsServiceType.UpsNextDayAir) { ServiceLevel = ServiceLevelType.OneDay };
            RateResult result2 = new RateResult("Some Service", "3", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.OneDay };

            rateGroup2.Rates.Add(result1);
            rateGroup2.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.True(rates.Rates.Select(x => x.Description).Contains("UPS Ground"));
            Assert.True(rates.Rates.Select(x => x.Description).Contains("UPS Some Service"));
        }

        [Fact]
        public void GetBestRates_AddsBrokerException_WhenExceptionsRateFootnoteFactoryIsReturnedFromBase()
        {
            List<BrokerException> brokerExceptions = new List<BrokerException>();

            RateGroup rateGroup = new RateGroup(new List<RateResult>());
            rateGroup.AddFootnoteFactory(new ExceptionsRateFootnoteFactory(ShipmentTypeCode.UpsOnLineTools, new Exception("blah")));

            testObject.GetRatesAction = (shipment, type) => rateGroup;

            testObject.GetBestRates(testShipment, brokerExceptions);

            Assert.Equal(1, brokerExceptions.Count);
            Assert.Equal("blah", brokerExceptions.Single().Message);
        }

        [Fact]
        public void GetBestRates_AddsBrokerException_WhenSurePostCanBeUsed_AndSurePostRatesAreNotIncluded()
        {
            List<BrokerException> brokerExceptions = new List<BrokerException>();

            genericRepositoryMock.Setup(r => r.Accounts).Returns(new List<UpsAccountEntity> { account1 });

            RateResult rate1 = new RateResult("Account 1", "4", 12, UpsServiceType.Ups2DayAir) { ServiceLevel = ServiceLevelType.TwoDays };
            RateResult rate2 = new RateResult("Account 1", "3", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.FourToSevenDays };
            RateResult rate3 = new RateResult("Account 1", "1", 15, UpsServiceType.UpsNextDayAir) { ServiceLevel = ServiceLevelType.OneDay };

            RateGroup rateGroup = new RateGroup(new[] { rate1, rate2, rate3 });

            testObject.GetRatesAction = (shipment, type) => rateGroup;

            // Setup the broker settings to indicate we can use SurePost (and we don't have an exception added for MI)
            Mock<IBestRateBrokerSettings> settings = new Mock<IBestRateBrokerSettings>();
            settings.Setup(s => s.IsMailInnovationsAvailable(It.IsAny<ShipmentType>())).Returns(false);
            settings.Setup(s => s.CanUseSurePost()).Returns(true);

            testObject.Configure(settings.Object);
            testObject.GetBestRates(testShipment, brokerExceptions);

            Assert.Equal(1, brokerExceptions.Count);
        }

        [Fact]
        public void GetBestRates_BrokerExceptionMessageIndicatesSurePostRatesWereNotReceived_WhenSurePostCanBeUsed_AndSurePostRatesAreNotIncluded()
        {
            List<BrokerException> brokerExceptions = new List<BrokerException>();


            genericRepositoryMock.Setup(r => r.Accounts).Returns(new List<UpsAccountEntity> { account1 });

            RateResult rate1 = new RateResult("Account 1", "4", 12, UpsServiceType.Ups2DayAir) { ServiceLevel = ServiceLevelType.TwoDays };
            RateResult rate2 = new RateResult("Account 1", "3", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.FourToSevenDays };
            RateResult rate3 = new RateResult("Account 1", "1", 15, UpsServiceType.UpsNextDayAir) { ServiceLevel = ServiceLevelType.OneDay };

            RateGroup rateGroup = new RateGroup(new[] { rate1, rate2, rate3 });
            testObject.GetRatesAction = (shipment, type) => rateGroup;

            // Setup the broker settings to indicate we can use SurePost (and we don't have an exception added for MI)
            Mock<IBestRateBrokerSettings> settings = new Mock<IBestRateBrokerSettings>();
            settings.Setup(s => s.IsMailInnovationsAvailable(It.IsAny<ShipmentType>())).Returns(false);
            settings.Setup(s => s.CanUseSurePost()).Returns(true);

            testObject.Configure(settings.Object);
            testObject.GetBestRates(testShipment, brokerExceptions);

            Assert.Equal("UPS did not provide SurePost rates.", brokerExceptions[0].Message);
        }

        [Fact]
        public void GetBestRates_BrokerExceptionSeverityLevelIsWarning_WhenSurePostCanBeUsed_AndSurePostRatesAreNotIncluded()
        {
            List<BrokerException> brokerExceptions = new List<BrokerException>();

            genericRepositoryMock.Setup(r => r.Accounts).Returns(new List<UpsAccountEntity> { account1 });

            RateResult rate1 = new RateResult("Account 1", "4", 12, UpsServiceType.Ups2DayAir) { ServiceLevel = ServiceLevelType.TwoDays };
            RateResult rate2 = new RateResult("Account 1", "3", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.FourToSevenDays };
            RateResult rate3 = new RateResult("Account 1", "1", 15, UpsServiceType.UpsNextDayAir) { ServiceLevel = ServiceLevelType.OneDay };

            RateGroup rateGroup = new RateGroup(new[] { rate1, rate2, rate3 });
            testObject.GetRatesAction = (shipment, type) => rateGroup;

            // Setup the broker settings to indicate we can use SurePost (and we don't have an exception added for MI)
            Mock<IBestRateBrokerSettings> settings = new Mock<IBestRateBrokerSettings>();
            settings.Setup(s => s.IsMailInnovationsAvailable(It.IsAny<ShipmentType>())).Returns(false);
            settings.Setup(s => s.CanUseSurePost()).Returns(true);

            testObject.Configure(settings.Object);
            testObject.GetBestRates(testShipment, brokerExceptions);

            Assert.Equal(BrokerExceptionSeverityLevel.Warning, brokerExceptions[0].SeverityLevel);
        }

        [Fact]
        public void GetBestRates_DoesNotIncludeBrokerException_WhenSurePostCannotBeUsed_AndSurePostRatesAreNotIncluded()
        {
            List<BrokerException> brokerExceptions = new List<BrokerException>();

            genericRepositoryMock.Setup(r => r.Accounts).Returns(new List<UpsAccountEntity> { account1 });

            RateResult rate1 = new RateResult("Account 1", "4", 12, UpsServiceType.Ups2DayAir) { ServiceLevel = ServiceLevelType.TwoDays };
            RateResult rate2 = new RateResult("Account 1", "3", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.FourToSevenDays };
            RateResult rate3 = new RateResult("Account 1", "1", 15, UpsServiceType.UpsNextDayAir) { ServiceLevel = ServiceLevelType.OneDay };

            RateGroup rateGroup = new RateGroup(new[] { rate1, rate2, rate3 });
            testObject.GetRatesAction = (shipment, type) => rateGroup;

            // Setup the broker settings to indicate we cannot use SurePost (and we don't have an exception added for MI)
            Mock<IBestRateBrokerSettings> settings = new Mock<IBestRateBrokerSettings>();
            settings.Setup(s => s.IsMailInnovationsAvailable(It.IsAny<ShipmentType>())).Returns(false);
            settings.Setup(s => s.CanUseSurePost()).Returns(false);

            testObject.Configure(settings.Object);
            testObject.GetBestRates(testShipment, brokerExceptions);

            Assert.Equal(0, brokerExceptions.Count);
        }

        [Fact]
        public void GetBestRates_DoesNotIncludeBrokerException_WhenSurePostCanBeUsed_AndSurePostRatesAreIncluded()
        {
            List<BrokerException> brokerExceptions = new List<BrokerException>();

            genericRepositoryMock.Setup(r => r.Accounts).Returns(new List<UpsAccountEntity> { account1 });

            // Include a rate with a sure post service for this test
            RateResult rate1 = new RateResult("Account 1", "4", 12, UpsServiceType.Ups2DayAir) { ServiceLevel = ServiceLevelType.TwoDays };
            RateResult rate2 = new RateResult("Account 1", "3", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.FourToSevenDays };
            RateResult rate3 = new RateResult("Account 1", "1", 15, UpsServiceType.UpsNextDayAir) { ServiceLevel = ServiceLevelType.OneDay };
            RateResult rate4 = new RateResult("Account 1", "1", 15, UpsServiceType.UpsSurePost1LbOrGreater) { ServiceLevel = ServiceLevelType.OneDay };

            RateGroup rateGroup = new RateGroup(new[] { rate1, rate2, rate3, rate4 });
            testObject.GetRatesAction = (shipment, type) => rateGroup;

            // Setup the broker settings to indicate we cannot use SurePost (and we don't have an exception added for MI)
            Mock<IBestRateBrokerSettings> settings = new Mock<IBestRateBrokerSettings>();
            settings.Setup(s => s.IsMailInnovationsAvailable(It.IsAny<ShipmentType>())).Returns(false);
            settings.Setup(s => s.CanUseSurePost()).Returns(false);

            testObject.Configure(settings.Object);
            testObject.GetBestRates(testShipment, brokerExceptions);

            Assert.Equal(0, brokerExceptions.Count);
        }

        [Fact]
        public void GetInsuranceProvider_ReturnsShipWorks_UpsSettingSpecfiesShipWorks()
        {
            Assert.Equal(InsuranceProvider.ShipWorks, testObject.GetInsuranceProvider(new ShippingSettingsEntity() { UpsInsuranceProvider = (int) InsuranceProvider.ShipWorks }));
        }

        [Fact]
        public void GetInsuranceProvider_ReturnsCarrier_UpsSettingSpecfiesCarrier()
        {
            Assert.Equal(InsuranceProvider.Carrier, testObject.GetInsuranceProvider(new ShippingSettingsEntity() { UpsInsuranceProvider = (int) InsuranceProvider.Carrier }));
        }
    }
}
